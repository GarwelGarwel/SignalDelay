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
        public static bool IsEnabled => HighLogic.CurrentGame.Parameters.CustomParams<SignalDelaySettings>().enabled;

        [GameParameters.CustomFloatParameterUI("Speed of Light", toolTip = "How fast signal travels, m/s", minValue = 1e8f, maxValue = 1e9f)]
        public float lightSpeed = 299792458;
        public static float LightSpeed => HighLogic.CurrentGame.Parameters.CustomParams<SignalDelaySettings>().lightSpeed;

        [GameParameters.CustomParameterUI("Round Trip Time", toolTip = "Delay is doubled to include time for the signal to return back")]
        public bool roundtrip = true;
        public static bool RoundTrip => HighLogic.CurrentGame.Parameters.CustomParams<SignalDelaySettings>().roundtrip;

        [GameParameters.CustomParameterUI("Hide Part Actions", toolTip = "Prevent part actions (e.g. science experiments) from being accessible through right-click menu. Use action groups instead!")]
        public bool hidePartActions = false;
        public static bool HidePartActions => HighLogic.CurrentGame.Parameters.CustomParams<SignalDelaySettings>().hidePartActions;

        [GameParameters.CustomParameterUI("Display Delay", toolTip = "Show delay, in seconds, in the top-left corner")]
        public bool showDelay = false;
        public static bool ShowDelay => HighLogic.CurrentGame.Parameters.CustomParams<SignalDelaySettings>().showDelay;

        [GameParameters.CustomParameterUI("Debug Mode", toolTip = "Verbose logging, obligatory for bug submissions")]
        public bool debugMode = true;
        public static bool DebugMode => HighLogic.CurrentGame.Parameters.CustomParams<SignalDelaySettings>().debugMode;
    }
}
