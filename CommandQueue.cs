using System.Collections.Generic;

namespace SignalDelay
{
    public class CommandQueue : Queue<Command>
    {
        public const string ConfigNodeName = "CommandQueue";

        public CommandQueue() : base()
        { }

        public CommandQueue(ConfigNode node) : base(node.CountNodes) => ConfigNode = node;

        public double NextCommandTime => Count != 0 ? Peek().Time : double.PositiveInfinity;

        public ConfigNode ConfigNode
        {
            get
            {
                ConfigNode node = new ConfigNode(ConfigNodeName);
                foreach (Command c in this)
                    node.AddNode(c.ConfigNode);
                if (Core.IsLogging() && node.CountNodes > 0)
                    Core.Log($"{node.CountNodes} commands saved.");
                return node;
            }

            set
            {
                foreach (ConfigNode n in value.GetNodes(Command.ConfigNodeName))
                    Enqueue(new Command(n));
                if (Core.IsLogging())
                    Core.Log($"{value.GetNodes(Command.ConfigNodeName).Length} commands loaded.");
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
