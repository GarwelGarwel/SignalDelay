﻿using UnityEngine;
using KSP.UI.Screens;

namespace SignalDelay
{
    class Core
    {
        public static double LightSpeed
        { get { return 150000; } }

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
        { get { return LogLevel.Debug; } }

        /// <summary>
        /// Write into output_log.txt
        /// </summary>
        /// <param name="message">Text to log</param>
        /// <param name="messageLevel"><see cref="LogLevel"/> of the entry</param>
        public static void Log(string message, LogLevel messageLevel = LogLevel.Debug)
        { if ((messageLevel <= Level) && (message != "")) Debug.Log("[SignalDelay] " + (messageLevel == LogLevel.Error ? "ERROR: " : "") + message); }
    }
}
