using UnityEngine;
using System.Collections;
using SimpleFileBrowser;
using System.IO;
using TMPro;
using UnityEngine.UI;

public class FileBrowserTest : MonoBehaviour
{
    public AudioSource audioSource;
    private string fileName;
    public static FileBrowserTest instance;

    public Slider slider;

    // Start is called before the first frame update
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);
    }

    private void Start()
    {
        FileBrowser.SetFilters(true, new FileBrowser.Filter("Audio", ".wav", ".mp3"));
        FileBrowser.SetDefaultFilter(".wav");
        FileBrowser.SetExcludedExtensions(".lnk", ".tmp", ".zip", ".rar", ".exe");
        FileBrowser.AddQuickLink("Users", "C:\\Users", null);
    }

    public void ShowDialog()
    {
        StartCoroutine(ShowLoadDialogCoroutine());
    }

    private IEnumerator ShowLoadDialogCoroutine()
    {
        yield return FileBrowser.WaitForLoadDialog(false, null, "Load File", "Load");
        fileName = FileBrowser.Result;
        if (Path.GetExtension(fileName) == ".mp3" || Path.GetExtension(fileName) == ".wav" || Path.GetExtension(fileName) == ".WAV")
        {
            AddMusic.allMusic.allMusic.Add(fileName);
            DataSave.WriteJson(AddMusic.allMusic);
            AddMusic.instance.AddItem(fileName);
        }

        //Debug.Log(FileBrowser.Success + " " + FileBrowser.Result);
        if (FileBrowser.Success)
        {
            byte[] bytes = FileBrowserHelpers.ReadBytesFromFile(FileBrowser.Result);
        }
    }

    public IEnumerator LoadMp3Song(PlayMusic item)
    {
        string url = item.songPath;
        WWW www = new WWW(url);
        yield return www;
        audioSource.clip = NAudioPlayer.FromMp3Data(www.bytes);
        audioSource.Play();
        foreach (var i in AddMusic.instance.items)
        {
            i.GetComponentInChildren<TMP_Text>().color = new Color(0.6f, 0.6f, 0.6f);
        }
        item.GetComponentInChildren<TMP_Text>().color = new Color(1, 1, 1);
        MusicManager.instance.isPlaying = true;
        MusicManager.instance.PlayInList();

        MusicManager.instance.whatIsPlaying = item.gameObject;
        MusicManager.instance.slider.value = 0;
        slider.maxValue = audioSource.clip.length;

        MusicManager.instance.count = -1;
        foreach (var i in AddMusic.instance.items)
        {
            MusicManager.instance.count++;
            if (MusicManager.instance.whatIsPlaying == i)
                break;
        }
    }

    public IEnumerator LoadWAVSong(PlayMusic item)
    {
        string url = item.songPath;
        WWW www = new WWW(url);
        yield return www;
        audioSource.clip = www.GetAudioClip();
        audioSource.Play();
        foreach (var i in AddMusic.instance.items)
        {
            i.GetComponentInChildren<TMP_Text>().color = new Color(0.6f, 0.6f, 0.6f);
        }

        item.GetComponentInChildren<TMP_Text>().color = new Color(1, 1, 1);

        MusicManager.instance.isPlaying = true;
        MusicManager.instance.PlayInList();

        MusicManager.instance.whatIsPlaying = item.gameObject;
        MusicManager.instance.slider.value = 0;
        slider.maxValue = audioSource.clip.length;
        MusicManager.instance.count = -1;
        foreach (var i in AddMusic.instance.items)
        {
            MusicManager.instance.count++;
            if (MusicManager.instance.whatIsPlaying == i)
                break;
        }
    }
}