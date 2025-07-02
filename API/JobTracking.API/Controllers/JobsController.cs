using JobTracking.Application.Services;
using JobTracking.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System; // Added for Console.WriteLine for debugging. Remove in production.
using System.Threading.Tasks; // Added for async operations

// You will likely need these namespaces for Authorization
// using Microsoft.AspNetCore.Authorization;
// using System.Security.Claims;

namespace JobTracking.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JobsController : ControllerBase
    {
        private readonly IJobService _jobService;
        // Uncomment and inject IApplicationService if you create one
        // private readonly IApplicationService _applicationService;

        public JobsController(IJobService jobService /*, IApplicationService applicationService */)
        {
            _jobService = jobService;
            // _applicationService = applicationService;
        }

        [HttpGet]
        public IActionResult GetAllJobs()
        {
            return Ok(_jobService.GetAllJobs());
        }

        [HttpGet("{id}")]
        public IActionResult GetJobById(int id)
        {
            var job = _jobService.GetJobById(id);
            if (job == null)
            {
                return NotFound();
            }
            return Ok(job);
        }

        [HttpPost]
        // [Authorize(Roles = "Admin")] // Uncomment and configure authorization as needed
        public IActionResult CreateJob(JobTracking.Domain.Models.JobListing newJob)
        {
            var createdJob = _jobService.CreateJob(newJob);
            return CreatedAtAction(nameof(GetJobById), new { id = createdJob.Id }, createdJob);
        }

        [HttpPost("{jobId}/apply")]
        // [Authorize] // Uncomment to require authentication for applying
        public async Task<IActionResult> ApplyForJob(int jobId, [FromBody] ApplicationRequestDto request)
        {
            var job = _jobService.GetJobById(jobId);
            if (job == null)
            {
                return NotFound(new { message = "Job not found." });
            }


            int userId = request.UserId;


           
            Console.WriteLine($"User {userId} applied for Job ID {jobId}"); // For debugging purposes

            return Ok(new { message = "Application submitted successfully!", jobId = jobId, userId = userId });
        }

        [HttpGet("company/{companyId}")]
        // [Authorize(Roles = "Admin")] // Uncomment and configure authorization as needed
        public IActionResult GetJobsByCompany(int companyId)
        {
            // You need to add a GetJobsByCompanyId method to your IJobService
            var jobs = _jobService.GetJobsByCompanyId(companyId);
            if (jobs == null || !jobs.Any()) // Check if jobs is null or empty
            {
                return NotFound(new { message = "No jobs found for this company." });
            }
            return Ok(jobs);
        }

        [HttpDelete("{id}")]
        // [Authorize(Roles = "Admin")] // Uncomment and configure authorization as needed
        public IActionResult DeleteJob(int id)
        {
            var jobToDelete = _jobService.GetJobById(id);
            if (jobToDelete == null)
            {
                return NotFound(new { message = "Job not found." });
            }

            // You need to add a DeleteJob method to your IJobService
            _jobService.DeleteJob(id);

            return NoContent(); // 204 No Content for successful deletion
        }
    }

    public class ApplicationRequestDto
    {
        public int UserId { get; set; }
    }
}