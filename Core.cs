using System;
using UnityEngine;

namespace SignalDelay
{
    /// <summary>
    /// Log levels:
    /// <list type="bullet">
    /// <item><definition>None: do not log</definition></item>
    /// <item><definition>Error: log only errors</definition></item>
    /// <item><definition>Important: log only errors and important information</definition></item>
    /// <item><definition>Debug: log all information</definition></item>
    /// </list>
    /// </summary>
    internal enum LogLevel { None = 0, Error, Important, Debug };

    static class Core
    {
        /// <summary>
        /// Returns effective light speed for signal delay calculation (half the speed if round trip is enabled)
        /// </summary>
        public static double LightSpeed => SignalDelaySettings.Instance.LightSpeed / (SignalDelaySettings.Instance.Roundtrip ? 2 : 1);

        /// <summary>
        /// Current <see cref="LogLevel"/>: either Debug or Important
        /// </summary>
        public static LogLevel Level => SignalDelaySettings.Instance.DebugMode ? LogLevel.Debug : LogLevel.Important;

        /// <summary>
        /// Formats time as a string, e.g. 876 d 5 h 43 m 21.09 s
        /// </summary>
        /// <param name="time">Time in seconds</param>
        /// <param name="digits">Number of floating-point digits in seconds, default = 2</param>
        /// <returns></returns>
        public static string FormatTime(double time, int digits = 2)
        {
            double t = time;
            int d, m, h;
            string res = "";
            bool show0 = false;
            if (t >= KSPUtil.dateTimeFormatter.Day)
            {
                d = (int)Math.Floor(t / KSPUtil.dateTimeFormatter.Day);
                t -= d * KSPUtil.dateTimeFormatter.Day;
                res += d + " d ";
                show0 = true;
            }
            if (show0 || t >= 3600)
            {
                h = (int)Math.Floor(t / 3600);
                t -= h * 3600;
                res += h + " h ";
                show0 = true;
            }
            if (show0 || t >= 60)
            {
                m = (int)Math.Floor(t / 60);
                t -= m * 60;
                res += m + " m ";
            }
            if (time < 1 || Math.Round(t, digits) > 0)
                res += t.ToString("F" + digits) + " s";
            return res;
        }

        /// <summary>
        /// Returns a Double from the ConfigNode or default value
        /// </summary>
        /// <param name="node">ConfigNode to search the value in</param>
        /// <param name="key">Key of the value</param>
        /// <param name="defaultValue">Value to return if the key is not found/in invalid format</param>
        /// <returns></returns>
        public static double GetDouble(this ConfigNode node, string key, double defaultValue = 0)
        {
            double res = 0;
            return node.TryGetValue(key, ref res) ? res : defaultValue;
        }

        /// <summary>
        /// Prints non-zero values in a FlightCtrlState to a string, with a title
        /// </summary>
        /// <param name="flightCtrlState">FlightCtrlState</param>
        /// <param name="title">Title to apply to the string</param>
        /// <returns>Human-readable string with values or empty if flightCtrlState is all zeroes</returns>
        public static string FCSToString(FlightCtrlState flightCtrlState, string title = "")
        {
            string res = "";
            if (flightCtrlState.pitch != 0)
                res += "Pitch: " + flightCtrlState.pitch + "   ";
            if (flightCtrlState.pitchTrim != 0)
                res += "Pitch Trim: " + flightCtrlState.pitchTrim + "   ";
            if (flightCtrlState.yaw != 0)
                res += "Yaw: " + flightCtrlState.yaw + "   ";
            if (flightCtrlState.yawTrim != 0)
                res += "Yaw Trim: " + flightCtrlState.yawTrim + "   ";
            if (flightCtrlState.roll != 0)
                res += "Roll: " + flightCtrlState.roll + "   ";
            if (flightCtrlState.rollTrim != 0)
                res += "Roll Trim: " + flightCtrlState.rollTrim + "   ";
            if (flightCtrlState.mainThrottle != 0)
                res += "Throttle: " + flightCtrlState.mainThrottle;
            if (flightCtrlState.wheelSteer != 0)
                res += "Wheel Steer: " + flightCtrlState.wheelSteer + "   ";
            if (flightCtrlState.wheelThrottle != 0)
                res += "Wheel Throttle: " + flightCtrlState.wheelThrottle + "   ";
            return (((title != "") && (res != "")) ? title + ": " : "") + res;
        }

        /// <summary>
        /// Posts a screen message
        /// </summary>
        /// <param name="msg"></param>
        public static void ShowNotification(string msg) => ScreenMessages.PostScreenMessage(msg);

        /// <summary>
        /// Returns true if message with the given level should be logged under current settings
        /// </summary>
        /// <param name="messageLevel"></param>
        /// <returns></returns>
        public static bool IsLogging(LogLevel messageLevel = LogLevel.Debug) => messageLevel <= Level;

        /// <summary>
        /// Write the message into log file if <see cref="LogLevel"/> >= messageLevel
        /// </summary>
        /// <param name="message">Text to log</param>
        /// <param name="messageLevel"><see cref="LogLevel"/> of the entry</param>
        public static void Log(string message, LogLevel messageLevel = LogLevel.Debug)
        {
            if (IsLogging(messageLevel) && message.Length != 0)
                Debug.Log("[SignalDelay] " + (messageLevel == LogLevel.Error ? "ERROR: " : "") + message);
        }
    }
}
