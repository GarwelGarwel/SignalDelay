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
    }
}
