using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

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
        dataPath = Application.persistentDataPath +"/"+ playerName+ "/MatchmakingData.dat";
        if (File.Exists(dataPath))
        {
            string data = JsonUtility.ToJson(lifeData, true);
            System.IO.File.WriteAllText(dataPath, data);
        }
        else
        {
            File.Create(dataPath);
        }



    }

    public Queue<LifeData> LoadGame(string playerName)
    {
        dataPath = Application.persistentDataPath + "/" + playerName + "/MatchmakingData.dat";
        if (File.Exists(dataPath))
        {
            string data = File.ReadAllText(dataPath);

            Queue<LifeData> lives = JsonUtility.FromJson<Queue<LifeData>>(data);
            return lives;

        }
        else
        {
            return new Queue<LifeData>();
        }
    }
}
