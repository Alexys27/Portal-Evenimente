namespace EventsPortal.Models
{
    public class PackageModel
    {
        public Guid PackageID { get; set; }
        public string PackageName { get; set; }
        public string PackageDescription { get; set; }
        public decimal PricePerParticipant { get; set; }
    }
}
