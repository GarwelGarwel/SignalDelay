using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SignalDelay
{
    class SignalDelaySettings : GameParameters.CustomParameterNode
    {
        public override string Title => "Signal Delay Settings";

        public override GameParameters.GameMode GameMode => GameParameters.GameMode.ANY;

        public override string Section => "SignalDelay";

        public override string DisplaySection => "Signal Delay";

        public override int SectionOrder => 1;

        public override bool HasPresets => false;

        [GameParameters.CustomParameterUI("Mod Enabled", toolTip = "Enable signal delays for probes")]
        public bool enabled = true;
        public static bool IsEnabled
        {
            get => HighLogic.CurrentGame.Parameters.CustomParams<SignalDelaySettings>().enabled;
            set => HighLogic.CurrentGame.Parameters.CustomParams<SignalDelaySettings>().enabled = value;
        }
        
        [GameParameters.CustomFloatParameterUI("Speed of Light", toolTip = "How fast signal travels, m/s", minValue = 1e8f, maxValue = 1e9f)]
        public float lightSpeed = 299792458;
        public static float LightSpeed
        {
            get => HighLogic.CurrentGame.Parameters.CustomParams<SignalDelaySettings>().lightSpeed;
            set => HighLogic.CurrentGame.Parameters.CustomParams<SignalDelaySettings>().lightSpeed = value;
        }

        [GameParameters.CustomParameterUI("Round Trip Time", toolTip = "Delay is doubled to include time for the signal to return back")]
        public bool roundtrip = true;
        public static bool RoundTrip
        {
            get => HighLogic.CurrentGame.Parameters.CustomParams<SignalDelaySettings>().roundtrip;
            set => HighLogic.CurrentGame.Parameters.CustomParams<SignalDelaySettings>().roundtrip = value;
        }

        [GameParameters.CustomFloatParameterUI("Sensitivity", toolTip = "How fast throttle moves with key presses", minValue = 0.5f, maxValue = 2, asPercentage = true, displayFormat = "F2")]
        public float throttleSensitivity = 1;
        public static float ThrottleSensitivity
        {
            get => HighLogic.CurrentGame.Parameters.CustomParams<SignalDelaySettings>().throttleSensitivity;
            set => HighLogic.CurrentGame.Parameters.CustomParams<SignalDelaySettings>().throttleSensitivity = value;
        }

        [GameParameters.CustomParameterUI("Hide Part Actions", toolTip = "Prevent part actions (e.g. science experiments) from being accessible through right-click menu. Use action groups instead!")]
        public bool hidePartActions = false;
        public static bool HidePartActions
        {
            get => HighLogic.CurrentGame.Parameters.CustomParams<SignalDelaySettings>().hidePartActions;
            set => HighLogic.CurrentGame.Parameters.CustomParams<SignalDelaySettings>().hidePartActions = value;
        }

        [GameParameters.CustomParameterUI("Display Delay", toolTip = "Show delay, in seconds, in the top-left corner")]
        public bool showDelay = false;
        public static bool ShowDelay
        {
            get => HighLogic.CurrentGame.Parameters.CustomParams<SignalDelaySettings>().showDelay;
            set => HighLogic.CurrentGame.Parameters.CustomParams<SignalDelaySettings>().showDelay = value;
        }

        [GameParameters.CustomParameterUI("Debug Mode", toolTip = "Verbose logging, obligatory for bug submissions")]
        public bool debugMode = true;
        public static bool DebugMode
        {
            get => HighLogic.CurrentGame.Parameters.CustomParams<SignalDelaySettings>().debugMode;
            set => HighLogic.CurrentGame.Parameters.CustomParams<SignalDelaySettings>().debugMode = value;
        }
    }
}
