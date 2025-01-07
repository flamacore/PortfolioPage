using UnityEngine;

namespace PortfolioViewer.Utilities
{
    /// <summary>
    /// Provides logging utilities for the PortfolioViewer application.
    /// </summary>
    public static class DebugLogger
    {
        /// <summary>
        /// Prefix for all log messages.
        /// </summary>
        private const string Prefix = "Chao's Portfolio: ";

        /// <summary>
        /// Color used for informational log messages.
        /// </summary>
        private static readonly Color ColorInfo = new Color(0.49f, 0.82f, 1f);

        /// <summary>
        /// Color used for warning log messages.
        /// </summary>
        private static readonly Color ColorWarning = Color.yellow;

        /// <summary>
        /// Color used for error log messages.
        /// </summary>
        private static readonly Color ColorError = Color.red;

        /// <summary>
        /// Logs an informational message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public static void Log(string message)
        {
            WrapMessage(message, ColorInfo);
        }

        /// <summary>
        /// Logs a warning message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public static void LogWarning(string message)
        {
            WrapMessage(message, ColorWarning);
        }

        /// <summary>
        /// Logs an error message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public static void LogError(string message)
        {
            WrapMessage(message, ColorError);
        }

        /// <summary>
        /// Wraps the message with the specified color and logs it to the Unity console.
        /// </summary>
        /// <param name="message">The message to wrap and log.</param>
        /// <param name="color">The color to use for the message.</param>
        private static void WrapMessage(string message, Color color)
        {
            string wrappedMessage = $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{Prefix}{message}</color>";
            Debug.Log(wrappedMessage);
        }
    }
}