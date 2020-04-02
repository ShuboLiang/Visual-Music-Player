using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataSave : MonoBehaviour
{
    private void Start()
    {
        CreateJson(Application.dataPath + @"\data.json");
    }

    private void CreateJson(string dataPath)
    {
        if (File.Exists(dataPath))
            return;
        StreamWriter sw = new StreamWriter(dataPath);
        var allMusic = new AllMusic();
        allMusic.allMusic = new List<string>();
        string js = JsonUtility.ToJson(allMusic);
        sw.Write(js);
        sw.Close();
    }

    public static AllMusic LoadJson()
    {
        StreamReader sr = new StreamReader(Application.dataPath + @"\data.json");
        string js = sr.ReadToEnd();
        sr.Close();
        return JsonUtility.FromJson<AllMusic>(js);
    }

    public static void WriteJson(AllMusic allMusic)
    {
        StreamWriter sw = new StreamWriter(Application.dataPath + @"\data.json");
        string js = JsonUtility.ToJson(allMusic);
        sw.Write(js);
        sw.Close();
    }
}