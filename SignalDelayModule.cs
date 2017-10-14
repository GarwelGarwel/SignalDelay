using System;
using System.Collections.Generic;
using CommNet;

namespace SignalDelay
{
    class SignalDelayModule : CommNetVessel
    {
        public CommandQueue Queue { get; set; } = new CommandQueue();

        bool IsActiveVessel { get { return Vessel == FlightGlobals.ActiveVessel; } }

        protected override void OnSave(ConfigNode node)
        {
            Core.Log("Saving SignalDelayModule for " + Vessel.vesselName + ". Scene is " + HighLogic.LoadedScene + ". Active vessel is " + FlightGlobals.ActiveVessel?.vesselName + ".");
            //Core.Log("Saving SignalDelayModule for " + Vessel.vesselName + ".");
            node.AddNode(Queue.ConfigNode);
        }

        protected override void OnLoad(ConfigNode node)
        {
            Core.Log("Loading SignalDelayModule for " + Vessel.vesselName + ". Scene is " + HighLogic.LoadedScene + ". Active vessel is " + FlightGlobals.ActiveVessel?.vesselName + ".");
            if (node.HasNode("CommandQueue")) Queue = new CommandQueue(node.GetNode("CommandQueue"));
        }
    }
}
