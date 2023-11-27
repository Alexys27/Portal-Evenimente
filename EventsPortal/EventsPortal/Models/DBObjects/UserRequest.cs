using System;
using System.Collections.Generic;

namespace EventsPortal.Models.DBObjects
{
    public partial class UserRequest
    {
        public Guid RequestId { get; set; }
        public string RequestType { get; set; } = null!;
        public DateTime RequestDateTime { get; set; }
        public string? AdditionalInfo { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
    }
}
