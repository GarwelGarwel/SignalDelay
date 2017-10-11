using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using CommNet;

namespace SignalDelay
{
    [KSPScenario(ScenarioCreationOptions.AddToAllGames, GameScenes.FLIGHT)]
    public class SignalDelayScenario : ScenarioModule
    {
        CommandQueue queue;
        public static FlightCtrlState FlightCtrlState { get; set; }

        public void Start()
        {
            Core.Log("Start");
            queue = new CommandQueue() { Delay = GetDelay() };
            FlightCtrlState = new FlightCtrlState();
            FlightGlobals.ActiveVessel.OnFlyByWire += OnFlyByWire;
            GameEvents.CommNet.OnNetworkInitialized.Add(OnNetworkInitialized);

            // Logging game settings
            Core.Log("AXIS_PITCH = " + GameSettings.AXIS_PITCH.primary.sensitivity + ", " + GameSettings.AXIS_PITCH.secondary.sensitivity);
            Core.Log("AXIS_YAW = " + GameSettings.AXIS_YAW.primary.sensitivity + ", " + GameSettings.AXIS_YAW.secondary.sensitivity);
            Core.Log("AXIS_ROLL = " + GameSettings.AXIS_ROLL.primary.sensitivity + ", " + GameSettings.AXIS_ROLL.secondary.sensitivity);
            Core.Log("AXIS_THROTTLE = " + GameSettings.AXIS_THROTTLE.primary.sensitivity + ", " + GameSettings.AXIS_THROTTLE.secondary.sensitivity);
            Core.Log("AXIS_THROTTLE_INC = " + GameSettings.AXIS_THROTTLE_INC.primary.sensitivity + ", " + GameSettings.AXIS_THROTTLE_INC.secondary.sensitivity);
            Core.Log("AxisSensitivityMin = " + GameSettings.AxisSensitivityMin);
            Core.Log("AxisSensitivityMax = " + GameSettings.AxisSensitivityMax);
            Core.Log("INPUT_KEYBOARD_SENSIVITITY = " + GameSettings.INPUT_KEYBOARD_SENSIVITITY);
        }

        public void OnDestroy()
        {
            Core.Log("OnDestroy");
            GameEvents.CommNet.OnNetworkInitialized.Remove(OnNetworkInitialized);
            Active = false;
        }

        public void OnFlyByWire(FlightCtrlState fcs)
        {
            if (Active) fcs.CopyFrom(FlightCtrlState);
        }

        public void OnNetworkInitialized()
        { Active = true; }

        void RegisterInput()
        {
            if (GameSettings.HEADLIGHT_TOGGLE.GetKeyDown()) Active = !Active;
            if (!Active) return;
            if (GameSettings.LAUNCH_STAGES.GetKeyDown()) queue.Enqueue(CommandType.LAUNCH_STAGES);
            if (GameSettings.PITCH_DOWN.GetKey()) queue.Enqueue(CommandType.PITCH_DOWN);
            if (GameSettings.PITCH_UP.GetKey()) queue.Enqueue(CommandType.PITCH_UP);
            if (GameSettings.YAW_LEFT.GetKey()) queue.Enqueue(CommandType.YAW_LEFT);
            if (GameSettings.YAW_RIGHT.GetKey()) queue.Enqueue(CommandType.YAW_RIGHT);
            if (GameSettings.ROLL_LEFT.GetKey()) queue.Enqueue(CommandType.ROLL_LEFT);
            if (GameSettings.ROLL_RIGHT.GetKey()) queue.Enqueue(CommandType.ROLL_RIGHT);
            if (GameSettings.TRANSLATE_FWD.GetKey()) queue.Enqueue(CommandType.TRANSLATE_FWD);
            if (GameSettings.TRANSLATE_BACK.GetKey()) queue.Enqueue(CommandType.TRANSLATE_BACK);
            if (GameSettings.TRANSLATE_DOWN.GetKey()) queue.Enqueue(CommandType.TRANSLATE_DOWN);
            if (GameSettings.TRANSLATE_UP.GetKey()) queue.Enqueue(CommandType.TRANSLATE_UP);
            if (GameSettings.TRANSLATE_LEFT.GetKey()) queue.Enqueue(CommandType.TRANSLATE_LEFT);
            if (GameSettings.TRANSLATE_RIGHT.GetKey()) queue.Enqueue(CommandType.TRANSLATE_RIGHT);
            if (GameSettings.THROTTLE_CUTOFF.GetKey()) queue.Enqueue(CommandType.THROTTLE_CUTOFF);
            if (GameSettings.THROTTLE_FULL.GetKey()) queue.Enqueue(CommandType.THROTTLE_FULL);
            if (GameSettings.THROTTLE_DOWN.GetKey()) queue.Enqueue(CommandType.THROTTLE_DOWN);
            if (GameSettings.THROTTLE_UP.GetKey()) queue.Enqueue(CommandType.THROTTLE_UP);
            if (GameSettings.LANDING_GEAR.GetKeyDown()) queue.Enqueue(CommandType.LANDING_GEAR);
            if (GameSettings.SAS_TOGGLE.GetKeyDown()) queue.Enqueue(CommandType.SAS_TOGGLE);
            if (GameSettings.SAS_TOGGLE.GetKey()) queue.Enqueue(CommandType.SAS_HOLD);
        }

        void DisplayFCS(FlightCtrlState fcs)
        {
            Core.Log("Pitch = " + fcs.pitch);
            Core.Log("Yaw = " + fcs.yaw);
            Core.Log("Roll = " + fcs.roll);
            Core.Log("Throttle = " + fcs.mainThrottle);
        }

        FlightCtrlState Diff(FlightCtrlState oldFCS, FlightCtrlState newFCS)
        {
            return new FlightCtrlState()
            {
                pitch = newFCS.pitch - oldFCS.pitch,
                yaw = newFCS.yaw - oldFCS.yaw,
                roll = newFCS.roll - oldFCS.roll,
                mainThrottle = newFCS.mainThrottle - oldFCS.mainThrottle
            };
        }

        bool SameFCS(FlightCtrlState fcs1, FlightCtrlState fcs2)
        { return fcs1.pitch == fcs2.pitch && fcs1.yaw == fcs2.yaw && fcs1.roll == fcs2.roll && fcs1.mainThrottle == fcs2.mainThrottle; }

        FlightCtrlState fcs = new FlightCtrlState();
        public void Update()
        {
            FlightCtrlState newFCS = new FlightCtrlState();
            newFCS.CopyFrom(FlightGlobals.ActiveVessel.ctrlState);
            if (!SameFCS(fcs, newFCS))
            {
                //Core.Log("New FlightCtrlState:");
                //DisplayFCS(newFCS);
                //Core.Log("Difference:");
                //DisplayFCS(Diff(fcs, newFCS));
                fcs.CopyFrom(newFCS);
            }
            RegisterInput();
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
                    FlightCtrlState.CopyFrom(FlightGlobals.ActiveVessel.ctrlState);
                    InputLockManager.SetControlLock(ControlTypes.ALL_SHIP_CONTROLS, "this");
                    Core.ShowNotification("Delay activated.");
                }
                else
                {
                    queue.Clear();
                    InputLockManager.RemoveControlLock("this");
                    Core.ShowNotification("Delay deactivated.");
                }
            }
        }

        double GetDelay()
        {
            Core.Log("CommNet path");
            if (FlightGlobals.ActiveVessel?.Connection?.ControlPath == null)
            {
                Core.Log("Cannot access control path for the active vessel, delay set to 0.", Core.LogLevel.Error);
                return 0;
            }
            int i = 1;
            double dist = 0;
            foreach (CommLink l in FlightGlobals.ActiveVessel.Connection.ControlPath)
            {
                double d = Vector3d.Distance(l.a.position, l.b.position);
                Core.Log("Link #" + i++ + ": " + l.a.name + " -> " + l.b.name + " (" + d.ToString("N0") + " m)");
                dist += d;
            }
            Core.Log("Total distance to Control Source: " + dist.ToString("N0") + " m. Delay = " + (dist / Core.LightSpeed).ToString("F2") + " sec.");
            return dist / Core.LightSpeed;
        }

        ScreenMessage delayMsg = new ScreenMessage("", 5, ScreenMessageStyle.UPPER_LEFT);
        public void FixedUpdate()
        {
            if (!Active) return;
            FlightCtrlState.pitch = FlightCtrlState.yaw = FlightCtrlState.roll = 0;
            queue.Delay = GetDelay();
            delayMsg.message = "Delay: " + queue.Delay.ToString("F2") + " sec";
            ScreenMessages.PostScreenMessage(delayMsg);
            while (Planetarium.GetUniversalTime() >= queue.NextCommandTime)
                queue.Dequeue();
        }
    }
}
