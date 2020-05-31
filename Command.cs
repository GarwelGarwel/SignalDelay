using System;
using System.Collections.Generic;

namespace SignalDelay
{
    public enum CommandType
    {
        NONE,
        LAUNCH_STAGES,
        PITCH_DOWN,
        PITCH_UP,
        YAW_LEFT,
        YAW_RIGHT,
        ROLL_LEFT,
        ROLL_RIGHT,
        TRANSLATE_FWD,
        TRANSLATE_BACK,
        TRANSLATE_DOWN,
        TRANSLATE_UP,
        TRANSLATE_LEFT,
        TRANSLATE_RIGHT,
        THROTTLE_CUTOFF,
        THROTTLE_FULL,
        THROTTLE_DOWN,
        THROTTLE_UP,
        WHEEL_STEER_LEFT,
        WHEEL_STEER_RIGHT,
        WHEEL_THROTTLE_DOWN,
        WHEEL_THROTTLE_UP,
        LIGHT_TOGGLE,
        LANDING_GEAR,
        BRAKES,
        RCS_TOGGLE,
        SAS_TOGGLE,
        SAS_CHANGE_MODE,
        ABORT,
        ACTIONGROUP1,
        ACTIONGROUP2,
        ACTIONGROUP3,
        ACTIONGROUP4,
        ACTIONGROUP5,
        ACTIONGROUP6,
        ACTIONGROUP7,
        ACTIONGROUP8,
        ACTIONGROUP9,
        ACTIONGROUP10
    };

    public class Command
    {
        public Command(CommandType type, double time)
        {
            Type = type;
            Time = time;
        }

        public Command(ConfigNode node) => ConfigNode = node;

        public CommandType Type { get; set; }
        public double Time { get; set; }
        public List<object> Params { get; set; } = new List<object>();

        public ConfigNode ConfigNode
        {
            get
            {
                ConfigNode node = new ConfigNode("Command");
                node.AddValue("type", Type.ToString());
                node.AddValue("time", Time);
                return node;
            }
            set
            {
                try { Type = (CommandType)Enum.Parse(typeof(CommandType), value.GetValue("type")); }
                catch (Exception)
                {
                    Core.Log("Could not parse command type for this command: " + value, LogLevel.Error);
                    Type = CommandType.NONE;
                }
                Time = value.GetDouble("time");
            }
        }

        public void Execute()
        {
            Vessel v = FlightGlobals.ActiveVessel;
            switch (Type)
            {
                case CommandType.LAUNCH_STAGES:
                    KSP.UI.Screens.StageManager.ActivateNextStage();
                    break;

                case CommandType.PITCH_DOWN:
                    SignalDelayScenario.FlightCtrlState.pitch = -1;
                    break;

                case CommandType.PITCH_UP:
                    SignalDelayScenario.FlightCtrlState.pitch = 1;
                    break;

                case CommandType.YAW_LEFT:
                    SignalDelayScenario.FlightCtrlState.yaw = -1;
                    break;

                case CommandType.YAW_RIGHT:
                    SignalDelayScenario.FlightCtrlState.yaw = 1;
                    break;

                case CommandType.ROLL_LEFT:
                    SignalDelayScenario.FlightCtrlState.roll = -1;
                    break;

                case CommandType.ROLL_RIGHT:
                    SignalDelayScenario.FlightCtrlState.roll = 1;
                    break;

                case CommandType.THROTTLE_CUTOFF:
                    SignalDelayScenario.FlightCtrlState.mainThrottle = 0;
                    break;

                case CommandType.THROTTLE_FULL:
                    SignalDelayScenario.FlightCtrlState.mainThrottle = 1;
                    break;

                case CommandType.THROTTLE_DOWN:
                    SignalDelayScenario.FlightCtrlState.mainThrottle = Math.Max(SignalDelayScenario.FlightCtrlState.mainThrottle - 0.01f * SignalDelaySettings.Instance.ThrottleSensitivity, 0);
                    break;

                case CommandType.THROTTLE_UP:
                    SignalDelayScenario.FlightCtrlState.mainThrottle = Math.Min(SignalDelayScenario.FlightCtrlState.mainThrottle + 0.01f * SignalDelaySettings.Instance.ThrottleSensitivity, 1);
                    break;

                case CommandType.WHEEL_STEER_LEFT:
                    SignalDelayScenario.FlightCtrlState.wheelSteer = 1;
                    break;

                case CommandType.WHEEL_STEER_RIGHT:
                    SignalDelayScenario.FlightCtrlState.wheelSteer = -1;
                    break;

                case CommandType.WHEEL_THROTTLE_DOWN:
                    SignalDelayScenario.FlightCtrlState.wheelThrottle = -1;
                    break;

                case CommandType.WHEEL_THROTTLE_UP:
                    SignalDelayScenario.FlightCtrlState.wheelThrottle = 1;
                    break;

                case CommandType.LIGHT_TOGGLE:
                    v.ActionGroups.ToggleGroup(KSPActionGroup.Light);
                    break;

                case CommandType.LANDING_GEAR:
                    v.ActionGroups.ToggleGroup(KSPActionGroup.Gear);
                    break;

                case CommandType.BRAKES:
                    v.ActionGroups.ToggleGroup(KSPActionGroup.Brakes);
                    break;

                case CommandType.RCS_TOGGLE:
                    v.ActionGroups.ToggleGroup(KSPActionGroup.RCS);
                    break;

                case CommandType.SAS_TOGGLE:
                    v.ActionGroups.ToggleGroup(KSPActionGroup.SAS);
                    break;

                case CommandType.SAS_CHANGE_MODE:
                    if ((Params.Count > 0) && (Params[0] is VesselAutopilot.AutopilotMode) && v.Autopilot.CanSetMode((VesselAutopilot.AutopilotMode)Params[0]))
                        v.Autopilot.SetMode((VesselAutopilot.AutopilotMode)Params[0]);
                    break;

                case CommandType.ABORT:
                    v.ActionGroups.ToggleGroup(KSPActionGroup.Abort);
                    break;

                case CommandType.ACTIONGROUP1:
                    v.ActionGroups.ToggleGroup(KSPActionGroup.Custom01);
                    break;

                case CommandType.ACTIONGROUP2:
                    v.ActionGroups.ToggleGroup(KSPActionGroup.Custom02);
                    break;

                case CommandType.ACTIONGROUP3:
                    v.ActionGroups.ToggleGroup(KSPActionGroup.Custom03);
                    break;

                case CommandType.ACTIONGROUP4:
                    v.ActionGroups.ToggleGroup(KSPActionGroup.Custom04);
                    break;

                case CommandType.ACTIONGROUP5:
                    v.ActionGroups.ToggleGroup(KSPActionGroup.Custom05);
                    break;

                case CommandType.ACTIONGROUP6:
                    v.ActionGroups.ToggleGroup(KSPActionGroup.Custom06);
                    break;

                case CommandType.ACTIONGROUP7:
                    v.ActionGroups.ToggleGroup(KSPActionGroup.Custom07);
                    break;

                case CommandType.ACTIONGROUP8:
                    v.ActionGroups.ToggleGroup(KSPActionGroup.Custom08);
                    break;

                case CommandType.ACTIONGROUP9:
                    v.ActionGroups.ToggleGroup(KSPActionGroup.Custom09);
                    break;

                case CommandType.ACTIONGROUP10:
                    v.ActionGroups.ToggleGroup(KSPActionGroup.Custom10);
                    break;

                default:
                    Core.Log("Unimplemented command " + Type, LogLevel.Error);
                    break;
            }
        }

        public override string ToString() => Type + " @ " + Time;
    }
}
