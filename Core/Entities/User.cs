using System.ComponentModel.DataAnnotations;
using SQLite;

namespace Core.Entities
{
    public class User : BaseEntity
    {
        [PrimaryKey, AutoIncrement]
        public new int Id { get; set; }
        public string? Name { get; set; }
        public string? Username { get; set; }
        public string? PasswordHash { get; set; }
        public string? Role { get; set; } // "Seller" or "Admin"
        public string? Email { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime? LastLogin { get; set; }
    }
}
