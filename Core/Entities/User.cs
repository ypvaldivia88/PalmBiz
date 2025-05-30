using System.ComponentModel.DataAnnotations;
using SQLite;

namespace Core.Entities
{
    public class User : BaseEntity
    {
        [PrimaryKey, AutoIncrement]
        public new int Id { get; set; }
        public string? Name { get; set; }
        public string? Role { get; set; } // e.g., "Seller", "Admin"
    }

}
