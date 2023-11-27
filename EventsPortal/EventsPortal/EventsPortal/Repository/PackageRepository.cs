using EventsPortal.Data;
using EventsPortal.Models;
using EventsPortal.Models.DBObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EventsPortal.Repository
{
    public class PackageRepository
    {
        private ApplicationDbContext dbContext;

        public PackageRepository()
        {
            this.dbContext = new ApplicationDbContext();
        }

        public PackageRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public List<PackageModel> GetAllPackages()
        {
            List<PackageModel> packageList = new List<PackageModel>();

            foreach (Package dbPackage in dbContext.Packages)
            {
                packageList.Add(MapDbObjectToModel(dbPackage));
            }
            return packageList;
        }

        public PackageModel GetPackageByID(Guid ID)
        {
            return MapDbObjectToModel(dbContext.Packages.FirstOrDefault(x => x.PackageId == ID));
        }

        public void InsertPackage(PackageModel packageModel)
        {
            packageModel.PackageID = Guid.NewGuid();
            dbContext.Packages.Add(MapModelToDbObject(packageModel));
            dbContext.SaveChanges();
        }

        public void UpdatePackage(PackageModel packageModel)
        {
            Package existingPackage = dbContext.Packages.FirstOrDefault(x => x.PackageId == packageModel.PackageID);
            if (existingPackage != null)
            {
                existingPackage.PackageId = packageModel.PackageID;
                existingPackage.PackageName = packageModel.PackageName;
                existingPackage.PackageDescription = packageModel.PackageDescription;
                existingPackage.PricePerParticipant = packageModel.PricePerParticipant;
                dbContext.SaveChanges();
            }
        }

        public void DeletePackage(PackageModel packageModel)
        {
            Package existingPackage = dbContext.Packages.FirstOrDefault(x => x.PackageId == packageModel.PackageID);
            if (existingPackage != null)
            {
                dbContext.Packages.Remove(existingPackage);
                dbContext.SaveChanges();
            }
        }

        private PackageModel MapDbObjectToModel(Package dbPackage)
        {
            PackageModel packageModel = new PackageModel();
            if (dbPackage != null)
            {
                packageModel.PackageID = dbPackage.PackageId;
                packageModel.PackageName = dbPackage.PackageName;
                packageModel.PackageDescription = dbPackage.PackageDescription;
                packageModel.PricePerParticipant = dbPackage.PricePerParticipant;
            }
            return packageModel;
        }

        private Package MapModelToDbObject(PackageModel packageModel)
        {
            Package package = new Package();

            if (packageModel != null)
            {
                package.PackageId = packageModel.PackageID;
                package.PackageName = packageModel.PackageName;
                package.PackageDescription = packageModel.PackageDescription;
                package.PricePerParticipant = packageModel.PricePerParticipant;
            }

            return package;
        }
    }
}
