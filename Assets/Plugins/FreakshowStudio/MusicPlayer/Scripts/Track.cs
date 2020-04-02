/*! 
 * \file
 * \author Stig Olavsen <stig.olavsen@freakshowstudio.com>
 * \author http://www.freakshowstudio.com
 * \date © 2015
 */

using UnityEngine;
using System;
using System.Collections;

namespace FreakLib.Music
{
    /// <summary>
    /// This class implements a single Track for the MusicPlayer. 
	/// Tracks are added to a Playlist that the MusicPlayer can then
	/// play.
    /// </summary>
    [Serializable]
    public class Track
    {
        #region Inspector variables
        /// <summary>
        /// The Track name.
        /// </summary>
		public string name = "";
        /// <summary>
        /// The AudioClip for the Track.
        /// </summary>
        public AudioClip clip;
		/// <summary>
		/// If the Track is enabled. When disabled, the track will
		/// not be played.
		/// </summary>
		public bool enabled = true;
		/// <summary>
		/// If this Track should be expanded in the inspector.
		/// </summary>
		public bool foldout = false;
		#endregion // Inspector variables

		#region Properties
		/// <summary>
		/// The Track length, in seconds.
		/// </summary>
		/// <value>Track length.</value>
		public float Length
		{
			get 
			{
				if (clip == null)
				{
					return 0f;
				}
				return clip.length;
			}
		}

		/// <summary>
		/// The number of times this track has been played.
		/// 
		/// This will be reset by the MusicPlayer whenever a new
		/// Playlist is started.
		/// </summary>
		/// <value>The number of plays.</value>
		public int Plays
		{
			get; 
			set;
		}
		#endregion // Properties

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the 
		/// <see cref="FreakLib.Music.Track"/> class.
        /// </summary>
        public Track()
        {
			name = "New Track";
        }

		/// <summary>
		/// Initializes a new instance of the 
		/// <see cref="FreakLib.Music.Track"/> class.
		/// </summary>
		/// <param name="aClip">The AudioClip for this Track.</param>
		/// <param name="aName">The name of this Track.</param>
        public Track(AudioClip aClip, string aName = "")
        {
            clip = aClip;
            name = aName;
        }
        #endregion // Constructors

        #region Overrides
        /// <summary>
        /// Returns a <see cref="System.String"/> that represents 
		/// the current <see cref="FreakLib.Music.Track"/>.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents 
		/// the current <see cref="FreakLib.Music.Track"/>.</returns>
        public override string ToString()
        {
			return name;
        }
        #endregion // Overrides
    }
}
