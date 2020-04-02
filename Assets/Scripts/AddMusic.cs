using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Collections.Generic;

public class AddMusic : MonoBehaviour
{
    public static AddMusic instance;

    private Button addButton;

    public static AllMusic allMusic;
    public FileBrowserTest fileBrowser;

    public GameObject item;
    private GameObject list;

    public List<GameObject> items;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);
    }

    private void Start()
    {
        list = GameObject.FindGameObjectWithTag("list");
        allMusic = DataSave.LoadJson();
        DisplayMusic();
        addButton = GetComponent<Button>();
        addButton.onClick.AddListener(AddButton);
    }

    private void AddButton()
    {
        fileBrowser.ShowDialog();
    }

    public void DisplayMusic()
    {
        foreach (var song in allMusic.allMusic)
        {
            GameObject _Instance = Instantiate(item);
            _Instance.transform.SetParent(list.transform);
            _Instance.GetComponent<PlayMusic>().songPath = song;
            TMP_Text text = _Instance.GetComponentInChildren<TMP_Text>();
            text.text = Path.GetFileNameWithoutExtension(song);
            items.Add(_Instance);
        }
    }

    public void AddItem(string path)
    {
        GameObject _Instance = Instantiate(item);
        _Instance.transform.SetParent(list.transform);
        TMP_Text text = _Instance.GetComponentInChildren<TMP_Text>();
        _Instance.GetComponent<PlayMusic>().songPath = path;
        text.text = Path.GetFileNameWithoutExtension(allMusic.allMusic[allMusic.allMusic.Count - 1]);
        text.color = new Color(0.6f, 0.6f, 0.6f);
        items.Add(_Instance);
    }
}