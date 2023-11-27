using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EventsPortal.Models
{
    public class FutureDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            DateTime date = (DateTime)value;
            return date > DateTime.Now;
        }
    }
    public class EventModel
    {
        public Guid EventID { get; set; }
        public string EventType { get; set; }
        [Display(Name = "Participants")]
        [Required(ErrorMessage = "Please enter the number of participants.")]
        [Range(1, int.MaxValue, ErrorMessage = "Number of participants must be greater than 0.")]
        public int Participants { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Please enter a valid event date.")]
        [FutureDate(ErrorMessage = "Event date must be in the future.")]
        public DateTime EventDate { get; set; }

        public Guid LocationID { get; set; }

        [Display(Name = "Package")]
        public Guid SelectedPackageID { get; set; }

        [Display(Name = "Additional Services")]
        public List<Guid?> SelectedAdditionalServices { get; set; }
        // Constructor to initialize the list
        public EventModel()
        {
            SelectedAdditionalServices = new List<Guid?>();
        }
        [Display(Name = "Location")]
        public string? LocationName { get; set; }

        [Display(Name = "Package")]
        public string? SelectedPackageName { get; set; }

        [Display(Name = "Additional Services")]
        public List<string?> SelectedAdditionalServiceNames { get; set; }

    }
}
