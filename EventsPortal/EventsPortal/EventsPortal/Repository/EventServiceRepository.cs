using EventsPortal.Data;
using EventsPortal.Models;
using EventsPortal.Models.DBObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EventsPortal.Repository
{
    public class EventServiceRepository
    {
        private ApplicationDbContext dbContext;

        public EventServiceRepository()
        {
            this.dbContext = new ApplicationDbContext();
        }

        public EventServiceRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public List<EventServiceModel> GetAllEventServices()
        {
            List<EventServiceModel> eventServiceList = new List<EventServiceModel>();

            foreach (EventService dbEventService in dbContext.EventServices)
            {
                eventServiceList.Add(MapDbObjectToModel(dbEventService));
            }
            return eventServiceList;
        }

        public EventServiceModel GetEventServiceByID(Guid ID)
        {
            return MapDbObjectToModel(dbContext.EventServices.FirstOrDefault(x => x.EventServiceId == ID));
        }

        public void InsertEventService(EventServiceModel eventServiceModel)
        {
            eventServiceModel.EventServiceID = Guid.NewGuid();
            dbContext.EventServices.Add(MapModelToDbObject(eventServiceModel));
            dbContext.SaveChanges();
        }

        public void UpdateEventService(EventServiceModel eventServiceModel)
        {
            EventService existingEventService = dbContext.EventServices.FirstOrDefault(x => x.EventServiceId == eventServiceModel.EventServiceID);
            if (existingEventService != null)
            {
                existingEventService.EventServiceId = eventServiceModel.EventServiceID;
                existingEventService.EventId = eventServiceModel.EventID;
                existingEventService.ServiceId = (Guid)eventServiceModel.ServiceID;
                dbContext.SaveChanges();
            }
        }

        public void DeleteEventService(EventServiceModel eventServiceModel)
        {
            EventService existingEventService = dbContext.EventServices.FirstOrDefault(x => x.EventServiceId == eventServiceModel.EventServiceID);
            if (existingEventService != null)
            {
                dbContext.EventServices.Remove(existingEventService);
                dbContext.SaveChanges();
            }
        }

        private EventServiceModel MapDbObjectToModel(EventService dbEventService)
        {
            EventServiceModel eventServiceModel = new EventServiceModel();
            if (dbEventService != null)
            {
                eventServiceModel.EventServiceID = dbEventService.EventServiceId;
                eventServiceModel.EventID = dbEventService.EventId;
                eventServiceModel.ServiceID = dbEventService.ServiceId;
            }
            return eventServiceModel;
        }

        private EventService MapModelToDbObject(EventServiceModel eventServiceModel)
        {
            EventService eventService = new EventService();

            if (eventServiceModel != null)
            {
                eventService.EventServiceId = eventServiceModel.EventServiceID;
                eventService.EventId = eventServiceModel.EventID;
                eventService.ServiceId = (Guid)eventServiceModel.ServiceID;
            }

            return eventService;
        }
    }
}
