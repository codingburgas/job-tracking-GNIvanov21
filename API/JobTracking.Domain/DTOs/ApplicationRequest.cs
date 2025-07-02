using System.ComponentModel.DataAnnotations;

namespace JobTracking.Domain.DTOs
{
    public class ApplicationRequest
    {
        [Required]
        public int JobPostingId { get; set; }
    }
}