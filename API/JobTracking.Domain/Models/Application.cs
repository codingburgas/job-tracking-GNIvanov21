using JobTracking.Domain.Enums;

namespace JobTracking.Domain.Models
{
    public class Application
    {
        public int Id { get; set; }
        public int JobListingId { get; set; }
        public int UserId { get; set; }
        public ApplicationStatus Status { get; set; }
        public DateTime ApplicationDate { get; set; }
    }
}