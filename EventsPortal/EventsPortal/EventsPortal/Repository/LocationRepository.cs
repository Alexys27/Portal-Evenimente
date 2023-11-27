using EventsPortal.Data;
using EventsPortal.Models;
using EventsPortal.Models.DBObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EventsPortal.Repository
{
    public class LocationRepository
    {
        private ApplicationDbContext dbContext;

        public LocationRepository()
        {
            this.dbContext = new ApplicationDbContext();
        }

        public LocationRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public List<LocationModel> GetAllLocations()
        {
            List<LocationModel> locationList = new List<LocationModel>();

            foreach (Location dbLocation in dbContext.Locations)
            {
                locationList.Add(MapDbObjectToModel(dbLocation));
            }
            return locationList;
        }

        public LocationModel GetLocationByID(Guid ID)
        {
            Location dbLocation = dbContext.Locations.FirstOrDefault(x => x.LocationId == ID);
            return MapDbObjectToModel(dbLocation);
        }

        public void InsertLocation(LocationModel locationModel)
        {
            locationModel.LocationID = Guid.NewGuid();
            dbContext.Locations.Add(MapModelToDbObject(locationModel));
            dbContext.SaveChanges();
        }

        public void UpdateLocation(LocationModel locationModel)
        {
            Location existingLocation = dbContext.Locations.FirstOrDefault(x => x.LocationId == locationModel.LocationID);
            if (existingLocation != null)
            {
                existingLocation.LocationId = locationModel.LocationID;
                existingLocation.LocationName = locationModel.LocationName;
                existingLocation.LocationAddress = locationModel.LocationAdress;

                // Update image property
                existingLocation.ImagePath = locationModel.ImagePath;

                dbContext.SaveChanges();
            }
        }

        public void DeleteLocation(LocationModel locationModel)
        {
            Location existingLocation = dbContext.Locations.FirstOrDefault(x => x.LocationId == locationModel.LocationID);
            if (existingLocation != null)
            {
                dbContext.Locations.Remove(existingLocation);
                dbContext.SaveChanges();
            }
        }
        private LocationModel MapDbObjectToModel(Location dbLocation)
        {
            LocationModel locationModel = new LocationModel();
            if (dbLocation != null)
            {
                locationModel.LocationID = dbLocation.LocationId;
                locationModel.LocationName = dbLocation.LocationName;
                locationModel.LocationAdress = dbLocation.LocationAddress;

                // Map image property
                locationModel.ImagePath = dbLocation.ImagePath;
            }
            return locationModel;
        }


        private Location MapModelToDbObject(LocationModel locationModel)
        {
            Location location = new Location();

            if (locationModel != null)
            {
                location.LocationId = locationModel.LocationID;
                location.LocationName = locationModel.LocationName;
                location.LocationAddress = locationModel.LocationAdress;

                // Map image property
                location.ImagePath = locationModel.ImagePath;
            }

            return location;
        }
    }
}
