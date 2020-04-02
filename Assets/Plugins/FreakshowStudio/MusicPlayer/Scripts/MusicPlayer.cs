/*! 
 * \file
 * \author Stig Olavsen <stig.olavsen@freakshowstudio.com>
 * \author http://www.freakshowstudio.com
 * \date © 2015
 */

using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;

namespace FreakLib.Music
{
    /// <summary>
    /// This class implements a MusicPlayer for Unity.
	/// 
	/// The MusicPlayer will play a Playlist, either in order or
	/// shuffled, and allows pausing, skipping tracks forward and back, 
	/// as well as seeking.
	/// 
	/// It will also send various UnityEvents when changing tracks, 
	/// playlists and so on.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class MusicPlayer : MonoBehaviour
    {
		#region Event Definitions
		/// <summary>
		/// A custom UnityEvent, which allows passing a MusicPlayer
		/// as an argument.
		/// </summary>
		[Serializable] 
		public class MusicPlayerEvent : UnityEvent<MusicPlayer>
		{}
		#endregion // Event Definitions

		#region Inspector variables
        /// <summary>
		/// The <see cref="FreakLib.Music.Playlist"/>s associated with
		/// this MusicPlayer. 
		/// 
		/// The playlists added here can be played with a string reference
		/// to their name, but the MusicPlayer will also play other 
		/// playlists as long as they are passed with a reference to the
		/// playlist object.
        /// </summary>
        [SerializeField] private List<Playlist> m_Playlists =
			new List<Playlist>();
		/// <summary>
		/// If the MusicPlayer should start playing the first Playlist
		/// on Awake().
		/// </summary>
		[SerializeField] private bool m_PlayOnAwake = false;
		/// <summary>
		/// The volume.
		/// 
		/// This controls the volume of the attached AudioSource directly.
		/// </summary>
		[SerializeField] private float m_Volume = 1f;
		/// <summary>
		/// The length of the history. This is the number of tracks you
		/// can skip backwards.
		/// </summary>
		[SerializeField] private int m_HistoryLength = 25;
		#endregion // Inspector variables

		#region Unity Events
		/// <summary>
        /// A UnityEvent that will fire when we start playing.
        /// </summary>
		public MusicPlayerEvent OnPlay;
        /// <summary>
		/// A UnityEvent that will fire when we stop playing.
        /// </summary>
		public MusicPlayerEvent OnStop;
		/// <summary>
		/// A UnityEvent that will fire when we pause.
		/// </summary>
		public MusicPlayerEvent OnPause;
		/// <summary>
		/// A UnityEvent that will fire when we un-pause.
		/// </summary>
		public MusicPlayerEvent OnUnpause;
        /// <summary>
		/// A UnityEvent that will fire when we change track.
        /// </summary>
		public MusicPlayerEvent OnTrackChange;
        /// <summary>
		/// A UnityEvent that will fire when we change Playlist.
        /// </summary>
		public MusicPlayerEvent OnPlaylistChange;
		#endregion // Unity Events

        #region Private variables
        /// <summary>
        /// The connected AudioSource that is responsible for 
		/// the actual playing.
        /// </summary>
        private AudioSource m_AudioSource;
        /// <summary>
        /// The current playlist.
        /// </summary>
        private Playlist m_CurrentPlaylist;
		/// <summary>
		/// The index of the current track.
		/// </summary>
		private int m_CurrentTrackIdx = 0;
		/// <summary>
		/// If we are currently playing.
		/// </summary>
		private bool m_IsPlaying = false;
		/// <summary>
		/// If we are currently paused.
		/// </summary>
		private bool m_IsPaused = false;
		/// <summary>
		/// The track history.
		/// </summary>
        private LinkedList<int> m_History = new LinkedList<int>();
        #endregion // Private variables

        #region Properties
		/// <summary>
		/// Gets or sets a value indicating whether this 
		/// MusicPlayer should play on awake.
		/// </summary>
		/// <value><c>true</c> if play on awake; otherwise, <c>false</c>.
		/// </value>
		public bool PlayOnAwake
		{
			get { return m_PlayOnAwake; }
			set { m_PlayOnAwake = value; }
		}

		/// <summary>
		/// Gets or sets the volume.
		/// </summary>
		/// <value>The volume.</value>
		public float Volume
		{
			get { return m_Volume; }
			set { m_Volume = value; }
		}

		/// <summary>
		/// Gets or sets the length of the history.
		/// </summary>
		/// <value>The length of the history.</value>
		public int HistoryLength
		{
			get { return m_HistoryLength; }
			set { m_HistoryLength = value; }
		}

        /// <summary>
        /// Gets the playlists.
        /// </summary>
        /// <value>The playlists.</value>
        public List<Playlist> Playlists
        { 
            get { return m_Playlists; }
			set { m_Playlists = value; }
        }

        /// <summary>
        /// Gets the playlist names.
        /// </summary>
        /// <value>The playlist names.</value>
        public List<string> PlaylistNames
        { 
            get
            { 
				List<string> names = 
					m_Playlists.ConvertAll<string>(p => p.name);
                return names;
            }
        }

        /// <summary>
        /// Gets the current playlist.
        /// </summary>
        /// <value>The current playlist.</value>
        public Playlist CurrentPlaylist
        { 
            get { return m_CurrentPlaylist; }
        }

        /// <summary>
        /// Gets the name of the current playlist.
        /// </summary>
        /// <value>The name of the current playlist.</value>
        public string CurrentPlaylistName
        {
            get { return m_CurrentPlaylist.name; }
        }

        /// <summary>
        /// Gets the current track.
        /// </summary>
        /// <value>The current track.</value>
        public Track CurrentTrack
        { 
            get 
			{ 
				if (CurrentPlaylist == null) 
				{
					return null;
				}
				return CurrentPlaylist.tracks[m_CurrentTrackIdx]; 
			}
        }

        /// <summary>
        /// Gets the name of the current track.
        /// </summary>
        /// <value>The name of the current track.</value>
        public string CurrentTrackName
        {
            get { return CurrentTrack.ToString(); }
        }

		/// <summary>
		/// Gets a value indicating if we are currently playing.
		/// </summary>
		/// <value><c>true</c> if playing; otherwise, <c>false</c>.</value>
		public bool IsPlaying
		{
			get { return m_IsPlaying; }
		}

		/// <summary>
		/// Gets the current playtime, in seconds. Will be a value
		/// between 0 and the track length.
		/// </summary>
		/// <value>The playtime.</value>
		public float Playtime
		{
			get { return m_AudioSource.time; }
			set { m_AudioSource.time = value; }
		}

		/// <summary>
		/// Gets the current playtime normalized. Will be a value
		/// between 0 and 1.
		/// </summary>
		/// <value>The playtime normalized.</value>
		public float PlaytimeNormalized
		{
			get
			{
				float total = CurrentTrack.Length;
				if (total == 0f)
				{
					return 0f;
				}
				float current = Playtime;
				return current / total;
			}
			set
			{
				if (m_AudioSource == null || CurrentTrack == null)
				{
					return;
				}
				m_AudioSource.time = value * CurrentTrack.Length;
			}
		}
        #endregion // Properties

        #region Unity methods
        /// <summary>
        /// Unity Awake method.
        /// </summary>
        void Awake()
        {
			m_History = new LinkedList<int>();

            m_AudioSource = GetComponent<AudioSource>();
			m_AudioSource.volume = m_Volume;

			if (m_PlayOnAwake && m_Playlists != null && m_Playlists.Count > 0)
			{
				Play();
			}
        }

		/// <summary>
		/// Unity Update method.
		/// </summary>
		void Update()
		{
			m_AudioSource.volume = m_Volume;

			if (m_IsPlaying && !m_IsPaused && !m_AudioSource.isPlaying)
			{
				Next();
			}
		}
        #endregion // Unity methods

		#region Public methods
		/// <summary>
		/// Starts playing the current Playlist, or the first defined
		/// Playlist if the current Playlist is not set.
		/// 
		/// Calling Play will always start playing, regardless of the
		/// current state of the MusicPlayer. It will also reset the 
		/// Playlist.
		/// </summary>
		public void Play()
		{
			if (m_Playlists == null || m_Playlists.Count == 0)
			{
				return;
			}
			if (m_CurrentPlaylist != null)
			{
				Play(m_CurrentPlaylist);
			}
			else
			{
				Play(m_Playlists[0]);
			}
		}

		/// <summary>
		/// Starts playing the specified Playlist.
		/// 
		/// The name should match a Playlist defined in this instance
		/// of the MusicPlayer. If the specified playlist is not found,
		/// a UnityException is thrown.
		/// </summary>
		/// <param name="name">The name of the Playlist to play.</param>
		public void Play(string name)
		{
			Playlist pl = GetPlaylist(name);
			if (pl == null)
			{
				throw new UnityException(
					string.Format("Playlist {0} not found!", name));
			}
			Play(pl);
		}

		/// <summary>
		/// Starts playing the specified Playlist.
		/// 
		/// The specified Playlist does not necessarily need to be
		/// defined in this instance of the MusicPlayer, but can be
		/// generated and passed in as a separate instance.
		/// </summary>
		/// <param name="playlist">Playlist.</param>
		public void Play(Playlist playlist)
		{
			bool newPlaylist = (m_CurrentPlaylist != playlist);
			m_CurrentPlaylist = playlist;

			List<int> indices = m_CurrentPlaylist.EnabledTrackIndices;
			if (indices.Count == 0)
			{
				Stop();
				return;
			}

			m_History.Clear();
			for (int i = 0; i < m_CurrentPlaylist.tracks.Count; ++i)
			{
				m_CurrentPlaylist.tracks[i].Plays = 0;
			}

			if (!m_CurrentPlaylist.shuffle)
			{
				m_CurrentTrackIdx = indices[0];
			}
			else
			{
				int n = UnityEngine.Random.Range(0, indices.Count);
				m_CurrentTrackIdx = indices[n];
			}
			
			m_AudioSource.Stop();
			m_AudioSource.time = 0f;
			m_AudioSource.loop = false;
			
			m_AudioSource.clip = 
				m_CurrentPlaylist.tracks[m_CurrentTrackIdx].clip;

			m_History.AddLast(m_CurrentTrackIdx);
			while (m_History.Count > m_HistoryLength)
			{
				m_History.RemoveFirst();
			}

			m_AudioSource.Play();
			m_IsPlaying = true;
			m_IsPaused = false;

			if (newPlaylist)
			{
				OnPlaylistChange.Invoke(this);
			}
			OnPlay.Invoke(this);
			OnTrackChange.Invoke(this);
		}

		/// <summary>
		/// Calling this will Pause the MusicPlayer if it is playing,
		/// or resume from pause if it is currently paused.
		/// </summary>
		public void PauseOrResume()
		{
			if (m_IsPaused)
			{
				UnPause();
			}
			else
			{
				Pause();
			}
		}

		/// <summary>
		/// Pause the MusicPlayer.
		/// </summary>
		public void Pause()
		{
			if (!IsPlaying)
			{
				return;
			}
			m_AudioSource.Pause();
			OnPause.Invoke(this);
			m_IsPaused = true;
		}

		/// <summary>
		/// UnPause the MusicPlayer (resume from pause).
		/// </summary>
		public void UnPause()
		{
			if (!IsPlaying)
			{
				return;
			}
			if (!m_IsPaused)
			{
				return;
			}
			m_AudioSource.UnPause();
			OnUnpause.Invoke(this);
			m_IsPaused = false;
		}

		/// <summary>
		/// Stops the MusicPlayer.
		/// </summary>
		public void Stop()
		{
			if (!IsPlaying)
			{
				return;
			}
			m_AudioSource.Stop();
			m_AudioSource.time = 0f;
			OnStop.Invoke(this);
			m_IsPlaying = false;
			m_IsPaused = false;
		}

        /// <summary>
        /// Skip to the next Track.
        /// </summary>
        public void Next()
        {
			if (!IsPlaying)
			{
				return;
			}

			m_CurrentPlaylist.tracks[m_CurrentTrackIdx].Plays++;

			m_AudioSource.Stop();
			m_AudioSource.time = 0f;

			List<int> indices = m_CurrentPlaylist.EnabledTrackIndices;
			if (indices.Count == 0)
			{
				Stop();
				return;
			}

			if (indices.Count > 1)
			{
				if (!m_CurrentPlaylist.shuffle)
				{
					do
					{
						m_CurrentTrackIdx++;
						m_CurrentTrackIdx = 
							m_CurrentTrackIdx % m_CurrentPlaylist.tracks.Count;
					} while (!m_CurrentPlaylist.
					         tracks[m_CurrentTrackIdx].enabled);
				}
				else
				{
					List<Track> tracks = 
						m_CurrentPlaylist.tracks.FindAll(t => t.enabled);
					tracks.Remove(m_CurrentPlaylist.tracks[m_CurrentTrackIdx]);
					tracks.Sort((t1,t2) => UnityEngine.Random.Range(-1, 2));
					tracks.Sort((t1,t2) => t1.Plays.CompareTo(t2.Plays));
					Track nextTrack = tracks[0];
					m_CurrentTrackIdx = 
						m_CurrentPlaylist.tracks.IndexOf(nextTrack);
				}
			}

			m_AudioSource.clip = 
				m_CurrentPlaylist.tracks[m_CurrentTrackIdx].clip;

			m_History.AddLast(m_CurrentTrackIdx);
			while (m_History.Count > m_HistoryLength)
			{
				m_History.RemoveFirst();
			}

			m_AudioSource.time = 0f;
			m_AudioSource.Play();
			OnTrackChange.Invoke(this);
			m_IsPlaying = true;
			m_IsPaused = false;
        }

        /// <summary>
        /// Skip to the previous Track.
        /// </summary>
        public void Previous()
        {
			if (!IsPlaying)
			{
				return;
			}

			if (m_History.Count <= 1)
			{
				return;
			}

			m_AudioSource.Stop();
			m_AudioSource.time = 0f;

			m_History.RemoveLast();
			m_CurrentTrackIdx = m_History.Last.Value;

			m_AudioSource.clip = 
				m_CurrentPlaylist.tracks[m_CurrentTrackIdx].clip;

			m_AudioSource.time = 0f;
			m_AudioSource.Play();
			OnTrackChange.Invoke(this);
			m_IsPlaying = true;
			m_IsPaused = false;
        }

		/// <summary>
		/// Get the Playlist with the given name.
		/// 
		/// Returns null if a playlist is not found.
		/// </summary>
		/// <returns>The playlist.</returns>
		/// <param name="name">The name of the Playlist.</param>
		public Playlist GetPlaylist(string name)
		{
			Playlist pl = m_Playlists.Find(
				p => string.CompareOrdinal(p.name, name) == 0);
			return pl;
		}
        #endregion // Public methods
    }
}
