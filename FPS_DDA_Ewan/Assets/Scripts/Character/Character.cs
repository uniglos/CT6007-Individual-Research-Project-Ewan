using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public CharacterController characterController;

    [SerializeField]
    protected float health = 100;
    public float Health => health;
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

    protected bool isDead = true;
    public bool playerDead => isDead;

    protected Color originalCol;
    private Material defaultMaterial;
    private float matTimer =0.0f;
    




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
     *      Bullet Magnetism
                */

    public int kills;
    public float killDeathRatio;
    public int deaths;
    public float damageDealt;
    public float avgDamageDealtPerLife;
    public float damageTaken;
    public float avgDamageTakenPerLife;
    
    public int assists;

    public LifeData currentLifeData;
    public Queue<LifeData> lives = new Queue<LifeData>(); //Save Load System

    public float distanceTravelled;
    public GameObject spawnPoint;
    public float timeSpentAlive;
    public float previousLifeSpan;
    public string causeOfLastDeath;

    public List<string> playersFought;
    public int playersEncountered;


    public int bulletsFired;
    public int bulletsHit;
    public float accuracy;
    public float avgAccuracy;

    public Dictionary<string, int> reasonForDeath = new Dictionary<string, int>();   //When a player dies, the reason will be passed through to the game manager.

    /* These are the values that will be used to track and apply DDA
     * expected efficacy is how well we expect the player to do (SBMM)
     * efficacy is how well the player is doing in-game
     * assistance is how much DDA the player is benefitting from    */
    protected float expectedCmbtEff;
    protected float expectedAccEff;
    protected float expectedNavEff;

    public float combatEfficacy = 0;
    public float accuracyEfficacy = 0;
    public float navigationEfficacy = 0;

    public float combatAssist = 0; //Tracker of how much assistance the player is getting
    public float speedAssist = 0; //Increase in movespeed
    public float healthAssist = 0; //Damage Resistance
    public bool damageBoost = false; //Static Damage boost
    public bool powerUpBoost = false; //Static PowerUp boost

    public float accuracyAssist = 0; //Increased "bullet" size

    public float navigationAssist = 0;
    public float coyoteLimit = 0.5f;
    public float coyoteTimer = 0;
    protected bool canJump;
    protected float respawnTimer;
    public float respawnLimit = 4.0f;
    public float spawnKindness;

    public float assistance;


    
    // Start is called before the first frame update
    public virtual void Start()
    {
        reasonForDeath = new Dictionary<string, int>();
        originalCol = gameObject.GetComponent<Renderer>().material.color;
        gameObject.TryGetComponent<Renderer>(out Renderer renderer);
        if(renderer == null)
        {
            renderer = gameObject.GetComponentInChildren<Renderer>();
        }
        defaultMaterial = renderer.material;
        //Load In LifeData Queue() to lives
        if (playerName != null)
        {
            lives = MatchmakingData.instance.LoadGame(playerName);
        }
        if (lives != null)
        {
            foreach (LifeData i in lives)
            {
                kills += i.kills;
                damageDealt += i.damageDealt;
                damageTaken += i.damageTaken;
                accuracy += i.accuracy;
                distanceTravelled += i.distanceTravelled;
                timeSpentAlive += i.timeSpentAlive;
                //playersFought.Add(i.playersFought);
                playersEncountered += i.playersEncountered;
                if (reasonForDeath.ContainsKey(i.deathCause))
                {
                    reasonForDeath[i.deathCause] += 1;
                }
                deaths += 1;

            }
            if (deaths > 0)
            {
                killDeathRatio = kills / deaths;
                avgDamageDealtPerLife = damageDealt / deaths;
                avgDamageTakenPerLife = damageTaken / deaths;
                accuracy = accuracy / deaths;
            }
        }
        GameManager.instance.CalculateCombatDDA(this);

        health = 100;
        if (gameObject.GetComponent<EnemyBT>())
        {
            playerName = "Name";
        }

        kills = 0;
        damageDealt = 0;
        damageTaken = 0;
        if (gameObject.GetComponent<EnemyBT>() != null)
        {
            accuracy = 0;
        }
        distanceTravelled = 0;
        timeSpentAlive = 0;
        //playersFought.Add(i.playersFought);
        playersEncountered = 0;
        deaths = 0;
        currentLifeData = new LifeData();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (!isDead)
        {
            if (gameObject.GetComponent<EnemyBT>() == null)
            {
                CharacterMovement();
                //Aim and Fire
                WeaponUpdate();
            }
        }
        if (isDead)
        {
            gravity = 0;
            velocity.y = 0;
            characterController.enabled = false;
            gameObject.transform.position = GameManager.instance.transform.position;
            previousLifeSpan = timeSpentAlive;
            timeSpentAlive = 0;
            if (respawnTimer <= respawnLimit)
            {
                respawnTimer += Time.deltaTime;
            }
            else
            {
                gravity = -9.81f;
                health = 100;
                isDead = false;
                respawnTimer = 0.0f;
                respawnLimit = 4.0f;
                transform.position = GameManager.instance.RespawnPlayer(this.gameObject);
                //Debug.Log("Respawn Point: " + GameManager.instance.RespawnPlayer(this.gameObject));
                lives = MatchmakingData.instance.LoadGame(playerName);
                currentLifeData = new LifeData();
                characterController.enabled = true;
                moveSpeed = moveSpeed + speedAssist;
            }
        }
        else
        {
            timeSpentAlive += Time.deltaTime;
        }
        DamageFlashUpdate();
    }

    protected void DamageFlashUpdate()
    {
        if(gameObject.GetComponent<Renderer>().material.color == Color.red && matTimer > 0.25f)
        {
            gameObject.GetComponent<Renderer>().material.color = originalCol;
            matTimer = 0;
        }
        else
        {
            matTimer += Time.deltaTime;
        }
    }
    public void TakeDamage(float damage, GameObject dealer)
    {
        if (!isDead&&dealer.GetComponent<Character>()!=null)
        {
            Character attacker = dealer.GetComponent<Character>();
            gameObject.GetComponent<Renderer>().material.color = Color.red;
            matTimer = 0.0f;
            if (!playersFought.Contains(attacker.playerName))
            {
                playersFought.Add(attacker.playerName);
                playersEncountered += 1;
            }

            
            //Take damage equal to damage dealt by a weapon
            damage = damage/(1+healthAssist);

            health -= damage;
            attacker.currentLifeData.damageDealt += damage;
            attacker.damageDealt += damage;
            damageTaken += damage;
            currentLifeData.damageTaken += damage;
            if (health <= 0)
            {
                Die(attacker.gameObject, "Combat");
            }
        }
        if(gameObject.GetComponent<EnemyBT>()!=null)
        {
            transform.LookAt(dealer.transform);
        }
    }

    public void Die(GameObject deathCause, string causeForDeath = "Unknown")
    {
        //Perish
        isDead = true;
        Character killer;
        deaths += 1;
        causeOfLastDeath = causeForDeath;
        Debug.Log(deathCause);
        if (deathCause.TryGetComponent<Character>(out killer))
        {
            //Give score to player who killed you
            killer.score += scoreDropped;
            

            //Alter killers stats
            killer.currentLifeData.kills += 1;
            killer.kills += 1;
            //killer.avgkillsPerLife += 1;
            if (killer.gameObject.GetComponent<FieldOfView>().visibleTargets.Contains(this.transform))
            {
                killer.gameObject.GetComponent<FieldOfView>().visibleTargets.Remove(this.transform);
            }
        }
        Debug.Log(causeForDeath);
        Debug.Log(reasonForDeath);
        if (!reasonForDeath.ContainsKey(causeForDeath))
        {
            reasonForDeath.Add(causeForDeath, 1);
        }
        else
        {
            reasonForDeath[causeForDeath] += 1;

        }
        lives.Enqueue(currentLifeData);
        MatchmakingData.instance.SaveGame(lives,playerName);
    }

    protected virtual void CharacterMovement()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (coyoteTimer <= coyoteLimit && canJump)
        {
            coyoteTimer += Time.deltaTime;
        }
        else
        {
            canJump = false;
        }
        if (isGrounded)
        {
            coyoteTimer = 0;
            canJump = true;
        }
        
        
        
        velocity.y += gravity * Time.deltaTime;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -1f;
            canJump = true;
        }

        characterController.Move(velocity * Time.deltaTime);
    }

    public void CalculateAverages(Character player)
    {
        player.avgDamageDealtPerLife = player.damageDealt / player.deaths;
        player.avgDamageTakenPerLife = player.damageTaken / player.deaths;
    }

    protected virtual void WeaponUpdate()
    {
        int calcDeaths = deaths;
        if (calcDeaths <= 0)
        {
            calcDeaths = 1;
        }
        killDeathRatio = (float)kills / (float)calcDeaths;
        avgDamageDealtPerLife = damageDealt / calcDeaths;
        avgDamageTakenPerLife = damageTaken / calcDeaths;
        
        
    }



}
public struct LifeData
{
    public int kills;

    public float accuracy;

    public float damageDealt;

    public float damageTaken;

    public int assists;

    public float distanceTravelled;

    public float timeSpentAlive;

    public List<string> playersFought;
    public int playersEncountered;

    public string deathCause;
}
