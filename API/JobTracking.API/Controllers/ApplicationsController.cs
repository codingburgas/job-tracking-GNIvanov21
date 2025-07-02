using JobTracking.Application.Services;
using JobTracking.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace JobTracking.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApplicationsController : ControllerBase
    {
        private readonly IApplicationService _applicationService;

        public ApplicationsController(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        [HttpGet]
        public IActionResult GetAllApplications()
        {
            return Ok(_applicationService.GetAllApplications());
        }

        [HttpGet("user/{userId}")]
        public IActionResult GetApplicationsByUser(int userId)
        {
            return Ok(_applicationService.GetApplicationsByUserId(userId));
        }

        [HttpPost]
        public IActionResult CreateApplication(JobTracking.Domain.Models.Application newApplication)
        {
            var createdApplication = _applicationService.CreateApplication(newApplication);
            if (createdApplication == null)
            {
                return BadRequest("You have already applied for this job.");
            }
            return Ok(createdApplication);
        }
    }
}