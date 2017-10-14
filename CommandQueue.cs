using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SignalDelay
{
    public class CommandQueue : Queue<Command>
    {
        public void Enqueue(CommandType command)
        {
            Core.Log("Adding command " + command + " at " + Planetarium.GetUniversalTime() + ".");
            Core.ShowNotification("Input: " + command.ToString());
            base.Enqueue(new Command(command, Planetarium.GetUniversalTime() + Delay));
        }

        public new Command Dequeue()
        {
            Command res = base.Dequeue();
            Core.Log("Executing command " + res + ".");
            Core.ShowNotification("Execute: " + res.Type);
            res.Execute();
            return res;
        }

        public double Delay { get; set; }

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
