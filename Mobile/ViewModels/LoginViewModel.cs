using Core.Entities;
using Core.Interfaces;
using Microsoft.Maui.Controls;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Mobile.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly IAuthService _authService;

        private string _username;
        private string _password;
        private bool _isRegistering;
        private string _confirmPassword;
        private string _name;
        private string _email;
        private string _errorMessage;

        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public bool IsRegistering
        {
            get => _isRegistering;
            set => SetProperty(ref _isRegistering, value);
        }

        public string ConfirmPassword
        {
            get => _confirmPassword;
            set => SetProperty(ref _confirmPassword, value);
        }

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public ICommand LoginCommand { get; }
        public ICommand RegisterCommand { get; }
        public ICommand ToggleModeCommand { get; }

        public LoginViewModel(IAuthService authService)
        {
            Title = "Login";
            _authService = authService;

            LoginCommand = new Command(async () => await OnLoginAsync());
            RegisterCommand = new Command(async () => await OnRegisterAsync());
            ToggleModeCommand = new Command(() => IsRegistering = !IsRegistering);
        }

        private async Task OnLoginAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            ErrorMessage = string.Empty;

            try
            {
                if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
                {
                    ErrorMessage = "Por favor ingrese usuario y contraseña";
                    return;
                }

                var user = await _authService.AuthenticateAsync(Username, Password);
                if (user == null)
                {
                    ErrorMessage = "Credenciales inválidas";
                    return;
                }

                // Redirect to appropriate shell based on role
                await Shell.Current.GoToAsync("//main");
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error de autenticación: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task OnRegisterAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            ErrorMessage = string.Empty;

            try
            {
                if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(Name))
                {
                    ErrorMessage = "Por favor complete todos los campos obligatorios";
                    return;
                }

                if (Password != ConfirmPassword)
                {
                    ErrorMessage = "Las contraseñas no coinciden";
                    return;
                }

                var newUser = new User
                {
                    Username = Username,
                    Name = Name,
                    Email = Email,
                    Role = "Seller", // Default role
                    IsActive = true
                };

                bool result = await _authService.RegisterAsync(newUser, Password);
                if (result)
                {
                    await _authService.AuthenticateAsync(Username, Password);
                    await Shell.Current.GoToAsync("//main");
                }
                else
                {
                    ErrorMessage = "No se pudo registrar el usuario";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error de registro: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}