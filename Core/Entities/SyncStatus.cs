using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class SyncStatus
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public DateTime LastSync { get; set; }
    }
}
