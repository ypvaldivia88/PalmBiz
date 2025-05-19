namespace Mobile
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Start database initialization in the background with exception handling
            Task.Run(async () =>
            {
                try
                {
                    await DatabaseService.InitializeAsync();
                }
                catch (Exception ex)
                {
                    // Log the exception (replace with your logging mechanism)
                    System.Diagnostics.Debug.WriteLine($"Database initialization failed: {ex}");

                    // Optionally, show a user-friendly message (on the main thread)
                    MainThread.BeginInvokeOnMainThread(async () =>
                    {
                        await Application.Current.MainPage.DisplayAlert(
                            "Error",
                            "Failed to initialize the database. The app may not function correctly.",
                            "OK");
                    });
                }
            });

            MainPage = new AppShell();
        }
    }
}
