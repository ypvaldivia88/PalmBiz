using SQLite;

namespace Core.Entities
{
    public class Sale : BaseEntity
    {
        [PrimaryKey, AutoIncrement]
        public new int Id { get; set; }
        public DateTime Date { get; set; }
        public List<SaleDetail>? Details { get; set; }
        public decimal Total { get; set; }
        public string? PaymentMethod { get; set; }
        public string? Currency { get; set; }
    }

}
