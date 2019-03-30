# SignalDelay

## Features

This mod adds signal delay for probes in KSP, based on their CommNet connectivity. It means that you will only see the commands you issue after the radio wave reaches the vessel and returns back. Controlling interplanetary probes gets harder but more realistic.

Antennas also now use Electric Charge (when deployed) for telemetry. The lower the signal strength (only the first link counts), the higher EC usage.

All the features can be adjusted via in-game Settings. Quickly disable or enable the mod by clicking the toolbar button.

## Status

The mod is in beta. Please report bugs via the GitHub [Issues tab](https://github.com/GarwelGarwel/SignalDelay/issues) or at [the forum](https://forum.kerbalspaceprogram.com/index.php?/topic/166584-13x-signal-delay-011-2017-10-24/) and attach the output log.

## Supported Mods

- **[Module Manager](https://github.com/sarbian/ModuleManager) - required for EC usage feature**
- [Blizzy's Toolbar](https://forum.kerbalspaceprogram.com/index.php?/topic/161857-13-*)
- [Bluedog Design Bureau](https://forum.kerbalspaceprogram.com/index.php?/topic/122020-16x-bluedog-design-bureau-stockalike-saturn-apollo-and-more-v152-бруно-8feb2019)
- [CommNet Antennas Extension](https://forum.kerbalspaceprogram.com/index.php?/topic/177292-16-commnet-antennas-extension-commnet-antennas-info-2018-12-29/)
- [DMagic Orbital Science](https://github.com/DMagic1/Orbital-Science)
- [Global Construction](https://forum.kerbalspaceprogram.com/index.php?/topic/154167-145-global-construction/)
- [JX2Antenna](https://github.com/KSPSnark/JX2Antenna)
- [kOS](https://github.com/KSP-KOS/KOS) - terminal is supported, but all the commands are implemented without delay. You may use WAIT command to simulate delay.
- [Pathfinder (WBI)](https://forum.kerbalspaceprogram.com/index.php?/topic/121397-16x-pathfinder-space-camping-geoscience/)
- [RemoteTech Redev Antennas](https://forum.kerbalspaceprogram.com/index.php?/topic/139167-144-remotetech-v1812-2018-07-17/)
- [ReStock+](https://forum.kerbalspaceprogram.com/index.php?/topic/182679-161-restock-revamping-ksps-art/)
- [Surface Experiment Pack (SEP)](https://github.com/CobaltWolf/Surface-Experiment-Pack)
- [Tantares](https://forum.kerbalspaceprogram.com/index.php?/topic/73686-16x-tantares-stockalike-soyuz-and-mir-17018032019polyus/)
- [USI Kolonization Systems (MKS)](https://github.com/UmbraSpaceIndustries/MKS)

## Known Issues

- The mod doesn't handle part actions (e.g. experiment deployment), so they are not delayed. If you find it cheaty, you can choose to hide them, in the Settings, and use action groups instead.
- RCS linear translation (HNJLIK keys) is not supported at the moment. I haven't figured out how to execute these actions.
- Autopilot and similar mods (such as MechJeb) also have no delay, because they interact with the vessel directly.
