using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public CharacterController characterController;

    [SerializeField]
    protected int health = 100;
    public float moveSpeed = 10f;
    protected bool isSprinting = false;
    protected bool isCrouching = false;
    public float jumpHeight = 3f;

    public float gravity = -9.81f;
    protected Vector3 velocity;

    public Transform groundCheck;
    public float groundDistance = 0.3f;
    public LayerMask groundMask;
    protected bool isGrounded;

    public BaseWeapon[] weapons = new BaseWeapon[2];
    public int currentWeapon = 0;
    public Transform gunPoint;
    //Enumerator for what powerups are active
    public int score;
    public int scoreDropped;

    public string playerName;

    protected bool isDead;
    public bool playerDead => isDead;
    private float respawnTimer;



    //DDA variables
    //Make Subsystems for Combat, Navigation, Accuracy
    /*Combat:
     * Track Kills, Killstreaks, Damage Dealt and Taken, Deaths
     * DDA Methods
     *      Increased Health
     *      Increased MoveSpeed
     *      
                */
    /*Navigation:
     *  Track Distance Travelled from spawn, Time Spent Alive, Players Encountered and Fought
     *  DDA Methods
     *      Coyote Time
     *      
                */
    /* Accuracy:
     * Tracks accuracy
     * DDA Methods
     *      Aim Assist per weapon
     *      Bullet Magnetism
                */

    public int kills;
    public int killsPerLife;
    public float damageDealt;
    public float avgDamageDealtPerLife;
    public float damageTaken = 0;
    public float avgDamageTakenPerLife;
    public int deaths;
    public int assists;

    public LifeData currentLifeData;

    public float distanceTravelled;
    public GameObject spawnPoint;
    public float timeSpentAlive;

    public List<string> playersFought;
    public int playersEncountered;


    public int bulletsFired;
    public int bulletsHit;
    public float accuracy;

    public Dictionary<string, int> reasonForDeath;   //When a player dies, the reason will be passed through to the game manager.

    /* These are the values that will be used to track and apply DDA
     * expected efficacy is how well we expect the player to do (SBMM)
     * efficacy is how well the player is doing in-game
     * assistance is how much DDA the player is benefitting from    */
    protected float expectedEfficacy;
    public float combatEfficacy = 0;
    public float accuracyEfficacy = 0;
    public float navigationEfficacy = 0;
    public float combatAssist = 0;
    public float accuracyAssist = 0;
    public float navigationAssist = 0;
    public float coyoteLimit = 0;
    public float coyoteTimer = 0;

    public float assistance;



    // Start is called before the first frame update
    public virtual void Start()
    {
        health = 100;
        if (gameObject.GetComponent<EnemyBT>())
        {
            playerName = "Name";
        }
    }

    // Update is called once per frame
    public virtual void Update()
    {
        CharacterMovement();
        //Aim and Fire
        WeaponUpdate();
        if (isDead)
        {
            if (respawnTimer <= 4.0f)
            {
                respawnTimer += Time.deltaTime;
            }
            else
            {
                respawnTimer = 0.0f;
                //isDead = false;
                //GameManager.instance.RespawnPlayer(this);
            }
        }
    }

    public void TakeDamage(int damage, Character attacker)
    {
        if (!playersFought.Contains(attacker.playerName))
        {
            playersFought.Add(attacker.playerName);
            playersEncountered += 1;
        }


        //Take damage equal to damage dealt by a weapon
        health -= damage;

        attacker.damageDealt += damage;
        damageTaken += damage;
        if (health <= 0)
        {
            Die(attacker, attacker.playerName);
        }
    }

    public void Die(Character killer, string causeForDeath)
    {
        //Perish
        //Give score to player who killed you
        killer.score += scoreDropped;
        isDead = true;

        //Alter killers stats
        killer.kills += 1;
        killer.killsPerLife += 1;
        if (killer.gameObject.GetComponent<FieldOfView>().visibleTargets.Contains(this.transform))
        {
            killer.gameObject.GetComponent<FieldOfView>().visibleTargets.Remove(this.transform);
        }
        if (!reasonForDeath.ContainsKey(causeForDeath))
        {
            reasonForDeath.Add(causeForDeath, 1);
        }
        else
        {
            reasonForDeath[causeForDeath] += 1;

        }
    }

    protected virtual void CharacterMovement()
    {

        if (coyoteTimer <= coyoteLimit && !Physics.CheckSphere(groundCheck.position, groundDistance, groundMask))
        {
            coyoteTimer += Time.deltaTime;

        }
        else
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
            coyoteTimer = 0;
        }
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

    public struct LifeData
    {
        public int kills;
        public int killsPerLife;
        public float damageDealt;
        public float avgDamageDealtPerLife;
        public float damageTaken;
        public float avgDamageTakenPerLife;
        public int deaths;
        public int assists;
    }

}
