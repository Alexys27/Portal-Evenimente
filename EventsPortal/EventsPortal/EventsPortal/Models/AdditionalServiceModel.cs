using System.ComponentModel.DataAnnotations;

namespace EventsPortal.Models
{
    public class AdditionalServiceModel
    {
        public Guid ServiceID { get; set; }
        [Required]
        public string ServiceName { get; set; }
        [Required]
        public decimal AdditionalPrice { get; set; }
    }
}
