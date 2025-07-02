using JobTracking.Domain.Models;

namespace JobTracking.DataAccess
{
    public class DatabaseModel
    {
        public List<User> Users { get; set; } = new();
        public List<Company> Companies { get; set; } = new();
        public List<JobListing> JobListings { get; set; } = new();
        public List<Application> Applications { get; set; } = new();
    }
}