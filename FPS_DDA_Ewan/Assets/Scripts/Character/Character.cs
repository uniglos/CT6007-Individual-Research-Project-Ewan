using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private int health;
    private float moveSpeed;
    public GameObject[] weapons = new GameObject[2];
    protected int score;
    public int scoreDropped;

    public string playerName;

    //DDA variables
    /* These are the values that will be used to track and apply DDA
     * expected efficacy is how well we expect the player to do (SBMM)
     * efficacy is how well the player is doing in-game
     * assistance is how much DDA the player is benefitting from    */
    protected float expectedEfficacy;
    protected float efficacy;
    protected float assistance;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Movement
        //Aim and Fire
    }

    public void TakeDamage(int damage)
    {
        //Take damage equal to damage dealt by a weapon
    }

    public void Die()
    {
        //Perish
        //Give score to player who killed you
    }
}
