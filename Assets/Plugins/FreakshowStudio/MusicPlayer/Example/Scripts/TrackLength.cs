using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using FreakLib.Music;

[RequireComponent(typeof(Text))]
public class TrackLength : MonoBehaviour 
{
	private Text m_Text;

	void Awake()
	{
		m_Text = GetComponent<Text>();
		m_Text.text = "0:00";
	}

	public void OnTrackChange(MusicPlayer player)
	{
		if (m_Text == null)
		{
			return;
		}

		float t = player.CurrentTrack.Length;
		int min = Mathf.RoundToInt(t / 60f);
		int sec = Mathf.RoundToInt(t % 60f);
		m_Text.text = string.Format("{0}:{1,2}", min, sec.ToString("D2"));
	}
}
