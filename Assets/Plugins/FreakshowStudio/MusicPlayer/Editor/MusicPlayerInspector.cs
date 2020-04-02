/*! 
 * \file
 * \author Stig Olavsen <stig.olavsen@freakshowstudio.com>
 * \author http://www.freakshowstudio.com
 * \date © 2015
 */

#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using FreakLib.Music;

namespace FreakLibEditor.Music
{
	/// <summary>
	/// This class implements the inspector for the MusicPlayer.
	/// </summary>
	[CustomEditor(typeof(MusicPlayer))]
	public class MusicPlayerInspector : Editor
	{
		/// <summary>
		/// Draws the GUI for the MusicPlayer inspector.
		/// </summary>
		public override void OnInspectorGUI()
		{
			MusicPlayer player = target as MusicPlayer;
			Undo.RecordObject(player, "Edit playlist");

			EditorGUILayout.Separator();
			SettingsGUI(player);
			EditorGUILayout.Separator();
			PlaylistArrayGUI(player.Playlists);
			EditorGUILayout.Separator();
			AddPlaylistButton(player.Playlists);
			EditorGUILayout.Separator();
			EventsGUI();

			if (GUI.changed)
			{
				EditorUtility.SetDirty(target);
			}
		}

		/// <summary>
		/// Draws the GUI for the settings.
		/// </summary>
		/// <param name="player">The MusicPlayer to draw the 
		/// settings for.</param>
		private void SettingsGUI(MusicPlayer player)
		{
			player.PlayOnAwake = EditorGUILayout.Toggle(
				"Play On Awake", 
				player.PlayOnAwake);

			player.Volume = EditorGUILayout.Slider(
				"Volume",
				player.Volume, 0f, 1f);
			player.Volume = Mathf.Clamp01(player.Volume);

			player.HistoryLength = EditorGUILayout.IntField(
				"History Length",
				player.HistoryLength);
			player.HistoryLength = Mathf.Max(player.HistoryLength, 0);

			if (GUI.changed)
			{
				EditorUtility.SetDirty(target);
			}
		}

		/// <summary>
		/// Draws the GUI for the playlists.
		/// </summary>
		/// <param name="playlists">The playlists.</param>
		private void PlaylistArrayGUI(List<Playlist> playlists)
		{
			if (playlists == null || playlists.Count == 0)
			{
				return;
			}

			List<Playlist> markedForDeletion = new List<Playlist>();
			List<Playlist> moveListUp = new List<Playlist>();
			List<Playlist> moveListDown = new List<Playlist>();

			foreach (Playlist p in playlists)
			{
				bool deleteThis = false;
				bool moveUp = false;
				bool moveDown = false;
				PlaylistGUI(p, ref deleteThis, ref moveUp, ref moveDown);
				if (deleteThis)
				{
					markedForDeletion.Add(p);
				}
				if (moveUp)
				{
					moveListUp.Add(p);
				}
				if (moveDown)
				{
					moveListDown.Add(p);
				}
				EditorGUILayout.Separator();
			}

			foreach (Playlist p in markedForDeletion)
			{
				playlists.Remove(p);
			}

			foreach (Playlist p in moveListUp)
			{
				int i = playlists.IndexOf(p);
				if (i <= 0)
				{
					continue;
				}
				Playlist tp = playlists[i-1];
				playlists[i-1] = p;
				playlists[i] = tp;
			}

			foreach (Playlist p in moveListDown)
			{
				int i = playlists.IndexOf(p);
				if (i >= playlists.Count - 1)
				{
					continue;
				}
				Playlist tp = playlists[i+1];
				playlists[i+1] = p;
				playlists[i] = tp;
			}
		}

		/// <summary>
		/// Draws a button to add a new playlist.
		/// </summary>
		/// <param name="playlists">The playlist-list to add to.</param>
		private void AddPlaylistButton(List<Playlist> playlists)
		{
			bool addPlaylist = GUILayout.Button("Add new playlist");
			if (addPlaylist)
			{
				playlists.Add(new Playlist());
			}
		}

		/// <summary>
		/// Draws the GUI for a single Playlist.
		/// </summary>
		/// <param name="p">The Playlist.</param>
		/// <param name="delete">If this Playlist should be deleted.</param>
		/// <param name="moveUp">If this Playlist should be moved up.</param>
		/// <param name="moveDown">If this Playlist should be moved down.
		/// </param>
		private void PlaylistGUI(Playlist p, 
		                         ref bool delete, 
		                         ref bool moveUp, 
		                         ref bool moveDown)
		{
			GUILayout.BeginHorizontal();
			p.foldout = EditorGUILayout.Foldout(
				p.foldout, "Playlist: " + p.name);
			moveUp = GUILayout.Button("Up", GUILayout.Width(30f));
			moveDown = GUILayout.Button("Dn", GUILayout.Width(30f));
			delete = GUILayout.Button("-", GUILayout.Width(20f));
			GUILayout.EndHorizontal();

			if (p.foldout)
			{
				EditorGUI.indentLevel++;

				GUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Name: ", GUILayout.Width(75f));
				p.name = EditorGUILayout.TextField(p.name);
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Shuffle: ", GUILayout.Width(75f));
				p.shuffle = EditorGUILayout.Toggle(p.shuffle);
				GUILayout.EndHorizontal();

				List<Track> deleteTracks = new List<Track>();
				List<Track> moveUpTracks = new List<Track>();
				List<Track> moveDownTracks = new List<Track>();

				foreach (Track t in p.tracks)
				{
					GUILayout.BeginHorizontal();
					t.foldout = EditorGUILayout.Foldout(
						t.foldout, "Track: " + t.ToString());

					bool tu = GUILayout.Button("Up", GUILayout.Width(30f));
					bool td = GUILayout.Button("Dn", GUILayout.Width(30f));
					bool dt = GUILayout.Button("-", GUILayout.Width(20f));

					if (dt)
					{
						deleteTracks.Add(t);
					}
					if (tu)
					{
						moveUpTracks.Add(t);
					}
					if (td)
					{
						moveDownTracks.Add(t);
					}
					GUILayout.EndHorizontal();

					if (t.foldout)
					{
						EditorGUI.indentLevel++;
						TrackGUI(t);
						EditorGUI.indentLevel--;
					}
				}

				foreach (Track t in deleteTracks)
				{
					p.tracks.Remove(t);
				}

				foreach (Track t in moveUpTracks)
				{
					int i = p.tracks.IndexOf(t);
					if (i <= 0)
					{
						continue;
					}
					Track tt = p.tracks[i-1];
					p.tracks[i-1] = t;
					p.tracks[i] = tt;
				}

				foreach (Track t in moveDownTracks)
				{
					int i = p.tracks.IndexOf(t);
					if (i >= p.tracks.Count - 1)
					{
						continue;
					}
					Track tt = p.tracks[i+1];
					p.tracks[i+1] = t;
					p.tracks[i] = tt;
				}

				bool addTrack = GUILayout.Button("Add new track");
				if (addTrack)
				{
					p.tracks.Add(new Track());
				}

				EditorGUI.indentLevel--;
			}
		}

		/// <summary>
		/// Draws the GUI for a single Track.
		/// </summary>
		/// <param name="t">The Track to draw.</param>
		private void TrackGUI(Track t)
		{
			GUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Enabled:", GUILayout.Width(100f));
			t.enabled = EditorGUILayout.Toggle(t.enabled);
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Name:", GUILayout.Width(100f));
			t.name = EditorGUILayout.TextField(t.name);
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("AudioClip:", GUILayout.Width(100f));
 			t.clip = (AudioClip) EditorGUILayout.ObjectField(t.clip, typeof(AudioClip), false);
			GUILayout.EndHorizontal();
		}

		/// <summary>
		/// Draws the GUI for the events.
		/// </summary>
		private void EventsGUI()
		{
			EditorGUILayout.LabelField("Events");

			SerializedProperty onPlay = 
				serializedObject.FindProperty("OnPlay");
			SerializedProperty onStop = 
				serializedObject.FindProperty("OnStop");
			SerializedProperty onPause =
				serializedObject.FindProperty("OnPause");
			SerializedProperty onUnpause =
				serializedObject.FindProperty("OnUnpause");
			SerializedProperty onTrackChange = 
				serializedObject.FindProperty("OnTrackChange");
			SerializedProperty onPlaylistChange = 
				serializedObject.FindProperty("OnPlaylistChange");

			EditorGUILayout.PropertyField(onPlay);
			EditorGUILayout.PropertyField(onStop);
			EditorGUILayout.PropertyField(onPause);
			EditorGUILayout.PropertyField(onUnpause);
			EditorGUILayout.PropertyField(onTrackChange);
			EditorGUILayout.PropertyField(onPlaylistChange);
			serializedObject.ApplyModifiedProperties();
			serializedObject.UpdateIfDirtyOrScript();
		}
	}
}
#endif // UNITY_EDITOR
