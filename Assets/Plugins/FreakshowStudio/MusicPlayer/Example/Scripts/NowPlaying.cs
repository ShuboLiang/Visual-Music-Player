using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using FreakLib.Music;

[RequireComponent(typeof(Text))]
public class NowPlaying : MonoBehaviour 
{
	private Text m_Text;

	void Awake()
	{
		m_Text = GetComponent<Text>();
	}

	public void OnPlay(MusicPlayer mp)
	{
		m_Text.text = mp.CurrentTrackName;
	}

	public void OnStop(MusicPlayer mp)
	{
		m_Text.text = "STOPPED";
	}

	public void OnPause(MusicPlayer mp)
	{
		m_Text.text = "PAUSED";
	}

	public void OnUnPause(MusicPlayer mp)
	{
		m_Text.text = mp.CurrentTrackName;
	}

	public void OnTrackChange(MusicPlayer mp)
	{
		m_Text.text = mp.CurrentTrackName;
	}
}
