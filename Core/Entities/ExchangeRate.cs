using SQLite;

namespace Core.Entities
{
    public class ExchangeRate : BaseEntity
    {
        [PrimaryKey, AutoIncrement]
        public new int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Rate { get; set; }
    }

}
