using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class MusicManager : MonoBehaviour
{
    public TMP_Text songName;
    public bool isPlaying;
    public Sprite playIcon;
    public Sprite pauseIcon;
    public Image pauseAndPlayImg;

    public GameObject playList;
    private bool isActive;

    public Slider slider;
    public TMP_Text currentTime;
    public TMP_Text allTime;
    public TMP_Text currentSong;

    public Slider vSlider;

    public TMP_InputField search;

    private int playMode = 0;
    public TMP_Text playModeText;
    // 0顺序 //1 循环//2 随机

    public static MusicManager instance;

    public GameObject whatIsPlaying;

    public int count = -1;

    private void Start()
    {
        slider.onValueChanged.AddListener(ChangeTime);
    }

    private void Update()
    {
        audioSource.volume = vSlider.value;
        slider.value = audioSource.time;
        if (audioSource.clip == null)
        {
            currentTime.text = "0:0";
            allTime.text = "0:0";
        }
        else
        {
            currentTime.text = ((int)slider.value / 60) + ":" + Mathf.Round(slider.value - 60 * ((int)slider.value / 60));
            allTime.text = (int)audioSource.clip.length / 60 + ":" + Mathf.Round(audioSource.clip.length - 60 * ((int)audioSource.clip.length / 60));
        }
        if (isPlaying && !audioSource.isPlaying)
        {
            isPlaying = false;
            if (playMode == 0)
                nextButton();
            else if (playMode == 1)
            {
                MusicManager.instance.slider.value = 0;
                play(count);
            }
            else if (playMode == 2)
            {
                count = Random.Range(0, AddMusic.instance.items.Count);
                MusicManager.instance.slider.value = 0;
                play(count);
            }
        }
        if (count == -1)
        {
            currentSong.text = "当前播放：无";
        }
        else
        {
            currentSong.text = "当前播放：" + Path.GetFileNameWithoutExtension(whatIsPlaying.GetComponent<PlayMusic>().songPath);
        }
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);
    }

    public AudioSource audioSource;

    public void PauseAndPlay()
    {
        if (whatIsPlaying == null)
            return;
        if (isPlaying)
        {
            audioSource.Pause();
            pauseAndPlayImg.sprite = playIcon;
            songName.text = "播放";
        }
        else
        {
            audioSource.Play();
            pauseAndPlayImg.sprite = pauseIcon;
            songName.text = "暂停";
        }

        isPlaying = !isPlaying;
    }

    public void PlayInList()
    {
        if (isPlaying)
        {
            songName.text = "暂停";
            pauseAndPlayImg.sprite = pauseIcon;
        }
    }

    public void PlayList()
    {
        isActive = !isActive;
        playList.SetActive(isActive);
    }

    public void aboveButton()
    {
        if (whatIsPlaying == null)
            return;

        count--;
        if (count < 0)
            count = AddMusic.instance.items.Count - 1;
        // MusicManager.instance.slider.value = 0;
        play(count);
    }

    public void nextButton()
    {
        if (whatIsPlaying == null)
            return;
        count++;
        if (count == AddMusic.instance.items.Count)
            count = 0;
        //MusicManager.instance.slider.value = 0;
        play(count);
    }

    public void PlayMode()
    {
        playMode++;
        if (playMode == 3)
            playMode = 0;

        switch (playMode)
        {
            case 0: playModeText.text = "顺序播放"; break;
            case 1: playModeText.text = "循环播放"; break;
            case 2: playModeText.text = "随机播放"; break;
        }
    }

    public void DeleteMusic()
    {
        if (whatIsPlaying == null)
            return;
        AddMusic.instance.items.RemoveAt(count);
        AddMusic.allMusic.allMusic.RemoveAt(count);
        DataSave.WriteJson(AddMusic.allMusic);
        Destroy(whatIsPlaying.GetComponent<PlayMusic>().gameObject);

        if (count == AddMusic.instance.items.Count)
            count--;
        if (count == -1)
        {
            isPlaying = false;
            audioSource.clip = null;
            return;
        }
        play(count);
    }

    private void play(int count)
    {
        if (Path.GetExtension(AddMusic.instance.items[count].GetComponent<PlayMusic>().songPath) == ".mp3")
            StartCoroutine(FileBrowserTest.instance.LoadMp3Song(AddMusic.instance.items[count].GetComponent<PlayMusic>()));
        else if (Path.GetExtension(AddMusic.instance.items[count].GetComponent<PlayMusic>().songPath) == ".wav" || Path.GetExtension(AddMusic.instance.items[count].GetComponent<PlayMusic>().songPath) == ".WAV")
            StartCoroutine(FileBrowserTest.instance.LoadWAVSong(AddMusic.instance.items[count].GetComponent<PlayMusic>()));
    }

    private void ChangeTime(float value)
    {
        audioSource.time = value;
    }

    public void Search()
    {
        string songName = search.text;
        int temp = -1;
        foreach (var i in AddMusic.allMusic.allMusic)
        {
            temp++;
            if (Path.GetFileNameWithoutExtension(i) == songName)
            {
                count = temp;
                play(count);
                return;
            }
        }
        return;
    }
}