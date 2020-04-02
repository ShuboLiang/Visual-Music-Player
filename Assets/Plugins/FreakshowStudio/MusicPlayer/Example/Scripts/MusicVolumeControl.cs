using UnityEngine;
using UnityEngine.Audio;
using FreakLib.Music;

public class MusicVolumeControl : MonoBehaviour 
{
	[SerializeField] private MusicPlayer m_MusicPlayer;
	[SerializeField] private AudioMixer m_MasterMixer;
	[SerializeField] private string m_MusicVolumeParameter = "MusicVolume";
	
	public void SetMusicVolume(float attenuation)
	{
#if UNITY_WEBGL
		// Mixer volume is not supported in WebGL, so use music player
		// volume instead.
		float a = (attenuation + 80f) / 80f; // Map from [-80,0] to [0,1]
		m_MusicPlayer.Volume = a;
#else
		m_MasterMixer.SetFloat(m_MusicVolumeParameter, attenuation);
#endif
	}
}
