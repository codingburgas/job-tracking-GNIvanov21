using JobTracking.DataAccess;
using JobTracking.Domain.Models;

namespace JobTracking.Application.Services
{
    public class ApplicationService : IApplicationService
    {
        private readonly IDataService _dataService;

        public ApplicationService(IDataService dataService)
        {
            _dataService = dataService;
        }

        public JobTracking.Domain.Models.Application? CreateApplication(JobTracking.Domain.Models.Application newApplication)
        {
            var db = _dataService.GetDatabase();

            var exists = db.Applications.Any(a => a.UserId == newApplication.UserId && a.JobListingId == newApplication.JobListingId);
            if (exists)
            {
                return null;
            }

            newApplication.Id = db.Applications.Any() ? db.Applications.Max(a => a.Id) + 1 : 1;
            newApplication.ApplicationDate = DateTime.UtcNow;
            newApplication.Status = Domain.Enums.ApplicationStatus.Submitted;

            db.Applications.Add(newApplication);
            _dataService.SaveChanges(db);

            return newApplication;
        }

        public IEnumerable<JobTracking.Domain.Models.Application> GetApplicationsByUserId(int userId)
        {
            return _dataService.GetDatabase().Applications.Where(a => a.UserId == userId);
        }

        public IEnumerable<JobTracking.Domain.Models.Application> GetAllApplications()
        {
            return _dataService.GetDatabase().Applications;
        }
    }
}