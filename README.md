# SoundCenSe v1.4.4 #

SoundCenSe is a audio engine for Dwarf Fortress based on ZweiStein's SoundSense written in c#/gtk.

**System requirements:**

Windows:

- .NET 4.0 ([https://download.microsoft.com/download/9/5/A/95A9616B-7A37-4AF6-BC36-D6EA96C8DAAE/dotNetFx40_Full_x86_x64.exe](https://download.microsoft.com/download/9/5/A/95A9616B-7A37-4AF6-BC36-D6EA96C8DAAE/dotNetFx40_Full_x86_x64.exe))
- GTK-Sharp ([http://download.xamarin.com/GTKforWindows/Windows/gtk-sharp-2.12.38.msi](http://download.xamarin.com/GTKforWindows/Windows/gtk-sharp-2.12.38.msi))
- Microsoft Visual c++ runtime redistributables 2013 ([https://download.microsoft.com/download/2/E/6/2E61CFA4-993B-4DD4-91DA-3737CD5CD6E3/vcredist_x86.exe](https://download.microsoft.com/download/2/E/6/2E61CFA4-993B-4DD4-91DA-3737CD5CD6E3/vcredist_x86.exe))

If it doesn't seem to start, please check the Log.txt, there might be clues to what is missing.


Linux:

- mono-complete
- gtk-sharp2

Don't know for sure for OSX, but mono and gtk-sharp should do the trick.


**Usage:**

Just start the SoundCenSeGTK.exe (linux users without binutils fix need to use 'mono SoundCenSe.exe').

## First start: ##

Since no sound files are provided in this download, you need to update the sound packs first or point the Soundpack Path to the appropriate folder.
More on that below

## The audio tab ##
On the audio tab you see the different channels (SFX, music, weather, swords and trading).
On the right side of each of the channels you have a volume and a mute control. Changes are save on closing SoundCenSe.

On the bottom of each channel you can show/hide the last 5 played sounds, which you can disable with the little button on the left side. Once you disabled a sound, it will not be played anymore until you remove it again from the *Disabled Sounds* tab, by again clicking the little button on the left side.

## The update tab ##
By clicking *Update Soundpack* the chosen soundpack folder will be checked and updated accordingly.
Please wait for the update to finish until you start Dwarf Fortress.

## The configuration tab ##
First, there is the path to the soundpack folder. Click on the ... button to change it.

Windows users have an additional checkbox for auto-detection of dwarf fortress or static path to the gamelog.
If you choose to use a static path, you can click the ... button to change the folder.

## The credits tab ##
Especially pay attention to this tab, since without these guys, there wouldn't be a SoundCenSe at all.



Credits:

- Toady One and ThreeToe for Dwarf Fortress
- Firelight Techonologies for fmod audio libraries
- ZweiStein for his good work on SoundSense
- jecowa for his efforts on OSX testing


Last changed: 9/7/2017 10:20:32 AM 
