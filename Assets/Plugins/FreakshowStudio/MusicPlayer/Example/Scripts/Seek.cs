using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using FreakLib.Music;

[RequireComponent(typeof(Slider))]
public class Seek : MonoBehaviour 
{
	public MusicPlayer musicPlayer;

	private Slider m_Slider;

	void Awake()
	{
		m_Slider = GetComponent<Slider>();
	}

	void Update()
	{
		if (!musicPlayer.IsPlaying)
		{
			m_Slider.value = 0f;
			return;
		}

		m_Slider.value = musicPlayer.PlaytimeNormalized;
	}

	public void OnEndDrag(BaseEventData eventData)
	{
		musicPlayer.PlaytimeNormalized = m_Slider.value;
	}
}
