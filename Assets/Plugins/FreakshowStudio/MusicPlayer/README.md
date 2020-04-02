Music Player
============


© 2015 Freakshow Studio AS
All rights reserved


Introduction
------------

*Music Player* is a scripting plugin for Unity that allows for easy
playing of music in your projects.

It lets you configure playlists, for example for different parts of your game
or different levels, and easily change between these at appropriate times.
For example by having one playlist for the main menu, and a different one
in game.

It is also possible to create playlists at runtime, if your audio clips are
not available when you are editing or you are downloading the music at runtime.

Playlists can be played in order or shuffled, and tracks can be disabled.
This can let users choose which tracks they want to hear.


Usage
-----

First, a Music Player should be added to the scene. This is done with the
menu option GameObject --> Audio --> Music Player. This will create a
GameObject named *MusicPlayer* in the scene, with an *AudioSource*
component and a *MusicPlayer* component.

It is possible to have several music players in a single scene, for example
if you want to use them as positional (3D) audio sources. The most common
scenario would however be to have just one, and then route the output of the
*AudioSource* component to an *Audio Mixer*.

Once you have created a *Music Player*, you can set up playlists and tracks
in its inspector. Set the *Play on Awake* option if you want it to
automatically play the first playlist on start.

Once you have set up the playlists as you wish, use the public methods on
the *Music Player* to control playback. These are *Play*, *Stop*, *Pause*,
*UnPause*, *Next* and *Previous*.

For further information, see the included User Guide and API Reference
manuals.


Support
-------

For support, please contact <support@freakshowstudio.com>
