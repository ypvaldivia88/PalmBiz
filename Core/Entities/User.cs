﻿using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Role { get; set; } // e.g., "Seller", "Admin"
    }

}
