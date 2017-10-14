using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SignalDelay
{
    public enum CommandType
    {
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
        SAS_HOLD,
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
        public CommandType Type { get; set; }
        public double Time { get; set; }

        public void Execute()
        {
            Vessel v = FlightGlobals.ActiveVessel;
            switch (Type)
            {
                case CommandType.LAUNCH_STAGES:
                    KSP.UI.Screens.StageManager.ActivateNextStage();
                    break;
                case CommandType.PITCH_DOWN:
                    SignalDelayScenario.FCSChange.pitch = -1;
                    break;
                case CommandType.PITCH_UP:
                    SignalDelayScenario.FCSChange.pitch = 1;
                    break;
                case CommandType.YAW_LEFT:
                    SignalDelayScenario.FCSChange.yaw = -1;
                    break;
                case CommandType.YAW_RIGHT:
                    SignalDelayScenario.FCSChange.yaw = 1;
                    break;
                case CommandType.ROLL_LEFT:
                    SignalDelayScenario.FCSChange.roll = -1;
                    break;
                case CommandType.ROLL_RIGHT:
                    SignalDelayScenario.FCSChange.roll = 1;
                    break;
                case CommandType.TRANSLATE_FWD:
                    v.Translate(new Vector3d(1, 0, 0));
                    break;
                case CommandType.TRANSLATE_UP:
                    v.Translate(new Vector3d(0, 1, 0));
                    break;
                case CommandType.TRANSLATE_RIGHT:
                    v.Translate(new Vector3d(0, 0, 1));
                    break;
                case CommandType.THROTTLE_CUTOFF:
                    SignalDelayScenario.FCSChange.mainThrottle = 0;
                    break;
                case CommandType.THROTTLE_FULL:
                    SignalDelayScenario.FCSChange.mainThrottle = 1;
                    break;
                case CommandType.THROTTLE_DOWN:
                    SignalDelayScenario.FCSChange.mainThrottle -= 0.01f;
                    break;
                case CommandType.THROTTLE_UP:
                    SignalDelayScenario.FCSChange.mainThrottle += 0.01f;
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
                    Core.Log("Autopilot is " + (v.Autopilot.Enabled ? "enabled" : "disabled") + " in " + v.Autopilot.Mode + " mode.");
                    //SignalDelayScenario.SASLock = !SignalDelayScenario.SASLock;
                    v.ActionGroups.ToggleGroup(KSPActionGroup.SAS);
                    break;
                case CommandType.SAS_HOLD:
                    Core.Log("Autopilot is " + (v.Autopilot.Enabled ? "enabled" : "disabled") + " in " + v.Autopilot.Mode + " mode.");
                    //SignalDelayScenario.SASHold = true;
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
                    Core.Log("Unimplemented command " + Type, Core.LogLevel.Error);
                    break;
            }
        }

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
                catch (Exception) { Core.Log("Could not parse command type for this command: " + value, Core.LogLevel.Error); }
                Time = Core.GetDouble(value, "time");
            }
        }

        public override string ToString()
        { return Type + " @ " + Time; }

        public Command(CommandType type, double time)
        {
            Type = type;
            Time = time;
        }

        public Command(ConfigNode node)
        { ConfigNode = node; }
    }
}
