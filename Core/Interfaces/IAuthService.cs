using Core.Entities;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IAuthService
    {
        Task<User> AuthenticateAsync(string username, string password);
        Task<bool> RegisterAsync(User user, string password);
        Task<User> GetCurrentUserAsync();
        bool IsUserAuthenticated();
        Task LogoutAsync();
        bool IsInRole(string role);
        Task<string> HashPasswordAsync(string password);
        Task<bool> VerifyPasswordAsync(string password, string passwordHash);
    }
}