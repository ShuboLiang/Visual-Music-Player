using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using FreakLib.Music;

public class PlaylistList : MonoBehaviour 
{
	public MusicPlayer musicPlayer;
	public GameObject playlist;

	void Start()
	{
		List<Playlist> playlists = musicPlayer.Playlists;
		float pos = 0f;
		foreach (Playlist p in playlists)
		{
			GameObject po = Instantiate<GameObject>(playlist);
			Text pt = po.GetComponentInChildren<Text>();
			pt.text = p.name;
			Button pb = po.GetComponentInChildren<Button>();
			Playlist p2 = p;
			pb.onClick.AddListener(delegate {
				musicPlayer.Play(p2);	 
			});
			po.transform.SetParent(transform, false);
			po.transform.localPosition = new Vector3(0f, pos, 0f);
			pos -= (po.transform as RectTransform).rect.height;
		}
		(transform as RectTransform).
			SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, -pos);
	}
}
