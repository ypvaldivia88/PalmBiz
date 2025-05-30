using System.ComponentModel.DataAnnotations;
using SQLite;

namespace Core.Entities
{
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Role { get; set; } // e.g., "Seller", "Admin"
    }

}
