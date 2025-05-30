using Core.Entities;
using Core.Interfaces;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private const string PREF_KEY_CURRENT_USER_ID = "currentUserId";
        private readonly IUserService _userService;
        private User _currentUser;

        public AuthService(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<User> AuthenticateAsync(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;

            var users = await _userService.GetAllAsync();
            var user = users.FirstOrDefault(u => u.Username == username && u.IsActive);

            if (user == null)
                return null;

            if (!await VerifyPasswordAsync(password, user.PasswordHash))
                return null;

            // Update last login
            user.LastLogin = DateTime.UtcNow;
            await _userService.UpdateAsync(user);

            // Save current user ID in preferences
            await SecureStorage.SetAsync(PREF_KEY_CURRENT_USER_ID, user.Id.ToString());
            
            _currentUser = user;
            return user;
        }

        public async Task<bool> RegisterAsync(User user, string password)
        {
            if (string.IsNullOrEmpty(user.Username) || string.IsNullOrEmpty(password))
                return false;

            // Hash password
            user.PasswordHash = await HashPasswordAsync(password);
            
            // Set default role if not specified
            if (string.IsNullOrEmpty(user.Role))
                user.Role = "Seller";

            await _userService.AddAsync(user);
            return true;
        }

        public async Task<User> GetCurrentUserAsync()
        {
            if (_currentUser != null)
                return _currentUser;

            string userId = await SecureStorage.GetAsync(PREF_KEY_CURRENT_USER_ID);
            
            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int id))
                return null;

            _currentUser = await _userService.GetByIdAsync(id);
            return _currentUser;
        }

        public bool IsUserAuthenticated()
        {
            return _currentUser != null || SecureStorage.GetAsync(PREF_KEY_CURRENT_USER_ID).Result != null;
        }

        public async Task LogoutAsync()
        {
            _currentUser = null;
            SecureStorage.Remove(PREF_KEY_CURRENT_USER_ID);
            await Task.CompletedTask;
        }

        public bool IsInRole(string role)
        {
            if (_currentUser == null)
                _currentUser = GetCurrentUserAsync().Result;

            return _currentUser?.Role == role;
        }

        public async Task<string> HashPasswordAsync(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        public async Task<bool> VerifyPasswordAsync(string password, string passwordHash)
        {
            string hashedPassword = await HashPasswordAsync(password);
            return hashedPassword == passwordHash;
        }
    }
}