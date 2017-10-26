# SignalDelay

## Features

This mod adds signal delay for probes in KSP, based on their CommNet connectivity. It means that you will only see the commands you issue after the radio wave reaches the vessel and returns back. Controlling interplanetary probes gets harder but more realistic.

Antennas also now use Electric Charge (when deployed) for telemetry. When signal strength is high, they use less power (down to 50%).

All the features can be enabled, disabled or adjusted through in-game Settings.

## Status

The mod is in beta. Please report bugs via the Issues tab or at [the forum](https://forum.kerbalspaceprogram.com/index.php?/topic/166584-13x-signal-delay-011-2017-10-24/) and attach the output log.

## Required & Supported Mods

- [Module Manageer](https://github.com/sarbian/ModuleManager) - required for EC usage feature
- [kOS](https://github.com/KSP-KOS/KOS) - terminal is supported, but all the commands are implemented without delay
- [DMagic Orbital Science](https://github.com/DMagic1/Orbital-Science)
- [JX2Antenna](https://github.com/KSPSnark/JX2Antenna)
- [USI Kolonization Systems (MKS)](https://github.com/UmbraSpaceIndustries/MKS)
- [Surface Experiment Pack (SEP)](https://github.com/CobaltWolf/Surface-Experiment-Pack)

## Known Issues

- The mod doesn't handle part actions (e.g. experiment deployment), so they are not delayed. If you find it cheaty, you can choose to hide them, in the Settings, and use action groups instead.
- RCS linear translation (HNJLIK keys) is not supported at the moment. I haven't figured out how to execute these actions.
