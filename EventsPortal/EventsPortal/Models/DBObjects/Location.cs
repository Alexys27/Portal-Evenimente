using System;
using System.Collections.Generic;

namespace EventsPortal.Models.DBObjects
{
    public partial class Location
    {
        public Location()
        {
            Events = new HashSet<Events>();
        }

        public Guid LocationId { get; set; }
        public string LocationName { get; set; } = null!;
        public string LocationAddress { get; set; } = null!;
        public string? ImagePath { get; set; }
        public virtual ICollection<Events> Events { get; set; }
    }
}
