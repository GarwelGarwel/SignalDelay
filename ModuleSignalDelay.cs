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
        static int resourceId;

        ModuleDeployableAntenna deployableAntenna;

        bool IsActive => SignalDelaySettings.Instance.ECUsage && ((deployableAntenna == null) || (deployableAntenna.deployState == ModuleDeployablePart.DeployState.EXTENDED));

        double ConsumptionRate
            => ecRate * (vessel.Connection.IsConnected ? (1 - vessel.Connection.ControlPath.First.signalStrength * (1 - SignalDelaySettings.Instance.ECBonus)) : 1);

        public List<PartResourceDefinition> GetConsumedResources() => new List<PartResourceDefinition>() { PartResourceLibrary.Instance.GetDefinition("ElectricCharge") };

        // Kerbalism compatibility method
        public string PlannerUpdate(List<KeyValuePair<string, double>> resources, CelestialBody body, Dictionary<string, double> environment)
        {
            if (IsActive)
                resources.Add(new KeyValuePair<string, double>("ElectricCharge", -ecRate));
            return "antenna";
        }

        // Kerbalism compatibility method
        public static string BackgroundUpdate(Vessel v, ProtoPartSnapshot part_snapshot, ProtoPartModuleSnapshot module_snapshot, PartModule proto_part_module, Part proto_part, Dictionary<string, double> availableResources, List<KeyValuePair<string, double>> resourceChangeRequest, double elapsed_s)
        {
            ModuleSignalDelay module = proto_part_module as ModuleSignalDelay;
            if (module.IsActive)
            {
                availableResources.TryGetValue("ElectricCharge", out double ec);
                module.actualECRate = (float)module.ConsumptionRate;
                resourceChangeRequest.Add(new KeyValuePair<string, double>("ElectricCharge", -module.actualECRate));
                Core.Log(v.vesselName + " " + part_snapshot.partName + ": consuming " + module.ConsumptionRate + " EC/sec in background (" + ec + " EC available)");
            }
            else module.actualECRate = 0;
            module.lastUpdated = Planetarium.GetUniversalTime();
            return "antenna";
        }

        public override void OnStart(StartState state)
        {
            base.OnStart(state);
            Core.Log("OnStart(" + state + ") in part " + part.name);
            resourceId = PartResourceLibrary.Instance.GetDefinition("ElectricCharge").id;
            deployableAntenna = part.FindModuleImplementing<ModuleDeployableAntenna>();
            lastUpdated = Planetarium.GetUniversalTime();
        }

        public void FixedUpdate()
        {
            double time = Planetarium.GetUniversalTime();
            if (time <= lastUpdated)
                return;
            if (IsActive)
            {
                actualECRate = (float)ConsumptionRate;
                part.RequestResource(resourceId, actualECRate * (time - lastUpdated));
            }
            else actualECRate = 0;
            lastUpdated = time;
        }

        public override string GetInfo() => "Background EC Usage: up to " + ecRate + "/sec";
    }
}
