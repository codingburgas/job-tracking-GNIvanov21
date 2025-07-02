using System.ComponentModel.DataAnnotations;
using JobTracking.Domain.Enums; // Уверете се, че използвате правилното пространство от имена

namespace JobTracking.Domain.DTOs
{
    public class UpdateApplicationStatusRequest
    {
        [Required]
        public ApplicationStatus Status { get; set; }
    }
}