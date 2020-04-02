/*! 
 * \file
 * \author Stig Olavsen <stig.olavsen@freakshowstudio.com>
 * \author http://www.freakshowstudio.com
 * \date © 2015
 */

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace FreakLib.Music
{
    /// <summary>
    /// This class implements a Playlist for the MusicPlayer.
	/// 
	/// A Playlist is a group of <see cref="FreakLib.Music.Track"/>s
	/// that the MusicPlayer can play in order, or randomly shuffled.
    /// </summary>
    [Serializable]
    public class Playlist
    {
        #region Inspector variables
        /// <summary>
        /// The name of the Playlist.
        /// </summary>
        public string name = "";
        /// <summary>
		/// The list of <see cref="FreakLib.Music.Track"/>s in this Playlist.
        /// </summary>
        public List<Track> tracks = new List<Track>();
        /// <summary>
        /// Should the Playlist be shuffeled during play?
        /// </summary>
        public bool shuffle = false;
		/// <summary>
		/// If this Playlist should be expanded in the inspector.
		/// </summary>
		public bool foldout = false;
        #endregion // Inspector variables

		#region Properties
		/// <summary>
		/// Returns a list of indices to the tracks in this Playlist
		/// that are currently enabled.
		/// </summary>
		/// <value>The enabled track indices.</value>
		public List<int> EnabledTrackIndices
		{
			get
			{
				List<int> indices = new List<int>(tracks.Count);
				for (int i = 0; i < tracks.Count; ++i)
				{
					if (tracks[i].enabled && tracks[i].clip != null)
					{
						indices.Add(i);
					}
				}
				return indices;
			}
		}
		#endregion // Properties

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the 
		/// <see cref="FreakLib.Jukebox.Playlist"/> class.
        /// </summary>
        public Playlist()
        {
			name = "New Playlist";
        }

		/// <summary>
		/// Initializes a new instance of the 
		/// <see cref="FreakLib.Music.Playlist"/> class.
		/// </summary>
		/// <param name="aName">The Playlist name.</param>
        public Playlist(string aName)
        {
			name = aName;
        }

		/// <summary>
		/// Initializes a new instance of the 
		/// <see cref="FreakLib.Music.Playlist"/> class.
		/// </summary>
		/// <param name="aName">The Playlist name.</param>
		/// <param name="theTracks">A list of tracks for this Playlist.</param>
		/// <param name="shouldShuffle">If set to <c>true</c> 
		/// the Playlist will shuffle.</param>
        public Playlist(string aName, 
		                List<Track> theTracks, 
		                bool shouldShuffle = false)
        {
			name = aName;
			tracks = theTracks;
			shuffle = shouldShuffle;
        }
        #endregion Constructors
    }
}
