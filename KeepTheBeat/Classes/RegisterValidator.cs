using KeepTheBeat.Interfaces;

namespace KeepTheBeat.Classes
{
    public class RegisterValidator
    {
        private readonly IUserService _userService;

        public RegisterValidator(IUserService userService)
        {
            _userService = userService;
        }

        public string FirstnameError { get; private set; }
        public string LastnameError { get; private set; }
        public string EmailError { get; private set; }
        public string PasswordError { get; private set; }
        public string UsernameError { get; private set; }

        public async Task<bool> ValidateFieldsAsync(string firstname, string lastname, string username, string email, string password)
        {
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(firstname))
            {
                FirstnameError = "Firstname is required.";
                isValid = false;
            }
            else
            {
                FirstnameError = null;
            }

            if (string.IsNullOrWhiteSpace(lastname))
            {
                LastnameError = "Lastname is required.";
                isValid = false;
            }
            else
            {
                LastnameError = null;
            }

            if (string.IsNullOrWhiteSpace(username))
            {
                UsernameError = "Username is required.";
                isValid = false;
            }
            else if (await _userService.IsUsernameTaken(username))
            {
                UsernameError = "Username is already taken.";
                isValid = false;
            }
            else
            {
                UsernameError = null;
            }

            if (string.IsNullOrWhiteSpace(email) || !IsValidEmail(email))
            {
                isValid = false;
            }
            else if (await _userService.IsEmailTaken(email))
            {
                EmailError = "Email is already taken.";
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(password) || !IsValidPassword(password))
            {
                isValid = false;
            }

            return isValid;
        }

        public bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                EmailError = null;
                return addr.Address == email;
            }
            catch
            {
                EmailError = "Invalid email format";
                return false;
            }
        }

        public bool IsValidPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
            {
                PasswordError = "Password must be at least 8 characters long";
                return false;
            }
            PasswordError = null;
            return true;
        }
    }
}
