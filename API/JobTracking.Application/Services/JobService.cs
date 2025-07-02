using JobTracking.DataAccess; // This will contain IDataService and your concrete DataService
using JobTracking.Domain.Models;
using System.Linq; // Needed for .Where(), .Max(), .FirstOrDefault()
using System.Collections.Generic; // For IEnumerable
using System; // For DateTime.UtcNow

namespace JobTracking.Application.Services
{
    public class JobService : IJobService
    {
        private readonly IDataService _dataService;

        public JobService(IDataService dataService)
        {
            _dataService = dataService;
        }

        public JobListing CreateJob(JobListing newJob)
        {
            var db = _dataService.GetDatabase();
            // Ensure unique ID generation, robust for empty list
            newJob.Id = db.JobListings.Any() ? db.JobListings.Max(j => j.Id) + 1 : 1;
            newJob.PublicationDate = DateTime.UtcNow;

            db.JobListings.Add(newJob);
            _dataService.SaveChanges(db); // Pass the updated database object back

            return newJob;
        }

        public IEnumerable<JobListing> GetAllJobs()
        {
            // Return a new list to avoid issues if the underlying collection is modified directly elsewhere
            return _dataService.GetDatabase().JobListings.ToList();
        }

        public JobListing? GetJobById(int id)
        {
            return _dataService.GetDatabase().JobListings.FirstOrDefault(j => j.Id == id);
        }

        // --- NEW IMPLEMENTATIONS FOR IJobService ---

        public IEnumerable<JobListing> GetJobsByCompanyId(int companyId)
        {
            return _dataService.GetDatabase().JobListings
                               .Where(j => j.CompanyId == companyId)
                               .ToList(); // Convert to list to ensure enumeration happens immediately
        }

        public void DeleteJob(int id)
        {
            var db = _dataService.GetDatabase();
            var jobToDelete = db.JobListings.FirstOrDefault(j => j.Id == id);

            if (jobToDelete != null)
            {
                db.JobListings.Remove(jobToDelete); // Remove from the in-memory/file list
                _dataService.SaveChanges(db); // Save the changes back to your data source
            }
            // If you want to indicate that the job wasn't found, you could throw an exception here
            // throw new KeyNotFoundException($"Job with ID {id} not found.");
        }
    }
}