using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] team1 = new GameObject[3];
    public GameObject[] team2 = new GameObject[3];
    public int team1Score;
    public int team2Score;

    //This will be the overall skew of the game's DDA
    //Comparing the success of individual players on each team
    //And representing it as a slider between each team
    public float ddaSkew;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
