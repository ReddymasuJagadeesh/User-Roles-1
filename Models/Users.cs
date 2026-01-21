using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace UserRoles.Models
{
    public class Users : IdentityUser
    {
        public string? Name { get; set; }

        // Parent in hierarchy (Admin or Manager)
        public string? ParentUserId { get; set; }

        // Explicit ManagerId column to match migrations / DB schema and FK
        public string? ManagerId { get; set; }

        [ForeignKey("ManagerId")]
        public Users? Manager { get; set; }

        public int PasswordResetCount { get; set; } = 0;
        public DateTime? PasswordResetDate { get; set; }

        [Phone]
        public string? MobileNumber { get; set; }

        public ICollection<Users> TeamMembers { get; set; }
    }
}
