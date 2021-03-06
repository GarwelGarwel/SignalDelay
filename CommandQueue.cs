﻿using System.Collections.Generic;

namespace SignalDelay
{
    public class CommandQueue : Queue<Command>
    {
        public CommandQueue() : base()
        { }

        public CommandQueue(ConfigNode node) : base(node.CountNodes)
            => ConfigNode = node;

        public double NextCommandTime => (Count != 0) ? Peek().Time : double.PositiveInfinity;

        public ConfigNode ConfigNode
        {
            get
            {
                ConfigNode node = new ConfigNode("CommandQueue");
                foreach (Command c in this)
                    node.AddNode(c.ConfigNode);
                if (node.CountNodes > 0)
                    Core.Log($"{node.CountNodes} commands saved.");
                return node;
            }
            set
            {
                foreach (ConfigNode n in value.GetNodes("Command"))
                    Enqueue(new Command(n));
                Core.Log($"{value.GetNodes("Command").Length} commands loaded.");
            }
        }

        public new Command Dequeue()
        {
            Command res = base.Dequeue();
            Core.Log($"Executing command {res}.");
            if (TimeWarp.CurrentRate > TimeWarp.MaxPhysicsRate)
                TimeWarp.SetRate(0, true);
            res.Execute();
            return res;
        }
    }
}
