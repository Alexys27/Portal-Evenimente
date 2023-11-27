namespace EventsPortal.Models
{
    public class LocationModel
    {
        public Guid LocationID { get; set; }
        public string LocationName { get; set; }
        public string LocationAdress { get; set; }
        public string? ImagePath { get; set; }
        public IFormFile? ImageFile { get; set; }
    }
}