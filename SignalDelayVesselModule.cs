using CommNet;

namespace SignalDelay
{
    class SignalDelayVesselModule : CommNetVessel
    {
        public CommandQueue Queue { get; set; } = new CommandQueue();

        protected override void OnSave(ConfigNode node)
        {
            Core.Log($"Saving SignalDelayModule for {Vessel.vesselName}. Scene is {HighLogic.LoadedScene}. Active vessel is {FlightGlobals.ActiveVessel?.vesselName}.");
            node.AddNode(Queue.ConfigNode);
        }

        protected override void OnLoad(ConfigNode node)
        {
            Core.Log($"Loading SignalDelayModule for {Vessel.vesselName}. Scene is {HighLogic.LoadedScene}. Active vessel is {FlightGlobals.ActiveVessel?.vesselName}.");
            ConfigNode n = null;
            if (node.TryGetNode(CommandQueue.ConfigNodeName, ref n))
                Queue = new CommandQueue(n);
        }
    }
}
