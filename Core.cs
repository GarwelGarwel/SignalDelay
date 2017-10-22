using System;
using UnityEngine;

namespace SignalDelay
{
    class Core
    {
        public static double LightSpeed
        { get { return SignalDelaySettings.LightSpeed / (SignalDelaySettings.RoundTrip ? 2 : 1); } }

        public static string FormatTime(double time, int digits = 2)
        {
            double t = time;
            int d, m, h;
            string res = "";
            bool show0 = false;
            if (t >= KSPUtil.dateTimeFormatter.Day)
            {
                d = (int)Math.Floor(time / KSPUtil.dateTimeFormatter.Day);
                t -= d * KSPUtil.dateTimeFormatter.Day;
                res += d + " d ";
                show0 = true;
            }
            if ((t >= 3600) || show0)
            {
                h = (int)Math.Floor(time / 3600);
                t -= h * 3600;
                res += h + " h ";
                show0 = true;
            }
            if ((t >= 60) || show0)
            {
                m = (int)Math.Floor(time / 60);
                t -= m * 60;
                res += m + " m ";
            }
            res += t.ToString("F" + digits) + " s";
            return res;
        }

        public static double GetDouble(ConfigNode n, string key, double defaultValue = 0)
        {
            double res;
            try { res = Double.Parse(n.GetValue(key)); }
            catch (Exception) { res = defaultValue; }
            return res;
        }

        public static string FCSToString(FlightCtrlState flightCtrlState, string title = "")
        {
            string res = "";
            if (flightCtrlState.pitch != 0) res += "Pitch: " + flightCtrlState.pitch + "   ";
            if (flightCtrlState.pitchTrim != 0) res += "Pitch Trim: " + flightCtrlState.pitchTrim + "   ";
            if (flightCtrlState.yaw != 0) res += "Yaw: " + flightCtrlState.yaw + "   ";
            if (flightCtrlState.yawTrim != 0) res += "Yaw Trim: " + flightCtrlState.yawTrim + "   ";
            if (flightCtrlState.roll != 0) res += "Roll: " + flightCtrlState.roll + "   ";
            if (flightCtrlState.rollTrim != 0) res += "Roll Trim: " + flightCtrlState.rollTrim + "   ";
            if (flightCtrlState.mainThrottle != 0) res += "Throttle: " + flightCtrlState.mainThrottle;
            return (((title != "") && (res != "")) ? title + ": " : "") + res;
            return res;
        }

        public static void ShowNotification(string msg)
        { ScreenMessages.PostScreenMessage(msg); }

        /// <summary>
        /// Log levels:
        /// <list type="bullet">
        /// <item><definition>None: do not log</definition></item>
        /// <item><definition>Error: log only errors</definition></item>
        /// <item><definition>Important: log only errors and important information</definition></item>
        /// <item><definition>Debug: log all information</definition></item>
        /// </list>
        /// </summary>
        public enum LogLevel { None, Error, Important, Debug };

        /// <summary>
        /// Current <see cref="LogLevel"/>: either Debug or Important
        /// </summary>
        public static LogLevel Level
        { get { return SignalDelaySettings.DebugMode ? LogLevel.Debug : LogLevel.Important; } }

        /// <summary>
        /// Write into output_log.txt
        /// </summary>
        /// <param name="message">Text to log</param>
        /// <param name="messageLevel"><see cref="LogLevel"/> of the entry</param>
        public static void Log(string message, LogLevel messageLevel = LogLevel.Debug)
        { if ((messageLevel <= Level) && (message != "")) Debug.Log("[SignalDelay] " + (messageLevel == LogLevel.Error ? "ERROR: " : "") + message); }
    }
}
