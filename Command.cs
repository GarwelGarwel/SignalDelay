using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SignalDelay
{
    public enum CommandType { LAUNCH_STAGES, PITCH_DOWN, PITCH_UP, YAW_LEFT, YAW_RIGHT, ROLL_LEFT, ROLL_RIGHT, TRANSLATE_FWD, TRANSLATE_BACK, TRANSLATE_DOWN, TRANSLATE_UP, TRANSLATE_LEFT, TRANSLATE_RIGHT, THROTTLE_CUTOFF, THROTTLE_FULL, THROTTLE_DOWN, THROTTLE_UP, WHEEL_STEER_LEFT, WHEEL_STEER_RIGHT, WHEEL_THROTTLE_DOWN, WHEEL_THROTTLE_UP, LANDING_GEAR, SAS_TOGGLE, SAS_HOLD };

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
                case CommandType.TRANSLATE_FWD:
                    break;
                case CommandType.THROTTLE_CUTOFF:
                    SignalDelayScenario.FlightCtrlState.mainThrottle = 0;
                    break;
                case CommandType.THROTTLE_FULL:
                    SignalDelayScenario.FlightCtrlState.mainThrottle = 1;
                    break;
                case CommandType.THROTTLE_DOWN:
                    SignalDelayScenario.FlightCtrlState.mainThrottle -= 0.01f;
                    break;
                case CommandType.THROTTLE_UP:
                    SignalDelayScenario.FlightCtrlState.mainThrottle += 0.01f;
                    break;
                case CommandType.LANDING_GEAR:
                    SignalDelayScenario.FlightCtrlState.gearDown = !SignalDelayScenario.FlightCtrlState.gearDown;
                    //SignalDelayScenario.FlightCtrlState.gearUp = !SignalDelayScenario.FlightCtrlState.gearUp;
                    break;
                default:
                    Core.Log("Unimplemented command " + Type);
                    break;
            }
        }

        public override string ToString()
        { return Type + " @ " + Time; }

        public Command(CommandType type, double time)
        {
            Type = type;
            Time = time;
        }
    }
}
