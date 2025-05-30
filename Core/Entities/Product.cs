using SQLite;

namespace Core.Entities
{
    public class Product
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Category { get; set; }
        public string? ImageUrl { get; set; }
        public decimal BasePrice { get; set; }
        public int? Stock { get; set; }
        public decimal? Cost { get; set; }
    }
}
