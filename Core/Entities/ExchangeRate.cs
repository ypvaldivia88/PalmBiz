using SQLite;

namespace Core.Entities
{
    public class ExchangeRate
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Rate { get; set; }
    }

}
