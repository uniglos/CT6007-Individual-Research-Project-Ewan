using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    protected int health;
    protected float moveSpeed = 10f;
    protected float jumpHeight = 3f;

    public float gravity = -9.81f;
    protected Vector3 velocity;

    public Transform groundCheck;
    public float groundDistance = 0.3f;
    public LayerMask groundMask;
    protected bool isGrounded;

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
    public virtual void Update()
    {
        CharacterMovement();
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

    protected virtual void CharacterMovement()
    {

    }
}
