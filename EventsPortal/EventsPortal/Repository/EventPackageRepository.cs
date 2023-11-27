using EventsPortal.Data;
using EventsPortal.Models;
using EventsPortal.Models.DBObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EventsPortal.Repository
{
    public class EventPackageRepository
    {
        private ApplicationDbContext dbContext;

        public EventPackageRepository()
        {
            this.dbContext = new ApplicationDbContext();
        }

        public EventPackageRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public List<EventPackageModel> GetAllEventPackages()
        {
            List<EventPackageModel> eventPackageList = new List<EventPackageModel>();

            foreach (EventPackage dbEventPackage in dbContext.EventPackages)
            {
                eventPackageList.Add(MapDbObjectToModel(dbEventPackage));
            }
            return eventPackageList;
        }

        public EventPackageModel GetEventPackageByID(Guid ID)
        {
            return MapDbObjectToModel(dbContext.EventPackages.FirstOrDefault(x => x.EventPackageId == ID));
        }

        public void InsertEventPackage(EventPackageModel eventPackageModel)
        {
            eventPackageModel.EventPackageID = Guid.NewGuid();
            dbContext.EventPackages.Add(MapModelToDbObject(eventPackageModel));
            dbContext.SaveChanges();
        }

        public void UpdateEventPackage(EventPackageModel eventPackageModel)
        {
            EventPackage existingEventPackage = dbContext.EventPackages
                .FirstOrDefault(x => x.EventId == eventPackageModel.EventID);

            if (existingEventPackage != null)
            {
                existingEventPackage.PackageId = eventPackageModel.PackageID;
                dbContext.SaveChanges();
            }
            else
            {
                InsertEventPackage(eventPackageModel);
                dbContext.SaveChanges();
            }
        }
        public void DeleteEventPackage(EventPackageModel eventPackageModel)
        {
            EventPackage existingEventPackage = dbContext.EventPackages.FirstOrDefault(x => x.EventPackageId == eventPackageModel.EventPackageID);
            if (existingEventPackage != null)
            {
                dbContext.EventPackages.Remove(existingEventPackage);
                dbContext.SaveChanges();
            }
        }

        private EventPackageModel MapDbObjectToModel(EventPackage dbEventPackage)
        {
            EventPackageModel eventPackageModel = new EventPackageModel();
            if (dbEventPackage != null)
            {
                eventPackageModel.EventPackageID = dbEventPackage.EventPackageId;
                eventPackageModel.EventID = dbEventPackage.EventId;
                eventPackageModel.PackageID = dbEventPackage.PackageId;
            }
            return eventPackageModel;
        }

        private EventPackage MapModelToDbObject(EventPackageModel eventPackageModel)
        {
            EventPackage eventPackage = new EventPackage();

            if (eventPackageModel != null)
            {
                eventPackage.EventPackageId = eventPackageModel.EventPackageID;
                eventPackage.EventId = eventPackageModel.EventID;
                eventPackage.PackageId = eventPackageModel.PackageID;
            }

            return eventPackage;
        }
    }
}
