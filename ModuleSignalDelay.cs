using System.Collections.Generic;

namespace SignalDelay
{
    public class ModuleSignalDelay : PartModule, IResourceConsumer
    {
        [KSPField]
        public float ecRate = 0;

        [KSPField(guiName = "EC Usage", guiUnits = "/sec", guiActive = true, guiFormat = "F2")]
        public float actualECRate = 0;

        double lastUpdated;
        int resourceId;

        public List<PartResourceDefinition> GetConsumedResources() => new List<PartResourceDefinition>() { PartResourceLibrary.Instance.GetDefinition("ElectricCharge") };

        ModuleDeployableAntenna deployableAntenna;

        bool IsActive => SignalDelaySettings.IsECUsageEnabled && ((deployableAntenna == null) || (deployableAntenna.deployState == ModuleDeployablePart.DeployState.EXTENDED));

        public override void OnStart(StartState state)
        {
            base.OnStart(state);
            Core.Log("OnStart(" + state + ") in part " + part.name);
            resourceId = PartResourceLibrary.Instance.GetDefinition("ElectricCharge").id;
            deployableAntenna = part.FindModuleImplementing<ModuleDeployableAntenna>();
            lastUpdated = Planetarium.GetUniversalTime();
            if (HighLogic.LoadedSceneIsEditor) actualECRate = ecRate;
        }

        double ConsumptionRate => ecRate * (vessel.Connection.IsConnected ? (1 - vessel.Connection.ControlPath.First.signalStrength * (1 - SignalDelaySettings.ECBonus)) : 1);

        public void FixedUpdate()
        {
            double time = Planetarium.GetUniversalTime();
            if (time <= lastUpdated) return;
            if (IsActive)
            {
                actualECRate = (float)ConsumptionRate;
                part.RequestResource(resourceId, actualECRate * (time - lastUpdated));
            }
            else actualECRate = 0;
            lastUpdated = time;
        }

        public override string GetInfo() => "Telemetry EC Usage: up to " + ecRate + "/sec";
    }
}
