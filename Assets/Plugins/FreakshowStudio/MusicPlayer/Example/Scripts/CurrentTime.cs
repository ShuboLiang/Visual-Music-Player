using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using FreakLib.Music;

[RequireComponent(typeof(Text))]
public class CurrentTime : MonoBehaviour 
{
	public MusicPlayer musicPlayer;
	private Text m_Text;

	void Awake()
	{
		m_Text = GetComponent<Text>();
	}

	void Update()
	{
		if (!musicPlayer.IsPlaying)
		{
			m_Text.text = "0:00";
		}
		else
		{
			float t = musicPlayer.Playtime;
			int min = Mathf.RoundToInt(t / 60f);
			int sec = Mathf.RoundToInt(t % 60f);
			m_Text.text = string.Format("{0}:{1,2}", min, sec.ToString("D2"));
		}
	}
}
