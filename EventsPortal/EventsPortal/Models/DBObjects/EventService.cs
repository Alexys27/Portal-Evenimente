using System;
using System.Collections.Generic;

namespace EventsPortal.Models.DBObjects
{
    public partial class EventService
    {
        public Guid EventServiceId { get; set; }
        public Guid EventId { get; set; }
        public Guid ServiceId { get; set; }

        public virtual Events Event { get; set; } = null!;
        public virtual AdditionalService Service { get; set; } = null!;
    }
}
