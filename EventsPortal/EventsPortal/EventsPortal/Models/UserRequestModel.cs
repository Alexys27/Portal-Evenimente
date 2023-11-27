using System.ComponentModel.DataAnnotations;
namespace EventsPortal.Models
{
    public class UserRequestModel
    {
        public Guid RequestID { get; set; }
        public string RequestType { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime RequestDateTime { get; set; }
        public string AdditionalInfo { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }

    }
}
