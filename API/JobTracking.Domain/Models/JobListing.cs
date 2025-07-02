using JobTracking.Domain.Enums;

namespace JobTracking.Domain.Models
{
    public class JobListing
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int CompanyId { get; set; } // Changed from CompanyName
        public string Description { get; set; }
        public DateTime PublicationDate { get; set; }
        public JobStatus Status { get; set; }
    }
}