using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject[] team1 = new GameObject[3];
    public GameObject[] team2 = new GameObject[3];
    public int team1Score;
    public int team2Score;

    public GameObject[] respawnPoints;

    //This will be the overall skew of the game's DDA
    //Comparing the success of individual players on each team
    //And representing it as a slider between each team
    [SerializeField]
    [Range(-1,1)]
    public float ddaSkew;

    private void Awake()
    {
        if (instance != null)
        {
            if (instance != this)
            {
                Destroy(this.gameObject);
            }
        }
        instance = this;
        DontDestroyOnLoad(instance);

    }
    void Start()
    {
        CalculateDDASkew();
        foreach(var a in team1)
        {
            a.gameObject.layer = 6;
        }
        foreach (var a in team2)
        {
            a.gameObject.layer = 7;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RespawnPlayer(Character deadPlayer)
    {
        CalculateDDASkew();
        //Move the character to a respawn point
        //Make them alive again.
        //Possibly apply DDA
    }

    private void CalculateDDASkew()
    {
        List<float> team1Values = new List<float>();
        List<float> team2Values = new List<float>();
        float team1Skew = 0;
        float team2Skew = 0;
        float t1min = 1000;
        float t2min= 1000;
        float t1max = 0;
        float t2max = 0;

        foreach(var p in team1)
        {
            team1Values.Add(p.GetComponent<Character>().assistance);
            if(p.GetComponent<Character>().assistance < t1min)
            {
                t1min = p.GetComponent<Character>().assistance;
            }
            if(p.GetComponent<Character>().assistance > t1max)
            {
                t1max = p.GetComponent<Character>().assistance;
            }
        }
        foreach (var p in team2)
        {
            team2Values.Add(p.GetComponent<Character>().assistance);
            if (p.GetComponent<Character>().assistance < t2min)
            {
                t2min = p.GetComponent<Character>().assistance;
            }
            if (p.GetComponent<Character>().assistance > t2max)
            {
                t2max = p.GetComponent<Character>().assistance;
            }
        }
        foreach(float p in team1Values)
        {
            team1Skew = (p - t1min) / t1max - t1min;
        }
        foreach (float p in team2Values)
        {
            team2Skew = (p - t2min) / t2max - t2min;
        }

        ddaSkew = -team1Skew + team2Skew;

    }
}
