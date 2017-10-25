using System;
using CommNet;

namespace SignalDelay
{
    [KSPScenario(ScenarioCreationOptions.AddToAllGames, GameScenes.FLIGHT)]
    public class SignalDelayScenario : ScenarioModule
    {
        #region LIFE CYCLE METHODS

        public void Start()
        { Vessel.OnFlyByWire += OnFlyByWire; }

        public void OnDisable()
        { Active = false; }

        public void Update()
        {
            //if (GameSettings.LANDING_GEAR.GetKeyDown()) SignalDelaySettings.IsEnabled = !SignalDelaySettings.IsEnabled;  // -- COMMENT AFTER TEST!!!
            if (!Active) return;

            // Checking if kOS terminal is focused and locks control
            if (InputLockManager.lockStack.ContainsKey("kOSTerminal")) return;

            // Checking all key presses and enqueing corresponding actions
            if (GameSettings.LAUNCH_STAGES.GetKeyDown()) Enqueue(CommandType.LAUNCH_STAGES);
            if (GameSettings.PITCH_DOWN.GetKey()) Enqueue(CommandType.PITCH_DOWN);
            if (GameSettings.PITCH_UP.GetKey()) Enqueue(CommandType.PITCH_UP);
            if (GameSettings.YAW_LEFT.GetKey()) Enqueue(CommandType.YAW_LEFT);
            if (GameSettings.YAW_RIGHT.GetKey()) Enqueue(CommandType.YAW_RIGHT);
            if (GameSettings.ROLL_LEFT.GetKey()) Enqueue(CommandType.ROLL_LEFT);
            if (GameSettings.ROLL_RIGHT.GetKey()) Enqueue(CommandType.ROLL_RIGHT);
            if (GameSettings.TRANSLATE_FWD.GetKey()) Enqueue(CommandType.TRANSLATE_FWD);
            if (GameSettings.TRANSLATE_BACK.GetKey()) Enqueue(CommandType.TRANSLATE_BACK);
            if (GameSettings.TRANSLATE_DOWN.GetKey()) Enqueue(CommandType.TRANSLATE_DOWN);
            if (GameSettings.TRANSLATE_UP.GetKey()) Enqueue(CommandType.TRANSLATE_UP);
            if (GameSettings.TRANSLATE_LEFT.GetKey()) Enqueue(CommandType.TRANSLATE_LEFT);
            if (GameSettings.TRANSLATE_RIGHT.GetKey()) Enqueue(CommandType.TRANSLATE_RIGHT);
            if (GameSettings.THROTTLE_CUTOFF.GetKeyDown()) Enqueue(CommandType.THROTTLE_CUTOFF);
            if (GameSettings.THROTTLE_FULL.GetKeyDown()) Enqueue(CommandType.THROTTLE_FULL);
            if (GameSettings.THROTTLE_DOWN.GetKey()) Enqueue(CommandType.THROTTLE_DOWN);
            if (GameSettings.THROTTLE_UP.GetKey()) Enqueue(CommandType.THROTTLE_UP);
            if (GameSettings.WHEEL_STEER_LEFT.GetKey()) Enqueue(CommandType.WHEEL_STEER_LEFT);
            if (GameSettings.WHEEL_STEER_RIGHT.GetKey()) Enqueue(CommandType.WHEEL_STEER_RIGHT);
            if (GameSettings.WHEEL_THROTTLE_DOWN.GetKey()) Enqueue(CommandType.WHEEL_THROTTLE_DOWN);
            if (GameSettings.WHEEL_THROTTLE_UP.GetKey()) Enqueue(CommandType.WHEEL_THROTTLE_UP);
            if (GameSettings.HEADLIGHT_TOGGLE.GetKeyDown()) Enqueue(CommandType.LIGHT_TOGGLE);
            if (GameSettings.LANDING_GEAR.GetKeyDown()) Enqueue(CommandType.LANDING_GEAR);
            if (GameSettings.BRAKES.GetKeyDown()) Enqueue(CommandType.BRAKES);
            if (GameSettings.RCS_TOGGLE.GetKeyDown()) Enqueue(CommandType.RCS_TOGGLE);
            if (GameSettings.SAS_TOGGLE.GetKeyDown()) Enqueue(CommandType.SAS_TOGGLE);
            if (GameSettings.SAS_HOLD.GetKeyDown()) Enqueue(CommandType.SAS_TOGGLE);
            if (GameSettings.SAS_HOLD.GetKeyUp()) Enqueue(CommandType.SAS_TOGGLE);
            if (GameSettings.AbortActionGroup.GetKeyDown()) Enqueue(CommandType.ABORT);
            if (GameSettings.CustomActionGroup1.GetKeyDown()) Enqueue(CommandType.ACTIONGROUP1);
            if (GameSettings.CustomActionGroup2.GetKeyDown()) Enqueue(CommandType.ACTIONGROUP2);
            if (GameSettings.CustomActionGroup3.GetKeyDown()) Enqueue(CommandType.ACTIONGROUP3);
            if (GameSettings.CustomActionGroup4.GetKeyDown()) Enqueue(CommandType.ACTIONGROUP4);
            if (GameSettings.CustomActionGroup5.GetKeyDown()) Enqueue(CommandType.ACTIONGROUP5);
            if (GameSettings.CustomActionGroup6.GetKeyDown()) Enqueue(CommandType.ACTIONGROUP6);
            if (GameSettings.CustomActionGroup7.GetKeyDown()) Enqueue(CommandType.ACTIONGROUP7);
            if (GameSettings.CustomActionGroup8.GetKeyDown()) Enqueue(CommandType.ACTIONGROUP8);
            if (GameSettings.CustomActionGroup9.GetKeyDown()) Enqueue(CommandType.ACTIONGROUP9);
            if (GameSettings.CustomActionGroup10.GetKeyDown()) Enqueue(CommandType.ACTIONGROUP10);

            // If the user has changed SAS mode, enqueue this command and reset mode
            if (Vessel.Autopilot.Mode != sasMode)
            {
                Enqueue(CommandType.SAS_CHANGE_MODE, Vessel.Autopilot.Mode);
                Vessel.Autopilot.SetMode(sasMode);
            }
        }

        ScreenMessage delayMsg = new ScreenMessage("", 1, ScreenMessageStyle.UPPER_LEFT);
        public void FixedUpdate()
        {
            CheckVessel();
            delayRecalculated = false;
            FlightCtrlState.pitch = FlightCtrlState.yaw = FlightCtrlState.roll = FlightCtrlState.wheelSteer = FlightCtrlState.wheelThrottle = 0;
            while (Planetarium.GetUniversalTime() >= Queue.NextCommandTime) Queue.Dequeue();
            sasMode = Vessel.Autopilot.Mode;
            if (!Active) return;
            if (SignalDelaySettings.ShowDelay)
            {
                delayMsg.message = "Delay: " + Core.FormatTime(Delay);
                ScreenMessages.PostScreenMessage(delayMsg);
            }
        }

        #endregion
        #region MOD CONTROL METHODS

        bool active;
        bool Active
        {
            get => active;
            set
            {
                if (value == active) return;
                Core.Log("Active = " + value);
                active = value;
                if (active)
                {
                    FlightCtrlState = new FlightCtrlState()
                    { mainThrottle = throttleCache = Vessel.ctrlState.mainThrottle };
                    Core.Log("Cached throttle = " + throttleCache);
                    sasMode = Vessel.Autopilot.Mode;
                    InputLockManager.SetControlLock(SignalDelaySettings.HidePartActions ? ControlTypes.ALL_SHIP_CONTROLS : ControlTypes.ALL_SHIP_CONTROLS ^ ControlTypes.ACTIONS_SHIP, "this");
                    if (SignalDelaySettings.DebugMode) Core.ShowNotification("Signal delay activated.");
                }
                else
                {
                    InputLockManager.RemoveControlLock("this");
                    if (SignalDelaySettings.DebugMode) Core.ShowNotification("Signal delay deactivated.");
                }
            }
        }

        public void CheckVessel()
        { Active = SignalDelaySettings.IsEnabled && Vessel.Connection.IsConnected && (Vessel.Connection.ControlState & VesselControlState.Probe) == VesselControlState.Probe; }

        #endregion
        #region COMMAND QUEUE METHODS

        public CommandQueue Queue
        {
            get
            {
                foreach (VesselModule vm in Vessel.vesselModules)
                    if (vm is SignalDelayVesselModule) return ((SignalDelayVesselModule)vm).Queue;
                return null;
            }
            set
            {
                foreach (VesselModule vm in Vessel.vesselModules)
                    if (vm is SignalDelayVesselModule) ((SignalDelayVesselModule)vm).Queue = value;
            }
        }

        void Enqueue(CommandType commandType, params object[] par)
        {
            double time = Planetarium.GetUniversalTime();
            Core.Log("Adding command " + commandType + " at " + time + ".");
            if (SignalDelaySettings.DebugMode) Core.ShowNotification("Input: " + commandType.ToString());
            Command c = new Command(commandType, time + Delay);
            foreach (object p in par) c.Params.Add(p);
            Queue.Enqueue(c);
        }

        #endregion
        #region VESSEL METHODS

        Vessel Vessel => FlightGlobals.ActiveVessel;

        bool delayRecalculated = false;
        double delay;
        public double Delay
        {
            get
            {
                if (!delayRecalculated) CalculateDelay();
                return delay;
            }
            set
            {
                delay = value;
                delayRecalculated = true;
            }
        }

        void CalculateDelay()
        {
            if (Vessel?.Connection?.ControlPath == null)
            {
                Core.Log("Cannot access control path for " + Vessel?.vesselName + ", delay set to 0.", Core.LogLevel.Error);
                Delay = 0;
                return;
            }
            double dist = 0;
            foreach (CommLink l in Vessel.Connection.ControlPath)
                dist += Vector3d.Distance(l.a.position, l.b.position);
            Delay = dist / Core.LightSpeed;
        }

        public static FlightCtrlState FlightCtrlState { get; set; } = new FlightCtrlState();
        float throttleCache;
        VesselAutopilot.AutopilotMode sasMode;
        bool sasPaused = false;

        public void OnFlyByWire(FlightCtrlState fcs)
        {
            //Core.Log(Core.FCSToString(fcs, "Input"));
            //Core.Log(Core.FCSToString(FlightCtrlState, "Output"));
            if (Active)
            {
                if (Vessel.Autopilot.Enabled && sasMode == VesselAutopilot.AutopilotMode.StabilityAssist && (FlightCtrlState.pitch != 0 || FlightCtrlState.yaw != 0 || FlightCtrlState.roll != 0))
                {
                    Core.Log("User is steering the vessel in StabilityAssist mode. Temporarily disabling autopilot.", Core.LogLevel.Important);
                    Vessel.Autopilot.Disable();
                    sasPaused = true;
                }
                else if (sasPaused)
                {
                    Core.Log("No user steering. Re-enabling autopilot.", Core.LogLevel.Important);
                    Vessel.Autopilot.Enable();
                    sasPaused = false;
                }
                fcs.pitch += FlightCtrlState.pitch;
                fcs.yaw += FlightCtrlState.yaw;
                fcs.roll += FlightCtrlState.roll;
                fcs.wheelSteer += FlightCtrlState.wheelSteer;
                if (fcs.mainThrottle == throttleCache)  // Checking whether throttle has been changed by any other mod such as kOS
                    fcs.mainThrottle = FlightCtrlState.mainThrottle;
                else FlightCtrlState.mainThrottle = throttleCache = fcs.mainThrottle;
                fcs.wheelThrottle = FlightCtrlState.wheelThrottle;
            }
        }
        #endregion
    }
}

