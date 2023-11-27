using System;
using System.Collections.Generic;

namespace EventsPortal.Models.DBObjects
{
    public partial class Package
    {
        public Package()
        {
            EventPackages = new HashSet<EventPackage>();
        }

        public Guid PackageId { get; set; }
        public string PackageName { get; set; } = null!;
        public string PackageDescription { get; set; } = null!;
        public decimal PricePerParticipant { get; set; }

        public virtual ICollection<EventPackage> EventPackages { get; set; }
    }
}
