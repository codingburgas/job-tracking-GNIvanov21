using JobTracking.Domain.Models;

namespace JobTracking.Application.Services
{
    public interface IApplicationService
    {
        // Use the full name of the class here
        JobTracking.Domain.Models.Application? CreateApplication(JobTracking.Domain.Models.Application newApplication);
        
        IEnumerable<JobTracking.Domain.Models.Application> GetApplicationsByUserId(int userId);
        
        IEnumerable<JobTracking.Domain.Models.Application> GetAllApplications();
    }
}