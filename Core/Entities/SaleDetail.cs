using SQLite;

namespace Core.Entities
{
    public class SaleDetail : BaseEntity
    {
        [PrimaryKey, AutoIncrement]
        public new int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }

}
