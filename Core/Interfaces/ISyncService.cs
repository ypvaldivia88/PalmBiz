using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface ISyncService
    {
        Task InitializeAsync();
        Task<bool> SyncAllAsync();
        Task<bool> SyncEntityAsync<T>(T entity) where T : BaseEntity;
        Task<DateTime> GetLastSyncTimeAsync();
        Task SetLastSyncTimeAsync(DateTime time);
        Task<bool> IsConnectedAsync();
    }
}
