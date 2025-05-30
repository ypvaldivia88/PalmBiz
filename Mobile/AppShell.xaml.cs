using Core.Interfaces;
using Microsoft.Maui.Controls;

namespace Mobile
{
    public partial class AppShell : Shell
    {
        private readonly IAuthService _authService;

        public AppShell(IAuthService authService)
        {
            InitializeComponent();
            _authService = authService;
            
            // Register routes
            Routing.RegisterRoute(nameof(Views.ProductListView), typeof(Views.ProductListView));
            
            // Detect authentication state changes
            this.Navigated += OnNavigated;
        }

        private async void OnNavigated(object sender, ShellNavigatedEventArgs e)
        {
            // Do not handle the initial navigation to the login page
            if (e.Current.Location.OriginalString.Contains("login"))
                return;

            if (!_authService.IsUserAuthenticated())
            {
                // User is not authenticated, redirect to login
                await Current.GoToAsync("//login");
                return;
            }

            // Check role-based access control
            bool isAdmin = _authService.IsInRole("Admin");
            
            // Show/hide admin tabs based on role
            AdminTab.IsVisible = isAdmin;
        }

        protected override async void OnNavigating(ShellNavigatingEventArgs args)
        {
            base.OnNavigating(args);

            // Skip checks for the login page to avoid infinite loops
            if (args.Target.Location.OriginalString.Contains("login"))
                return;

            // If not authenticated, cancel navigation and redirect to login
            if (!_authService.IsUserAuthenticated())
            {
                args.Cancel();
                await Current.GoToAsync("//login");
            }
        }

        // Add logout functionality
        public async void Logout()
        {
            await _authService.LogoutAsync();
            await Current.GoToAsync("//login");
        }
    }
}
