using CommNet;
using KSP.UI.Screens;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace SignalDelay
{
    [KSPScenario(ScenarioCreationOptions.AddToAllGames, GameScenes.FLIGHT)]
    public class SignalDelayScenario : ScenarioModule
    {
        #region LIFE CYCLE METHODS

        IButton toolbarButton;
        ApplicationLauncherButton appLauncherButton;
        Texture2D icon = new Texture2D(38, 38);

        ScreenMessage delayMsg = new ScreenMessage("", 1, ScreenMessageStyle.UPPER_LEFT);

        public void Start()
        {
            if (ToolbarManager.ToolbarAvailable)
            {
                Core.Log("Registering Blizzy's Toolbar button...", LogLevel.Important);
                toolbarButton = ToolbarManager.Instance.add("SignalDelay", "SignalDelay");
                toolbarButton.Text = "Signal Delay";
                toolbarButton.TexturePath = "SignalDelay/icon24";
                toolbarButton.ToolTip = "Switch Signal Delay";
                toolbarButton.OnClick += e => { ToggleMod(); };
            }
            if (SignalDelaySettings.Instance.AppLauncherButton)
            {
                icon.LoadImage(File.ReadAllBytes(System.IO.Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "icon128.png")));
                appLauncherButton = ApplicationLauncher.Instance.AddModApplication(ToggleMod, ToggleMod, null, null, null, null, ApplicationLauncher.AppScenes.FLIGHT, icon);
            }
            ResetButtonState();

            GameEvents.onVesselSwitching.Add(OnVesselSwitching);
            GameEvents.CommNet.OnCommStatusChange.Add(ResetButtonState);
        }

        public void OnDisable()
        {
            GameEvents.onVesselSwitching.Remove(OnVesselSwitching);
            GameEvents.CommNet.OnCommStatusChange.Remove(ResetButtonState);
            if (toolbarButton != null)
                toolbarButton.Destroy();
            if (appLauncherButton != null && ApplicationLauncher.Instance != null)
                ApplicationLauncher.Instance.RemoveModApplication(appLauncherButton);
            Active = false;
        }

        /// <summary>
        /// Deactivates signal delay before switching vessels; resets button's enabled state
        /// </summary>
        public void OnVesselSwitching(Vessel from, Vessel to)
        {
            Core.Log("OnVesselSwitching(" + from.vesselName + ", " + to.vesselName + ")");
            Active = false;
            ResetButtonState();
        }

        /// <summary>
        /// Checks key presses and SAS mode changes and queues them for execution if signal delay is active
        /// </summary>
        public void Update()
        {
            if (!Active)
                return;

            // Checking if kOS terminal is focused and locks control => ignoring input then
            if (InputLockManager.lockStack.ContainsKey("kOSTerminal"))
                return;

            // Checking all key presses and enqueing corresponding actions
            if (GameSettings.LAUNCH_STAGES.GetKeyDown())
                Enqueue(CommandType.LAUNCH_STAGES);
            if (GameSettings.PITCH_DOWN.GetKey())
                Enqueue(CommandType.PITCH_DOWN);
            if (GameSettings.PITCH_UP.GetKey())
                Enqueue(CommandType.PITCH_UP);
            if (GameSettings.YAW_LEFT.GetKey())
                Enqueue(CommandType.YAW_LEFT);
            if (GameSettings.YAW_RIGHT.GetKey())
                Enqueue(CommandType.YAW_RIGHT);
            if (GameSettings.ROLL_LEFT.GetKey())
                Enqueue(CommandType.ROLL_LEFT);
            if (GameSettings.ROLL_RIGHT.GetKey())
                Enqueue(CommandType.ROLL_RIGHT);
            if (GameSettings.TRANSLATE_FWD.GetKey())
                Enqueue(CommandType.TRANSLATE_FWD);
            if (GameSettings.TRANSLATE_BACK.GetKey())
                Enqueue(CommandType.TRANSLATE_BACK);
            if (GameSettings.TRANSLATE_DOWN.GetKey())
                Enqueue(CommandType.TRANSLATE_DOWN);
            if (GameSettings.TRANSLATE_UP.GetKey())
                Enqueue(CommandType.TRANSLATE_UP);
            if (GameSettings.TRANSLATE_LEFT.GetKey())
                Enqueue(CommandType.TRANSLATE_LEFT);
            if (GameSettings.TRANSLATE_RIGHT.GetKey())
                Enqueue(CommandType.TRANSLATE_RIGHT);
            if (GameSettings.THROTTLE_CUTOFF.GetKeyDown())
                Enqueue(CommandType.THROTTLE_CUTOFF);
            if (GameSettings.THROTTLE_FULL.GetKeyDown())
                Enqueue(CommandType.THROTTLE_FULL);
            if (GameSettings.THROTTLE_DOWN.GetKey())
                Enqueue(CommandType.THROTTLE_DOWN);
            if (GameSettings.THROTTLE_UP.GetKey())
                Enqueue(CommandType.THROTTLE_UP);
            if (GameSettings.WHEEL_STEER_LEFT.GetKey())
                Enqueue(CommandType.WHEEL_STEER_LEFT);
            if (GameSettings.WHEEL_STEER_RIGHT.GetKey())
                Enqueue(CommandType.WHEEL_STEER_RIGHT);
            if (GameSettings.WHEEL_THROTTLE_DOWN.GetKey())
                Enqueue(CommandType.WHEEL_THROTTLE_DOWN);
            if (GameSettings.WHEEL_THROTTLE_UP.GetKey())
                Enqueue(CommandType.WHEEL_THROTTLE_UP);
            if (GameSettings.HEADLIGHT_TOGGLE.GetKeyDown())
                Enqueue(CommandType.LIGHT_TOGGLE);
            if (GameSettings.LANDING_GEAR.GetKeyDown())
                Enqueue(CommandType.LANDING_GEAR);
            if (GameSettings.BRAKES.GetKeyDown())
                Enqueue(CommandType.BRAKES);
            if (GameSettings.BRAKES.GetKeyUp())
                Enqueue(CommandType.BRAKES);
            if (GameSettings.RCS_TOGGLE.GetKeyDown())
                Enqueue(CommandType.RCS_TOGGLE);
            if (GameSettings.SAS_TOGGLE.GetKeyDown())
                Enqueue(CommandType.SAS_TOGGLE);
            if (GameSettings.SAS_HOLD.GetKeyDown())
                Enqueue(CommandType.SAS_TOGGLE);
            if (GameSettings.SAS_HOLD.GetKeyUp())
                Enqueue(CommandType.SAS_TOGGLE);
            if (GameSettings.AbortActionGroup.GetKeyDown())
                Enqueue(CommandType.ABORT);
            if (GameSettings.CustomActionGroup1.GetKeyDown())
                Enqueue(CommandType.ACTIONGROUP1);
            if (GameSettings.CustomActionGroup2.GetKeyDown())
                Enqueue(CommandType.ACTIONGROUP2);
            if (GameSettings.CustomActionGroup3.GetKeyDown())
                Enqueue(CommandType.ACTIONGROUP3);
            if (GameSettings.CustomActionGroup4.GetKeyDown())
                Enqueue(CommandType.ACTIONGROUP4);
            if (GameSettings.CustomActionGroup5.GetKeyDown())
                Enqueue(CommandType.ACTIONGROUP5);
            if (GameSettings.CustomActionGroup6.GetKeyDown())
                Enqueue(CommandType.ACTIONGROUP6);
            if (GameSettings.CustomActionGroup7.GetKeyDown())
                Enqueue(CommandType.ACTIONGROUP7);
            if (GameSettings.CustomActionGroup8.GetKeyDown())
                Enqueue(CommandType.ACTIONGROUP8);
            if (GameSettings.CustomActionGroup9.GetKeyDown())
                Enqueue(CommandType.ACTIONGROUP9);
            if (GameSettings.CustomActionGroup10.GetKeyDown())
                Enqueue(CommandType.ACTIONGROUP10);

            // If the user has changed SAS mode, enqueue this command and reset mode
            if (Vessel.Autopilot.Mode != sasMode)
            {
                Enqueue(CommandType.SAS_CHANGE_MODE, Vessel.Autopilot.Mode);
                Vessel.Autopilot.SetMode(sasMode);
            }
        }

        public void FixedUpdate()
        {
            CheckVessel();
            if (!Active)
                return;
            if (Core.IsLogging())
                Core.Log(Core.FCSToString(Vessel.ctrlState, "Vessel FCS"));
            delayRecalculated = false;

            FlightCtrlState.pitch = FlightCtrlState.yaw = FlightCtrlState.roll = 0;
            FadeOut(ref FlightCtrlState.wheelSteer, 0.1f);
            FadeOut(ref FlightCtrlState.wheelThrottle, 0.1f);

            double time = Planetarium.GetUniversalTime();
            while (time >= Queue.NextCommandTime)
                Queue.Dequeue();

            sasMode = Vessel.Autopilot.Mode;

            if (SignalDelaySettings.Instance.ShowDelay)
            {
                delayMsg.message = "Delay: " + Core.FormatTime(Delay);
                ScreenMessages.PostScreenMessage(delayMsg);
            }
        }

        void FadeOut(ref float v, float amount)
        {
            if (v > amount)
                v -= amount;
            else if (v < -amount)
                v += amount;
            else v = 0;
        }

        #endregion LIFE CYCLE METHODS

        #region MOD CONTROL METHODS

        bool active;

        public bool IsConnected => Vessel?.Connection?.IsConnected ?? false;

        public bool IsProbe => (Vessel.Connection.ControlState & VesselControlState.Probe) != 0 && !Vessel.isEVA;

        /// <summary>
        /// Whether signal delay should be applied
        /// </summary>
        bool Active
        {
            get => active;
            set
            {
                if (value == active)
                    return;
                Core.Log("Active = " + value);
                active = value;
                ResetButtonState();
                if (active)
                {
                    Vessel.OnFlyByWire += OnFlyByWire;
                    FlightCtrlState = new FlightCtrlState()
                    { mainThrottle = throttleCache = Vessel.ctrlState.mainThrottle };
                    Core.Log($"Cached throttle = {throttleCache}");
                    sasMode = Vessel.Autopilot.Mode;
                    InputLockManager.SetControlLock(SignalDelaySettings.Instance.HidePartActions ? ControlTypes.ALL_SHIP_CONTROLS : ControlTypes.ALL_SHIP_CONTROLS & ~(ControlTypes.ACTIONS_ALL | ControlTypes.TWEAKABLES | ControlTypes.LINEAR), "this");
                    if (Core.IsLogging())
                        Core.ShowNotification("Signal delay activated.");
                }
                else
                {
                    Vessel.OnFlyByWire -= OnFlyByWire;
                    InputLockManager.RemoveControlLock("this");
                    Core.Log($"Deactivating signal delay. Setting main throttle to {FlightCtrlState.mainThrottle} (was {Vessel.ctrlState.mainThrottle}).");
                    Vessel.ctrlState.mainThrottle = FlightCtrlState.mainThrottle;
                    if (Core.IsLogging())
                        Core.ShowNotification("Signal delay deactivated.");
                }
            }
        }

        /// <summary>
        /// Toggles mod's enabled state on button click
        /// </summary>
        public void ToggleMod() => SignalDelaySettings.Instance.IsEnabled = !SignalDelaySettings.Instance.IsEnabled;

        /// <summary>
        /// Checks whether signal delay should be applied to the active vessel
        /// </summary>
        public void CheckVessel() => Active = SignalDelaySettings.Instance.IsEnabled && IsConnected && IsProbe;

        /// <summary>
        /// Enables or disables the AppLauncher/Toolbar button based on vessel's control type (probe or not) and connection state
        /// </summary>
        void ResetButtonState()
        {
            bool showButton = IsConnected && IsProbe;
            if (appLauncherButton != null)
                appLauncherButton.VisibleInScenes = showButton ? ApplicationLauncher.AppScenes.FLIGHT : ApplicationLauncher.AppScenes.NEVER;
            if (toolbarButton != null)
                toolbarButton.Enabled = showButton;
        }

        void ResetButtonState(Vessel v, bool state)
        {
            Core.Log($"ResetButtonState('{v.vesselName}', {state})");
            if (v.isActiveVessel)
                ResetButtonState();
        }

        #endregion MOD CONTROL METHODS

        #region COMMAND QUEUE METHODS

        /// <summary>
        /// Active vessel's command queue
        /// </summary>
        public CommandQueue Queue
        {
            get => Vessel.vesselModules.OfType<SignalDelayVesselModule>().FirstOrDefault()?.Queue;
            set
            {
                foreach (SignalDelayVesselModule vm in Vessel.vesselModules.OfType<SignalDelayVesselModule>())
                {
                    vm.Queue = value;
                    return;
                }
            }
        }

        /// <summary>
        /// Adds a command with possible parameters to the queue
        /// </summary>
        /// <param name="commandType"></param>
        /// <param name="par"></param>
        void Enqueue(CommandType commandType, params object[] par)
        {
            double time = Planetarium.GetUniversalTime();
            Core.Log("Adding command " + commandType + " at " + time + ".");
            Command c = new Command(commandType, time + Delay)
            { Params = new List<object>(par) };
            Queue.Enqueue(c);
        }

        #endregion COMMAND QUEUE METHODS

        #region VESSEL METHODS

        bool delayRecalculated = false;
        double delay;
        float throttleCache;
        VesselAutopilot.AutopilotMode sasMode;
        bool sasPaused = false;
        public static FlightCtrlState FlightCtrlState { get; set; } = new FlightCtrlState();

        /// <summary>
        /// Current signal delay in seconds
        /// </summary>
        public double Delay
        {
            get
            {
                if (!delayRecalculated)
                    CalculateDelay();
                return delay;
            }
            set
            {
                delay = value;
                delayRecalculated = true;
            }
        }

        Vessel Vessel => FlightGlobals.ActiveVessel;

        /// <summary>
        /// Updates FlightCtrlState for the active vessel
        /// </summary>
        /// <param name="fcs"></param>
        public void OnFlyByWire(FlightCtrlState fcs)
        {
            if (Active)
            {
                if (Core.IsLogging())
                    Core.Log(Core.FCSToString(FlightCtrlState, "SignalDelay FCS"));

                if (Vessel.Autopilot.Enabled && sasMode == VesselAutopilot.AutopilotMode.StabilityAssist && (FlightCtrlState.pitch != 0 || FlightCtrlState.yaw != 0 || FlightCtrlState.roll != 0))
                {
                    Core.Log("User is steering the vessel in StabilityAssist mode. Temporarily disabling autopilot.");
                    Vessel.Autopilot.Disable();
                    sasPaused = true;
                }
                else if (sasPaused)
                {
                    Core.Log("No user steering. Re-enabling autopilot.");
                    Vessel.Autopilot.Enable();
                    sasPaused = false;
                }

                fcs.pitch += FlightCtrlState.pitch;
                fcs.yaw += FlightCtrlState.yaw;
                fcs.roll += FlightCtrlState.roll;
                if (fcs.mainThrottle == throttleCache)
                    fcs.mainThrottle = FlightCtrlState.mainThrottle;
                else
                {
                    Core.Log($"Throttle has been changed from {throttleCache} to {fcs.mainThrottle} by another mod.");
                    FlightCtrlState.mainThrottle = throttleCache = fcs.mainThrottle;
                }
                fcs.wheelSteer = FlightCtrlState.wheelSteer;
                fcs.wheelThrottle = FlightCtrlState.wheelThrottle;
            }
        }

        void CalculateDelay()
        {
            if (Vessel?.Connection?.ControlPath == null)
            {
                Core.Log($"Cannot access control path for {Vessel?.vesselName}, delay set to 0.", LogLevel.Error);
                Delay = 0;
            }
            else Delay = Vessel.Connection.ControlPath.Sum(link => Vector3d.Distance(link.a.position, link.b.position)) / Core.LightSpeed;
        }

        #endregion VESSEL METHODS
    }
}
