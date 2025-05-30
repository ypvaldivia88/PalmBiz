using Core.Interfaces;

namespace Mobile
{
    public partial class App : Application
    {
        private readonly IAuthService _authService;

        public App(IAuthService authService)
        {
            InitializeComponent();
            _authService = authService;

            // Create the shell with DI
            MainPage = new AppShell(_authService);
            
            // Navigate to the appropriate starting page
            Startup();
        }

        private async void Startup()
        {
            // Check if the user is authenticated
            await Task.Delay(100); // Small delay to let UI initialize
            
            if (!_authService.IsUserAuthenticated())
            {
                // Navigate to login page if not authenticated
                await Shell.Current.GoToAsync("//login");
            }
            else
            {
                // Navigate to main page if authenticated
                await Shell.Current.GoToAsync("//main");
            }
        }
    }
}
