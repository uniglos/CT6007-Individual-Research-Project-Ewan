using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public CharacterController characterController;

    [SerializeField]
    protected int health = 100;
    protected float moveSpeed = 10f;
    protected bool isSprinting = false;
    protected bool isCrouching = false;
    protected float jumpHeight = 3f;

    public float gravity = -9.81f;
    protected Vector3 velocity;

    public Transform groundCheck;
    public float groundDistance = 0.3f;
    public LayerMask groundMask;
    protected bool isGrounded;

    public BaseWeapon[] weapons = new BaseWeapon[2];
    public int currentWeapon = 0;
    //Enumerator for what powerups are active
    public int score;
    public int scoreDropped;

    public string playerName;

    protected bool isDead;
    private float respawnTimer;



    //DDA variables
    public int kills;
    public int killsPerLife;
    public int deaths;
    public int assists;
    /* These are the values that will be used to track and apply DDA
     * expected efficacy is how well we expect the player to do (SBMM)
     * efficacy is how well the player is doing in-game
     * assistance is how much DDA the player is benefitting from    */
    protected float expectedEfficacy;
    public float efficacy = 0;
    public float assistance = 0;

    

    // Start is called before the first frame update
    public virtual void Start()
    {
        health = 100;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        CharacterMovement();
        //Aim and Fire
        WeaponUpdate();
        if(isDead)
        {
            if(respawnTimer <= 4.0f)
            {
                respawnTimer += Time.deltaTime;
            }
            else
            {
                respawnTimer = 0.0f;
                isDead = false;
                GameManager.instance.RespawnPlayer(this);
            }
        }
    }

    public void TakeDamage(int damage, Character attacker)
    {
        //Take damage equal to damage dealt by a weapon
        health -= damage;
        if(health<=0)
        {
            Die(attacker);
        }
    }

    public void Die(Character killer)
    {
        //Perish
        //Give score to player who killed you
        killer.score += scoreDropped;
        isDead = true;

        //Alter killers stats
        killer.kills += 1;
        killer.killsPerLife += 1;

        //Alter DDA in accordance to how well the player performed in this life
        //How many kills the player got before they died, divided by how much DDA they were getting
        float DDAcalc = killsPerLife / assistance;
        efficacy += DDAcalc + (kills/deaths);
        //Increase assistance by how poorly the player has been performing over the game
        //Deaths and efficacy track these
        assistance = deaths - efficacy;
        if (assistance < 0) assistance = 0;

        deaths += 1;
        killsPerLife = 0;
    }

    protected virtual void CharacterMovement()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        velocity.y += gravity * Time.deltaTime;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -1f;
        }

        characterController.Move(velocity * Time.deltaTime);
    }

    protected virtual void WeaponUpdate()
    {
        
    }
}
