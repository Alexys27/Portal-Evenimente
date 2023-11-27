using EventsPortal.Data;
using EventsPortal.Models;
using EventsPortal.Models.DBObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EventsPortal.Repository
{
    public class AdditionalServiceRepository
    {
        private ApplicationDbContext dbContext;

        public AdditionalServiceRepository()
        {
            this.dbContext = new ApplicationDbContext();
        }

        public AdditionalServiceRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public List<AdditionalServiceModel> GetAllAdditionalServices()
        {
            List<AdditionalServiceModel> additionalServiceList = new List<AdditionalServiceModel>();

            foreach (AdditionalService dbAdditionalService in dbContext.AdditionalServices)
            {
                additionalServiceList.Add(MapDbObjectToModel(dbAdditionalService));
            }
            return additionalServiceList;
        }

        public AdditionalServiceModel GetAdditionalServiceByID(Guid ID)
        {
            return MapDbObjectToModel(dbContext.AdditionalServices.FirstOrDefault(x => x.ServiceId == ID));
        }

        public void InsertAdditionalService(AdditionalServiceModel additionalServiceModel)
        {
            additionalServiceModel.ServiceID = Guid.NewGuid();
            dbContext.AdditionalServices.Add(MapModelToDbObject(additionalServiceModel));
            dbContext.SaveChanges();
        }

        public void UpdateAdditionalService(AdditionalServiceModel additionalServiceModel)
        {
            AdditionalService existingAdditionalService = dbContext.AdditionalServices.FirstOrDefault(x => x.ServiceId == additionalServiceModel.ServiceID);
            if (existingAdditionalService != null)
            {
                existingAdditionalService.ServiceId = additionalServiceModel.ServiceID;
                existingAdditionalService.ServiceName = additionalServiceModel.ServiceName;
                existingAdditionalService.AdditionalPrice = additionalServiceModel.AdditionalPrice;
                dbContext.SaveChanges();
            }
        }

        public void DeleteAdditionalService(Guid id)
        {    // Find the AdditionalService to be deleted
            AdditionalService existingAdditionalService = dbContext.AdditionalServices.FirstOrDefault(x => x.ServiceId == id);

            if (existingAdditionalService != null)
            {
                // Find and delete related EventServices records
                var relatedEventServices = dbContext.EventServices.Where(es => es.ServiceId == id).ToList();
                dbContext.EventServices.RemoveRange(relatedEventServices);

                // Now, delete the AdditionalService
                dbContext.AdditionalServices.Remove(existingAdditionalService);

                // Save changes
                dbContext.SaveChanges();
            }
        }

        private AdditionalServiceModel MapDbObjectToModel(AdditionalService dbAdditionalService)
        {
            AdditionalServiceModel additionalServiceModel = new AdditionalServiceModel();
            if (dbAdditionalService != null)
            {
                additionalServiceModel.ServiceID = dbAdditionalService.ServiceId;
                additionalServiceModel.ServiceName = dbAdditionalService.ServiceName;
                additionalServiceModel.AdditionalPrice = dbAdditionalService.AdditionalPrice;
            }
            return additionalServiceModel;
        }

        private AdditionalService MapModelToDbObject(AdditionalServiceModel additionalServiceModel)
        {
            AdditionalService additionalService = new AdditionalService();

            if (additionalServiceModel != null)
            {
                additionalService.ServiceId = additionalServiceModel.ServiceID;
                additionalService.ServiceName = additionalServiceModel.ServiceName;
                additionalService.AdditionalPrice = additionalServiceModel.AdditionalPrice;
            }

            return additionalService;
        }
    }
}
