using JobTracking.Domain.Enums;
using System.Text.Json.Serialization;

namespace JobTracking.Domain.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        
        public string PasswordHash { get; set; }
        public Role Role { get; set; }
    }
}