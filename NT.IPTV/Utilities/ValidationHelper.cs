using System.Text.RegularExpressions;

namespace NT.IPTV.Utilities
{
    /// <summary>
    /// Provides centralized validation methods for login form input fields.
    /// </summary>
    public static class ValidationHelper
    {
        /// <summary>
        /// Validates that a username is not empty and meets minimum requirements.
        /// </summary>
        public static (bool IsValid, string ErrorMessage) ValidateUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return (false, "Username cannot be empty.");

            if (username.Length < 3)
                return (false, "Username must be at least 3 characters long.");

            if (username.Length > 50)
                return (false, "Username cannot exceed 50 characters.");

            return (true, string.Empty);
        }

        /// <summary>
        /// Validates that a password is not empty and meets minimum requirements.
        /// </summary>
        public static (bool IsValid, string ErrorMessage) ValidatePassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return (false, "Password cannot be empty.");

            if (password.Length < 4)
                return (false, "Password must be at least 4 characters long.");

            return (true, string.Empty);
        }

        /// <summary>
        /// Validates that a server address is not empty and is a valid hostname or IP address.
        /// </summary>
        public static (bool IsValid, string ErrorMessage) ValidateServer(string server)
        {
            if (string.IsNullOrWhiteSpace(server))
                return (false, "Server address cannot be empty.");

            server = server.Trim();

            // Remove protocol if present
            string cleanServer = server.Replace("https://", "").Replace("http://", "").Split('/')[0];

            // Check if it's a valid hostname or IP address
            if (!IsValidHostnameOrIP(cleanServer))
                return (false, "Server must be a valid hostname or IP address (e.g., example.com or 192.168.1.1).");

            return (true, string.Empty);
        }

        /// <summary>
        /// Validates that a port number is valid (1-65535).
        /// </summary>
        public static (bool IsValid, string ErrorMessage) ValidatePort(string port)
        {
            if (string.IsNullOrWhiteSpace(port))
                return (false, "Port cannot be empty.");

            if (!int.TryParse(port, out int portNumber))
                return (false, "Port must be a valid number.");

            if (portNumber < 1 || portNumber > 65535)
                return (false, "Port must be between 1 and 65535.");

            return (true, string.Empty);
        }

        /// <summary>
        /// Validates that a profile name is not empty.
        /// </summary>
        public static (bool IsValid, string ErrorMessage) ValidateProfileName(string profileName)
        {
            if (string.IsNullOrWhiteSpace(profileName))
                return (false, "Profile name cannot be empty.");

            if (profileName.Length > 50)
                return (false, "Profile name cannot exceed 50 characters.");

            return (true, string.Empty);
        }

        /// <summary>
        /// Validates all required fields for login.
        /// </summary>
        public static (bool IsValid, string ErrorMessage) ValidateAllLoginFields(string username, string password, string server, string port, string profileName)
        {
            // Validate username
            var usernameValidation = ValidateUsername(username);
            if (!usernameValidation.IsValid)
                return (false, usernameValidation.ErrorMessage);

            // Validate password
            var passwordValidation = ValidatePassword(password);
            if (!passwordValidation.IsValid)
                return (false, passwordValidation.ErrorMessage);

            // Validate server
            var serverValidation = ValidateServer(server);
            if (!serverValidation.IsValid)
                return (false, serverValidation.ErrorMessage);

            // Validate port
            var portValidation = ValidatePort(port);
            if (!portValidation.IsValid)
                return (false, portValidation.ErrorMessage);

            // Validate profile name
            var profileValidation = ValidateProfileName(profileName);
            if (!profileValidation.IsValid)
                return (false, profileValidation.ErrorMessage);

            return (true, string.Empty);
        }

        /// <summary>
        /// Calculates password strength (0-100 scale).
        /// </summary>
        public static int CalculatePasswordStrength(string password)
        {
            if (string.IsNullOrEmpty(password))
                return 0;

            int strength = 0;
            int length = password.Length;

            // Length scoring
            if (length >= 8) strength += 20;
            else if (length >= 6) strength += 10;

            // Character diversity scoring
            if (Regex.IsMatch(password, @"[a-z]")) strength += 15;  // Lowercase
            if (Regex.IsMatch(password, @"[A-Z]")) strength += 15;  // Uppercase
            if (Regex.IsMatch(password, @"[0-9]")) strength += 20;  // Numbers
            if (Regex.IsMatch(password, @"[!@#$%^&*()_\-+=\[\]{};:'"",.<>?/\\|`~]")) strength += 30; // Special chars

            return Math.Min(strength, 100);
        }

        /// <summary>
        /// Gets a password strength description based on calculated strength score.
        /// </summary>
        public static string GetPasswordStrengthDescription(int strengthScore)
        {
            return strengthScore switch
            {
                < 30 => "Weak",
                < 60 => "Fair",
                < 80 => "Good",
                _ => "Strong"
            };
        }

        /// <summary>
        /// Checks if a string is a valid hostname or IP address.
        /// </summary>
        private static bool IsValidHostnameOrIP(string host)
        {
            if (string.IsNullOrWhiteSpace(host))
                return false;

            // Check if it's a valid IP address
            if (System.Net.IPAddress.TryParse(host, out _))
                return true;

            // Check if it's a valid hostname
            // Hostname pattern: alphanumeric, dots, hyphens (but not starting/ending with hyphen)
            string hostnamePattern = @"^(?!-)([a-zA-Z0-9-]{1,63}(?<!-)\.)*[a-zA-Z0-9]{1,63}$";
            return Regex.IsMatch(host, hostnamePattern);
        }
    }
}
