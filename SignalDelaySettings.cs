namespace SignalDelay
{
    class SignalDelaySettings : GameParameters.CustomParameterNode
    {
        [GameParameters.CustomParameterUI("Delay Enabled", toolTip = "Enable signal delays for probes")]
        public bool IsEnabled = true;

        [GameParameters.CustomParameterUI("AppLauncher Button", toolTip = "Show an AppLauncher button to quickly disable/enable delay")]
        public bool AppLauncherButton = true;

        [GameParameters.CustomFloatParameterUI("Speed of Light, m/s", toolTip = "How fast signal travels, m/s", minValue = 1e8f, maxValue = 1e9f)]
        public float LightSpeed = 299792458;

        [GameParameters.CustomParameterUI("Round Trip Time", toolTip = "Delay is doubled to include time for the signal to return back")]
        public bool Roundtrip = true;

        [GameParameters.CustomFloatParameterUI("Sensitivity", toolTip = "How fast throttle moves with key presses", minValue = 0.5f, maxValue = 2, asPercentage = true, displayFormat = "F2")]
        public float ThrottleSensitivity = 1;

        [GameParameters.CustomParameterUI("Hide Part Actions", toolTip = "Prevent part actions (e.g. science experiments) from being accessible through right-click menu. Use action groups instead!")]
        public bool HidePartActions = false;

        [GameParameters.CustomParameterUI("Display Delay", toolTip = "Show delay, in seconds, in the top-left corner")]
        public bool ShowDelay = true;

        [GameParameters.CustomParameterUI("EC Usage Enabled", toolTip = "Enable EC usage by antennas for telemetry (independent from delay setting)")]
        public bool ECUsage = true;

        [GameParameters.CustomFloatParameterUI("EC Usage @ 100% Signal Strength", toolTip = "How much EC is used when having 100% connection, relative to maximum usage", asPercentage = true, displayFormat = "F1", minValue = 0, maxValue = 1, stepCount = 11)]
        public float ECBonus = 0.5f;

        [GameParameters.CustomParameterUI("Debug Mode", toolTip = "Verbose logging, obligatory for bug submissions")]
        public bool DebugMode = false;

        public static SignalDelaySettings Instance => HighLogic.CurrentGame.Parameters.CustomParams<SignalDelaySettings>();

        public override string Title => "Signal Delay Settings";

        public override GameParameters.GameMode GameMode => GameParameters.GameMode.ANY;

        public override string Section => "SignalDelay";

        public override string DisplaySection => "Signal Delay";

        public override int SectionOrder => 1;

        public override bool HasPresets => false;
    }
}
