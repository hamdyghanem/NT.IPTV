using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NT.IPTV.Utilities
{
    /// <summary>
    /// Helper class for displaying user-friendly error and status messages.
    /// </summary>
    public static class UserMessageHelper
    {
        /// <summary>
        /// Shows a validation error message with icon and title.
        /// </summary>
        public static void ShowValidationError(string message, string fieldName = "Validation")
        {
            MessageBox.Show(message, $"❌ {fieldName} Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// Shows a connection error message with specific guidance.
        /// </summary>
        public static void ShowConnectionError(Exception ex)
        {
            string message = GetConnectionErrorMessage(ex);
            MessageBox.Show($"{message}\n\nDetails: {ex.Message}", "🔌 Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Shows a success message.
        /// </summary>
        public static void ShowSuccess(string message, string title = "Success")
        {
            MessageBox.Show(message, $"✅ {title}", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Shows an info message.
        /// </summary>
        public static void ShowInfo(string message, string title = "Information")
        {
            MessageBox.Show(message, $"ℹ️ {title}", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Gets a user-friendly error message based on exception type.
        /// </summary>
        private static string GetConnectionErrorMessage(Exception ex)
        {
            if (ex == null)
                return "An unknown error occurred during connection.";

            // Check for specific exception types
            if (ex is HttpRequestException)
            {
                return "Unable to connect to the server. Please verify:\n" +
                       "• Server address is correct\n" +
                       "• Network connection is active\n" +
                       "• Firewall is not blocking the connection";
            }

            if (ex is TimeoutException)
            {
                return "Connection timed out. The server did not respond in time.\n" +
                       "Please check your internet connection and try again.";
            }

            if (ex is UnauthorizedAccessException)
            {
                return "Authentication failed. Please verify your credentials and try again.";
            }

            if (ex is InvalidOperationException)
            {
                return "Invalid operation. Please check the server settings and try again.";
            }

            if (ex.Message.Contains("DNS", StringComparison.OrdinalIgnoreCase))
            {
                return "Could not resolve the server address. Please check the server name or IP address.";
            }

            if (ex.Message.Contains("connection", StringComparison.OrdinalIgnoreCase))
            {
                return "Connection failed. Please verify the server details and network settings.";
            }

            return "Connection failed. Please check your settings and try again.";
        }

        /// <summary>
        /// Logs a message to debug output (visible in Visual Studio Output window).
        /// </summary>
        public static void LogDebug(string message)
        {
            System.Diagnostics.Debug.WriteLine($"[{DateTime.Now:HH:mm:ss}] {message}");
        }

        /// <summary>
        /// Logs an exception to debug output.
        /// </summary>
        public static void LogException(Exception ex, string context = "")
        {
            string contextInfo = string.IsNullOrEmpty(context) ? "" : $" ({context})";
            System.Diagnostics.Debug.WriteLine($"[ERROR{contextInfo}] {DateTime.Now:HH:mm:ss} - {ex.GetType().Name}: {ex.Message}");
            System.Diagnostics.Debug.WriteLine(ex.StackTrace);
        }
    }
}
