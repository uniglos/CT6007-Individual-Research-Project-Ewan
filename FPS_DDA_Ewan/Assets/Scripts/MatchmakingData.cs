using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
//https://prasetion.medium.com/saving-data-as-json-in-unity-4419042d1334
//Adapted from this Medium Post by Prasetio Nugroho, 2019
public class MatchmakingData : MonoBehaviour
{
    static public MatchmakingData instance;

    // Start is called before the first frame update

    string dataPath;
    string json;

    private void Awake()
    {
        if (instance != null)
        {
            if (instance != this)
            {
                Destroy(this);
            }
        }
        instance = this;
    }

    public void SaveGame(Queue<LifeData> lifeData, string playerName)
    {
        dataPath = Application.persistentDataPath +"/"+ playerName;
        string file = dataPath + "/MatchmakingData.dat";
        if (Directory.Exists(dataPath))
        {
            if (File.Exists(file))
            {
                string data = JsonUtility.ToJson(lifeData, true);
                System.IO.File.WriteAllText(dataPath, data);
            }
            else
            {
                File.Create(file);
            }
        }
        else
        {
            Directory.CreateDirectory(dataPath);
        }



    }

    public Queue<LifeData> LoadGame(string playerName)
    {
        dataPath = Application.persistentDataPath + "/" + playerName;
        string file = dataPath + "/MatchmakingData.dat";
        if (Directory.Exists(dataPath))
        {
            if (File.Exists(file))
            {
                string data = File.ReadAllText(file);
                Queue<LifeData> lives = JsonUtility.FromJson<Queue<LifeData>>(data);
                return lives;
            }
            else
            {
                File.Create(file);
                return new Queue<LifeData>();
            }

        }
        else
        {
            Directory.CreateDirectory(dataPath);
            return new Queue<LifeData>();
        }
    }
}
