using SQLite;

namespace Core.Entities
{
    public class Sale
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public List<SaleDetail>? Details { get; set; }
        public decimal Total { get; set; }
        public string? PaymentMethod { get; set; }
        public string? Currency { get; set; }
    }

}
