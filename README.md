# SoundCenSe

Based on ZweiStein's SoundSense, this is a c# port to get rid of installing java on windows systems.

Using the [fmod audio](http://fmod.org "fmod audio") libraries by Firelight Technologies there is now full volume control for all sound channels.

*System requirements:*

- Mono or .NET 4.0 (depending on your OS)
- a sound card
- Dwarf Fortress
- Fun


On windows you have to install the gtk-sharp libraries ([http://www.mono-project.com/download/#download-win](http://www.mono-project.com/download/#download-win)).
On linux or OSX apparently the mono framework (successfully tested on a debian with mono version 3.28) and the gtk libraries are needed.

Memory footprint is around 30-45 MB, cpu load around 1.5% on a 4-core AMD phenom II X4 @ 3.2GHz.



Credits:

- Toady One and ThreeToe for Dwarf Fortress
- Firelight Techonologies for fmod audio libraries
- ZweiStein for his good work on SoundSense
- jecowa for his efforts on OSX testing

