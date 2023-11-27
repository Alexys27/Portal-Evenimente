using System;
using System.Collections.Generic;

namespace EventsPortal.Models.DBObjects
{
    public partial class AdditionalService
    {
        public AdditionalService()
        {
            EventServices = new HashSet<EventService>();
        }

        public Guid ServiceId { get; set; }
        public string ServiceName { get; set; } = null!;
        public decimal AdditionalPrice { get; set; }

        public virtual ICollection<EventService> EventServices { get; set; }
    }
}
