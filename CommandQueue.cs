using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SignalDelay
{
    public class CommandQueue : Queue<Command>
    {
        public new Command Dequeue()
        {
            Command res = base.Dequeue();
            Core.Log("Executing command " + res + ".");
            if (TimeWarp.CurrentRate > TimeWarp.MaxPhysicsRate) TimeWarp.SetRate(0, true);
            if (SignalDelaySettings.DebugMode) Core.ShowNotification("Execute: " + res.Type);
            res.Execute();
            return res;
        }

        public double NextCommandTime
        {
            get
            {
                if (Count == 0) return double.PositiveInfinity;
                return Peek().Time;
            }
        }

        public ConfigNode ConfigNode
        {
            get
            {
                ConfigNode node = new ConfigNode("CommandQueue");
                foreach (Command c in this)
                    node.AddNode(c.ConfigNode);
                Core.Log(node.CountNodes + " commands saved.");
                return node;
            }
            set
            {
                foreach (ConfigNode n in value.GetNodes("Command"))
                    Enqueue(new Command(n));
                Core.Log(value.GetNodes("Command").Length + " commands loaded.");
            }
        }

        public CommandQueue() : base()
        { }

        public CommandQueue(ConfigNode node) : base(node.CountNodes)
        { ConfigNode = node; }
    }
}
