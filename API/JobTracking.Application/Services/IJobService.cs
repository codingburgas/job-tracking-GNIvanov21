using System.Collections.Generic; // Make sure this is included for IEnumerable
using JobTracking.Domain.Models;

namespace JobTracking.Application.Services
{
    public interface IJobService
    {
        JobListing CreateJob(JobListing newJob);
        IEnumerable<JobListing> GetAllJobs();
        JobListing? GetJobById(int id);

        // --- ADD THESE NEW METHOD DECLARATIONS ---
        IEnumerable<JobListing> GetJobsByCompanyId(int companyId);
        void DeleteJob(int id);
    }
}