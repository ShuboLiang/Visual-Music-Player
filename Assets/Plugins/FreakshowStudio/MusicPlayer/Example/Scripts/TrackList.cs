using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using FreakLib.Music;

public class TrackList : MonoBehaviour 
{
	public Text title;
	public GameObject track;
	public Toggle shuffle;

	public void PlaylistChange(MusicPlayer player)
	{
		Playlist currentPlaylist = player.CurrentPlaylist;
		title.text = currentPlaylist.name;

		shuffle.isOn = player.CurrentPlaylist.shuffle;
		shuffle.onValueChanged.AddListener(delegate(bool on) {
			currentPlaylist.shuffle = on;
		});

		List<Track> tracks = currentPlaylist.tracks;
		float pos = 0f;
		foreach (Track t in tracks)
		{
			GameObject to = Instantiate<GameObject>(track);
			Text tt = to.GetComponentInChildren<Text>();
			tt.text = t.name;
			Toggle tg = to.GetComponentInChildren<Toggle>();
			tg.isOn = t.enabled;
			Track t2 = t;
			tg.onValueChanged.AddListener(delegate(bool on) {
				t2.enabled = on;
			});
			to.transform.SetParent(transform, false);
			to.transform.localPosition = new Vector3(0f, pos, 0f);
			player.OnPlaylistChange.AddListener(delegate {
				Destroy(to);
			});
			pos -= (to.transform as RectTransform).rect.height;
		}
		(transform as RectTransform).
			SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, -pos);
	}
}
