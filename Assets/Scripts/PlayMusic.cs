using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class PlayMusic : MonoBehaviour
{
    private Button song;
    public string songPath;

    private void Start()
    {
        song = GetComponentInChildren<Button>();
        song.onClick.AddListener(Play);
    }

    public void Play()
    {
        //MusicManager.instance.slider.value = 0;
        print(Path.GetExtension(songPath));
        if (Path.GetExtension(songPath) == ".mp3")
        {
            StartCoroutine(FileBrowserTest.instance.LoadMp3Song(this));
        }
        else if (Path.GetExtension(songPath) == ".wav" || Path.GetExtension(songPath) == ".WAV")
        {
            StartCoroutine(FileBrowserTest.instance.LoadWAVSong(this));
        }
    }
}