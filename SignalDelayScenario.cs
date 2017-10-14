using CommNet;

namespace SignalDelay
{
    [KSPScenario(ScenarioCreationOptions.AddToAllGames, GameScenes.FLIGHT)]
    public class SignalDelayScenario : ScenarioModule
    {
        public void Start()
        {
            Core.Log("Start");
            Vessel.OnFlyByWire += OnFlyByWire;
            GameEvents.CommNet.OnNetworkInitialized.Add(CheckVessel);
            GameEvents.CommNet.OnCommStatusChange.Add(OnCommStatusChange);
            GameEvents.onFlightReady.Add(CheckVessel);
            GameEvents.onVesselSwitching.Add(OnVesselSwitching);
            GameEvents.onVesselLoaded.Add(CheckVessel);
        }

        public void OnDisable()
        { 
            Core.Log("OnDisable");
            GameEvents.CommNet.OnNetworkInitialized.Remove(CheckVessel);
            GameEvents.CommNet.OnCommStatusChange.Remove(OnCommStatusChange);
            GameEvents.onFlightReady.Remove(CheckVessel);
            GameEvents.onVesselSwitching.Remove(OnVesselSwitching);
            GameEvents.onVesselLoaded.Remove(CheckVessel);
            Active = false;
        }

        Vessel Vessel { get { return FlightGlobals.ActiveVessel; } }

        public static FlightCtrlState FlightCtrlState { get; set; }

        public CommandQueue Queue
        {
            get
            {
                foreach (VesselModule vm in Vessel.vesselModules)
                    if (vm is SignalDelayModule) return ((SignalDelayModule)vm).Queue;
                return null;
            }
            set
            {
                foreach (VesselModule vm in Vessel.vesselModules)
                    if (vm is SignalDelayModule) ((SignalDelayModule)vm).Queue = value;
            }
        }

        bool active;
        bool Active
        {
            get { return active; }
            set
            {
                Core.Log("Active = " + value);
                if (value == active) return;
                active = value;
                if (active)
                {
                    FlightCtrlState = new FlightCtrlState();
                    FlightCtrlState.CopyFrom(Vessel.ctrlState);
                    InputLockManager.SetControlLock(SignalDelaySettings.HidePartActions ? ControlTypes.ALL_SHIP_CONTROLS : ControlTypes.ALL_SHIP_CONTROLS ^ ControlTypes.ACTIONS_SHIP, "this");
                    if (SignalDelaySettings.DebugMode) Core.ShowNotification("Delay activated.");
                }
                else
                {
                    InputLockManager.RemoveControlLock("this");
                    if (SignalDelaySettings.DebugMode) Core.ShowNotification("Delay deactivated.");
                }
            }
        }

        public void OnFlyByWire(FlightCtrlState fcs)
        { if (Active && !Vessel.Autopilot.Enabled) fcs.CopyFrom(FlightCtrlState); }

        public void CheckVessel(Vessel v)
        {
            Core.Log("Vessel is " + (v.Connection.IsConnected ? "" : "not ") + "connected, control state is " + v.Connection.ControlState + " (" + (int)v.Connection.ControlState + ").");
            Active = SignalDelaySettings.IsEnabled && v.Connection.IsConnected && (v.Connection.ControlState & VesselControlState.Probe) == VesselControlState.Probe;
        }

        public void CheckVessel()
        { CheckVessel(Vessel); }

        public void OnCommStatusChange(Vessel v, bool b)
        {
            Core.Log("OnCommStatusChange(" + v.vesselName + ", " + b + ")");
            if (v != Vessel)
            {
                Core.Log(v.vesselName + " is not the active vessel, aborting.");
                return;
            }
            CheckVessel();
        }

        public void OnVesselSwitching(Vessel from, Vessel to)
        {
            Core.Log("OnVesselSwitching(" + from.vesselName + ", " + to.vesselName + ")");
            Core.Log("Active Vessel is " + Vessel.vesselName);
            CheckVessel(to);
        }

        bool SameFCS(FlightCtrlState fcs1, FlightCtrlState fcs2)
        { return fcs1.pitch == fcs2.pitch && fcs1.yaw == fcs2.yaw && fcs1.roll == fcs2.roll && fcs1.mainThrottle == fcs2.mainThrottle; }

        void RegisterInput()
        {
            if (GameSettings.LAUNCH_STAGES.GetKeyDown()) Queue.Enqueue(CommandType.LAUNCH_STAGES);
            if (GameSettings.PITCH_DOWN.GetKey()) Queue.Enqueue(CommandType.PITCH_DOWN);
            if (GameSettings.PITCH_UP.GetKey()) Queue.Enqueue(CommandType.PITCH_UP);
            if (GameSettings.YAW_LEFT.GetKey()) Queue.Enqueue(CommandType.YAW_LEFT);
            if (GameSettings.YAW_RIGHT.GetKey()) Queue.Enqueue(CommandType.YAW_RIGHT);
            if (GameSettings.ROLL_LEFT.GetKey()) Queue.Enqueue(CommandType.ROLL_LEFT);
            if (GameSettings.ROLL_RIGHT.GetKey()) Queue.Enqueue(CommandType.ROLL_RIGHT);
            if (GameSettings.TRANSLATE_FWD.GetKey()) Queue.Enqueue(CommandType.TRANSLATE_FWD);
            if (GameSettings.TRANSLATE_BACK.GetKey()) Queue.Enqueue(CommandType.TRANSLATE_BACK);
            if (GameSettings.TRANSLATE_DOWN.GetKey()) Queue.Enqueue(CommandType.TRANSLATE_DOWN);
            if (GameSettings.TRANSLATE_UP.GetKey()) Queue.Enqueue(CommandType.TRANSLATE_UP);
            if (GameSettings.TRANSLATE_LEFT.GetKey()) Queue.Enqueue(CommandType.TRANSLATE_LEFT);
            if (GameSettings.TRANSLATE_RIGHT.GetKey()) Queue.Enqueue(CommandType.TRANSLATE_RIGHT);
            if (GameSettings.THROTTLE_CUTOFF.GetKey()) Queue.Enqueue(CommandType.THROTTLE_CUTOFF);
            if (GameSettings.THROTTLE_FULL.GetKey()) Queue.Enqueue(CommandType.THROTTLE_FULL);
            if (GameSettings.THROTTLE_DOWN.GetKey()) Queue.Enqueue(CommandType.THROTTLE_DOWN);
            if (GameSettings.THROTTLE_UP.GetKey()) Queue.Enqueue(CommandType.THROTTLE_UP);
            if (GameSettings.HEADLIGHT_TOGGLE.GetKeyDown()) Queue.Enqueue(CommandType.LIGHT_TOGGLE);
            if (GameSettings.LANDING_GEAR.GetKeyDown()) Queue.Enqueue(CommandType.LANDING_GEAR);
            if (GameSettings.BRAKES.GetKeyDown()) Queue.Enqueue(CommandType.BRAKES);
            if (GameSettings.RCS_TOGGLE.GetKeyDown()) Queue.Enqueue(CommandType.RCS_TOGGLE);
            if (GameSettings.SAS_TOGGLE.GetKeyDown()) Queue.Enqueue(CommandType.SAS_TOGGLE);
            if (GameSettings.AbortActionGroup.GetKeyDown()) Queue.Enqueue(CommandType.ABORT);
            if (GameSettings.CustomActionGroup1.GetKeyDown()) Queue.Enqueue(CommandType.ACTIONGROUP1);
            if (GameSettings.CustomActionGroup2.GetKeyDown()) Queue.Enqueue(CommandType.ACTIONGROUP2);
            if (GameSettings.CustomActionGroup3.GetKeyDown()) Queue.Enqueue(CommandType.ACTIONGROUP3);
            if (GameSettings.CustomActionGroup4.GetKeyDown()) Queue.Enqueue(CommandType.ACTIONGROUP4);
            if (GameSettings.CustomActionGroup5.GetKeyDown()) Queue.Enqueue(CommandType.ACTIONGROUP5);
            if (GameSettings.CustomActionGroup6.GetKeyDown()) Queue.Enqueue(CommandType.ACTIONGROUP6);
            if (GameSettings.CustomActionGroup7.GetKeyDown()) Queue.Enqueue(CommandType.ACTIONGROUP7);
            if (GameSettings.CustomActionGroup8.GetKeyDown()) Queue.Enqueue(CommandType.ACTIONGROUP8);
            if (GameSettings.CustomActionGroup9.GetKeyDown()) Queue.Enqueue(CommandType.ACTIONGROUP9);
            if (GameSettings.CustomActionGroup10.GetKeyDown()) Queue.Enqueue(CommandType.ACTIONGROUP10);
        }

        FlightCtrlState fcs = new FlightCtrlState();
        public void Update()
        {
            if (!Active) return;
            FlightCtrlState newFCS = new FlightCtrlState();
            newFCS.CopyFrom(Vessel.ctrlState);
            if (!SameFCS(fcs, newFCS)) fcs.CopyFrom(newFCS);
            RegisterInput();
        }

        double GetDelay()
        {
            //Core.Log("CommNet path");
            if (Vessel?.Connection?.ControlPath == null)
            {
                Core.Log("Cannot access control path for " + Vessel?.vesselName + ", delay set to 0.", Core.LogLevel.Error);
                return 0;
            }
            int i = 1;
            double dist = 0;
            foreach (CommLink l in Vessel.Connection.ControlPath)
            {
                dist += Vector3d.Distance(l.a.position, l.b.position);
                //Core.Log("Link #" + i++ + ": " + l.a.name + " -> " + l.b.name + " (" + d.ToString("N0") + " m)");
                //dist += d;
            }
            //Core.Log("Total distance to Control Source: " + dist.ToString("N0") + " m. Delay = " + (dist / Core.LightSpeed).ToString("F2") + " sec.");
            return dist / Core.LightSpeed;
        }

        ScreenMessage delayMsg = new ScreenMessage("", 5, ScreenMessageStyle.UPPER_LEFT);
        public void FixedUpdate()
        {
            //CheckVessel();
            FlightCtrlState.pitch = FlightCtrlState.yaw = FlightCtrlState.roll = 0;
            while (Planetarium.GetUniversalTime() >= Queue.NextCommandTime)
                Queue.Dequeue();
            if (!Active) return;
            Queue.Delay = GetDelay();
            if (SignalDelaySettings.ShowDelay)
            {
                delayMsg.message = "Delay: " + Queue.Delay.ToString("F2") + " sec";
                ScreenMessages.PostScreenMessage(delayMsg);
            }
        }
    }
}
