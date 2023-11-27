using System;
using System.Collections.Generic;

namespace EventsPortal.Models.DBObjects
{
    public partial class EventPackage
    {
        public Guid EventPackageId { get; set; }
        public Guid EventId { get; set; }
        public Guid PackageId { get; set; }

        public virtual Events Event { get; set; } = null!;
        public virtual Package Package { get; set; } = null!;
    }
}
