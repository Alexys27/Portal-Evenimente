using EventsPortal.Data;
using EventsPortal.Models;
using EventsPortal.Models.DBObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EventsPortal.Repository
{
    public class EventRepository
    {
        private ApplicationDbContext dbContext;

        public EventRepository()
        {
            this.dbContext = new ApplicationDbContext();
        }

        public EventRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public List<EventModel> GetEventsBySearch(string searchString)
        {
            var events = dbContext.Events
                .Where(e =>
                    e.EventType.Contains(searchString) ||
                    e.Location.LocationName.Contains(searchString)
                )
                .Select(e => new EventModel
                {
                    EventID = e.EventId,
                    EventType = e.EventType,
                    Participants = e.Participants,
                    EventDate = e.EventDate,
                    LocationID = e.LocationId,
                    LocationName = e.Location.LocationName,
                    SelectedPackageName = e.EventPackages.FirstOrDefault().Package.PackageName,
                    SelectedAdditionalServiceNames = e.EventServices.Select(es => es.Service.ServiceName).ToList()
                })
                .ToList();

            return events;
        }
        public List<EventModel> GetAllEvents()
        {
            List<EventModel> eventsTypeList = new List<EventModel>();

            foreach (Events dbEventsType in dbContext.Events)
            {
                eventsTypeList.Add(MapDbObjectToModel(dbEventsType));
            }
            return eventsTypeList;
        }
        public EventModel GetEventsByID(Guid ID)
        {
            var eventModel = dbContext.Events
                .Where(e => e.EventId == ID)
                .Select(e => new EventModel
                {
                    EventID = e.EventId,
                    EventType = e.EventType,
                    Participants = e.Participants,
                    EventDate = e.EventDate,
                    LocationID = e.LocationId,
                    LocationName = e.Location.LocationName, 
                    SelectedPackageName = e.EventPackages.FirstOrDefault().Package.PackageName,
                    SelectedAdditionalServiceNames = e.EventServices.Select(es => es.Service.ServiceName).ToList() 
                })
                .FirstOrDefault();

            return eventModel;
        }

        public void InsertEvent(EventModel eventsModel)
        {
            eventsModel.EventID = Guid.NewGuid();
            dbContext.Events.Add(MapModelToDbObject(eventsModel));
            dbContext.SaveChanges();
        }
        public void UpdateEvents(EventModel eventsModel)
        {
            Events existingEvents = dbContext.Events.FirstOrDefault(x => x.EventId == eventsModel.EventID);
            if (existingEvents != null)
            {
                existingEvents.EventId = eventsModel.EventID;
                existingEvents.EventType = eventsModel.EventType;
                existingEvents.Participants = eventsModel.Participants;
                existingEvents.EventDate = eventsModel.EventDate;
                existingEvents.LocationId = eventsModel.LocationID;
                dbContext.SaveChanges();
            }
        }

        public void DeleteEvents(Guid id)
        {
            // Delete associated event packages from EventPackages
            var eventPackagesToDelete = dbContext.EventPackages.Where(p => p.EventId == id).ToList();
            if (eventPackagesToDelete != null && eventPackagesToDelete.Any())
            {
                dbContext.EventPackages.RemoveRange(eventPackagesToDelete);
                dbContext.SaveChanges();
            }

            // Delete associated event services from EventServices
            var eventServicesToDelete = dbContext.EventServices.Where(es => es.EventId == id).ToList();
            if (eventServicesToDelete != null && eventServicesToDelete.Any())
            {
                dbContext.EventServices.RemoveRange(eventServicesToDelete);
                dbContext.SaveChanges();
            }

            // Delete event from Events
            var eventToDelete = dbContext.Events.FirstOrDefault(x => x.EventId == id);
            if (eventToDelete != null)
            {
                dbContext.Events.Remove(eventToDelete);
                dbContext.SaveChanges();
            }
        }

        private EventModel MapDbObjectToModel(Events dbEvents)
        {
            EventModel eventsModel = new EventModel();
            if (dbEvents != null)
            {
                eventsModel.EventID = dbEvents.EventId;
                eventsModel.EventType = dbEvents.EventType;
                eventsModel.Participants = dbEvents.Participants;
                eventsModel.EventDate = dbEvents.EventDate;
                eventsModel.LocationID = dbEvents.LocationId;
                eventsModel.LocationName = GetLocationNameById(dbEvents.LocationId);
            }
            return eventsModel;
        }

        private string GetLocationNameById(Guid locationId)
        {
            // Implement logic to get LocationName by ID from your data source
            return dbContext.Locations.FirstOrDefault(l => l.LocationId == locationId)?.LocationName;
        }
        private Events MapModelToDbObject(EventModel eventsModel)
        {
            Events events = new Events();

            if (eventsModel != null)
            {
                events.EventId = eventsModel.EventID;
                events.EventType = eventsModel.EventType;
                events.Participants = eventsModel.Participants;
                events.EventDate = eventsModel.EventDate;
                events.LocationId = eventsModel.LocationID;
            }

            return events;
        }
    }
}
