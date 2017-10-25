using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SignalDelay
{
    public class ModuleSignalDelay : PartModule, IResourceConsumer
    {
        [KSPField]
        public float ecRate = 0;

        double lastUpdated;
        int resourceId;

        public List<PartResourceDefinition> GetConsumedResources() => new List<PartResourceDefinition>() { PartResourceLibrary.Instance.GetDefinition("ElectricCharge") };

        ModuleDeployableAntenna DeployableAntenna => part.FindModuleImplementing<ModuleDeployableAntenna>();

        bool IsActive => SignalDelaySettings.IsECUsageEnabled && ((DeployableAntenna == null) || (DeployableAntenna.deployState == ModuleDeployablePart.DeployState.EXTENDED));

        public override void OnStart(StartState state)
        {
            base.OnStart(state);
            Core.Log("Start() in part " + part.name);
            resourceId = PartResourceLibrary.Instance.GetDefinition("ElectricCharge").id;
            lastUpdated = Planetarium.GetUniversalTime();
        }

        double ConsumptionRate => ecRate * (vessel.Connection.IsConnected ? (1 - vessel.Connection.SignalStrength * (1 - SignalDelaySettings.ECBonus)) : 1);

        public void FixedUpdate()
        {
            double time = Planetarium.GetUniversalTime();
            if (time <= lastUpdated) return;
            if (IsActive) part.RequestResource(resourceId, ConsumptionRate * (time - lastUpdated));
            lastUpdated = time;
        }

        public override string GetInfo() => "Telemetry EC Usage: up to " + ecRate + "/s";
    }
}
