using System;
using System.Collections.Generic;

namespace EventsPortal.Models.DBObjects
{
    public partial class Events
    {
        public Events()
        {
            EventPackages = new HashSet<EventPackage>();
            EventServices = new HashSet<EventService>();
        }

        public Guid EventId { get; set; }
        public string EventType { get; set; } = null!;
        public int Participants { get; set; }
        public DateTime EventDate { get; set; }
        public Guid LocationId { get; set; }

        public virtual Location Location { get; set; } = null!;
        public virtual ICollection<EventPackage> EventPackages { get; set; }
        public virtual ICollection<EventService> EventServices { get; set; }
    }
}
