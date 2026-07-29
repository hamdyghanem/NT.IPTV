namespace NT.IPTV.Utilities
{
    /// <summary>
    /// Provides validation and helpful hints for server configuration settings.
    /// </summary>
    public static class ConfigurationValidator
    {
        // Common default IPTV ports
        private static readonly int[] CommonIPTVPorts = { 80, 8080, 8000, 8888, 3000, 5000, 6000 };

        /// <summary>
        /// Gets helpful suggestions for common port numbers.
        /// </summary>
        public static string GetPortSuggestions()
        {
            return "Common IPTV ports: " + string.Join(", ", CommonIPTVPorts);
        }

        /// <summary>
        /// Checks if the given port is a commonly used IPTV port.
        /// </summary>
        public static bool IsCommonIPTVPort(int port)
        {
            return CommonIPTVPorts.Contains(port);
        }

        /// <summary>
        /// Validates and suggests server URL format.
        /// </summary>
        public static (bool IsValid, string Suggestion) ValidateServerUrlFormat(string server)
        {
            if (string.IsNullOrWhiteSpace(server))
                return (false, "Server address cannot be empty.");

            // Check for common mistakes
            if (server.Contains(" "))
                return (false, "Remove spaces from server address.");

            if (server.StartsWith("http://") || server.StartsWith("https://"))
            {
                // Extract just the hostname
                var uri = new Uri(server);
                return (true, $"Consider using just '{uri.Host}' without protocol prefix.");
            }

            if (server.EndsWith("/"))
                return (true, "Server address has trailing slash - this is usually fine.");

            // Check if it looks like a valid format
            if (server.Contains(".") || server.Contains(":") || System.Net.IPAddress.TryParse(server, out _))
                return (true, string.Empty);

            return (false, "Server address should be a hostname (e.g., server.com) or IP address (e.g., 192.168.1.1).");
        }

        /// <summary>
        /// Validates port and provides suggestions if invalid.
        /// </summary>
        public static (bool IsValid, string Suggestion) ValidatePortWithSuggestions(string port)
        {
            if (!int.TryParse(port, out int portNum))
                return (false, "Port must be a number.");

            if (portNum < 1 || portNum > 65535)
                return (false, "Port must be between 1 and 65535.");

            if (IsCommonIPTVPort(portNum))
                return (true, $"✓ Port {portNum} is a common IPTV port.");

            return (true, $"Note: Consider checking if your IPTV service uses ports: {GetPortSuggestions()}");
        }

        /// <summary>
        /// Validates server connectivity requirements.
        /// </summary>
        public static (bool CanConnect, string Requirement) CheckConnectivityRequirements(string server, int port)
        {
            // Check if server is localhost (often used for testing)
            if (server.Equals("localhost", StringComparison.OrdinalIgnoreCase) || 
                server.Equals("127.0.0.1", StringComparison.OrdinalIgnoreCase))
            {
                return (false, "Localhost services won't work outside this machine. Use a real IP address or hostname.");
            }

            // Check if port is in privileged range (requires admin)
            if (port < 1024)
            {
                return (true, "Note: Ports below 1024 require administrator access.");
            }

            return (true, string.Empty);
        }

        /// <summary>
        /// Gets a comprehensive configuration status message.
        /// </summary>
        public static string GetConfigurationStatusMessage(string username, string server, string port, string profile)
        {
            var issues = new List<string>();

            if (string.IsNullOrEmpty(username))
                issues.Add("• Username is empty");

            if (string.IsNullOrEmpty(server))
                issues.Add("• Server address is empty");

            if (string.IsNullOrEmpty(port))
                issues.Add("• Port is not specified");

            if (string.IsNullOrEmpty(profile))
                issues.Add("• Profile name is empty");

            if (issues.Count == 0)
                return "✓ All configuration fields are filled.";

            return "Configuration issues detected:\n" + string.Join("\n", issues);
        }

        /// <summary>
        /// Suggests default port based on server URL pattern.
        /// </summary>
        public static string SuggestDefaultPort(string server)
        {
            if (string.IsNullOrEmpty(server))
                return "8080"; // Most common default

            // If it's a known service provider pattern, suggest appropriate port
            if (server.Contains("ivacy") || server.Contains("smart"))
                return "8080";

            // Default fallback
            return "80";
        }

        /// <summary>
        /// Tests if a URL pattern suggests HTTPS is needed.
        /// </summary>
        public static bool LikelyNeedsHttps(string server)
        {
            if (string.IsNullOrEmpty(server))
                return false;

            string lower = server.ToLower();
            return lower.Contains("https") || 
                   lower.Contains("ssl") || 
                   lower.Contains("secure") ||
                   lower.Contains("443"); // 443 is HTTPS port
        }
    }
}
