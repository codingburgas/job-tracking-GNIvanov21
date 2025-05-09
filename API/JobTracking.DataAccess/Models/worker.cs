using System.ComponentModel.DataAnnotations;
using JobTracking.DataAccess.Data.Base;
using JobTracking.DataAccess.Models;

namespace JobTracking.DataAccess.Data.Models;
public class worker : IEntity
{    
    [Key]   
    public int Id { get; set; }
    public bool IsActive { get; set; }    
    public DateTime CreatedOn { get; set; }
    [Required]
    public string CreatedBy { get; set; }
    public DateTime? UpdatedOn { get; set; }    
    public string? UpdatedBy { get; set; }    
    [Required]    
    public int CompanyId { get; set; }    
    public virtual company Company { get; set; }
    
}