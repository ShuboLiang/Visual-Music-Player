/*! 
 * \file
 * \author Stig Olavsen <stig.olavsen@freakshowstudio.com>
 * \author http://www.freakshowstudio.com
 * \date © 2015
 */

#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;
using FreakLib.Music;

namespace FreakLibEditor.Music
{
	/// <summary>
	/// This class adds a menu item to create a new MusicPlayer in the
	/// scene.
	/// </summary>
	public class MusicPlayerMenuItem : Editor
	{
		/// <summary>
		/// Create a MusicPlayer in the scene and set up the AudioSource.
		/// </summary>
		[MenuItem("GameObject/Audio/Music Player", false, 10000)]
		private static void CreateMusicPlayer()
		{
			GameObject g = new GameObject("MusicPlayer");
			AudioSource a = g.AddComponent<AudioSource>();
			a.loop = false;
			a.bypassEffects = true;
			a.bypassReverbZones = true;
			a.dopplerLevel = 0f;
			a.playOnAwake = false;
			g.AddComponent<MusicPlayer>();
		}
	}
}
#endif // UNITY_EDITOR
