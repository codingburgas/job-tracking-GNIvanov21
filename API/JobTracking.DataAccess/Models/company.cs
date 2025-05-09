using System.ComponentModel.DataAnnotations;
using JobTracking.DataAccess.Data.Base;
using JobTracking.DataAccess.Data.Models;

namespace JobTracking.DataAccess.Models
;

public class company : IEntity
{
    [Key]    
    public int Id { get; set; }    
    public bool IsActive { get; set; }    
    public DateTime CreatedOn { get; set; }    
    public string CreatedBy { get; set; }    
    public DateTime? UpdatedOn { get; set; }    
    public string? UpdatedBy { get; set; }        
    public virtual ICollection<worker> workers { get; set; } = new List<worker>();
}