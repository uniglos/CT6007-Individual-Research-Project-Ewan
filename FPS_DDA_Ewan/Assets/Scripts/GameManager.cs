using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject enemyPrefab;
    public List<GameObject> team1;
    public List<GameObject> team2;
    public int team1Score;
    public int team2Score;

    public float navDeathMinRatio = 0.25f;
    public SpawnPoint[] respawnPoints;


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
        for(int a=0; a < team1.Count;a++)
        {
            if (team1[a] == null)
            {
                team1[a] = Instantiate<GameObject>(enemyPrefab);
                team1[a].gameObject.layer = LayerMask.NameToLayer("Team1");
            }
        }
        for (int a = 0; a < team2.Count; a++)
        {
            if (team2[a] == null)
            {
                team2[a] = Instantiate<GameObject>(enemyPrefab);
            }
        }
        CalculateDDASkew();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 RespawnPlayer(GameObject self)
    {
        self.TryGetComponent<Character>(out Character deadPlayer);
        CalculateNavigationDDA(deadPlayer);
        SpawnPoint bestSpawn = respawnPoints[0];
        foreach(SpawnPoint r in respawnPoints)
        {
            if(deadPlayer.gameObject.layer == LayerMask.NameToLayer("Team1"))
            {
                if(bestSpawn.team1Prescence > r.team1Prescence)
                {
                    bestSpawn = r;
                }
            }
            if (deadPlayer.gameObject.layer == LayerMask.NameToLayer("Team2"))
            {
                if (r.team2Prescence > r.team1Prescence)
                {
                    bestSpawn = r;
                }
            }
        }
        //Debug.Log(deadPlayer.transform.position);
        //self.transform.position = bestSpawn.transform.position;
        //Debug.Log(deadPlayer.transform.position);
        //Debug.Log(bestSpawn.transform.position);
        //Alter DDA in accordance to how well the player performed in their previous life  
        CalculateCombatDDA(deadPlayer);
        

        if (deadPlayer.combatAssist < 0) deadPlayer.combatAssist = 0;
        if (deadPlayer.navigationAssist < 0) deadPlayer.navigationAssist = 0;
        if (deadPlayer.accuracyAssist < 0) deadPlayer.accuracyAssist = 0;

        //Move the character to a respawn point
        //Make them alive again.
        deadPlayer.assistance = deadPlayer.combatAssist + deadPlayer.navigationAssist + deadPlayer.accuracyAssist;
        CalculateDDASkew();
        return bestSpawn.transform.position;
    }

    void CalculateCombatDDA(Character player)
    {
        player.reasonForDeath.TryGetValue("Combat", out int combatDeaths);
        player.avgDamageDealtPerLife = player.damageDealt / combatDeaths;
        player.avgDamageTakenPerLife = player.damageTaken / combatDeaths;

        //How many kills the player got before they died, divided by how much DDA they were getting
        player.combatEfficacy = player.killDeathRatio / (player.combatAssist + 1);
        //Increase assistance by how poorly the player has been performing over the game
        //Deaths and damage dealt track these
        player.combatAssist = combatDeaths - (player.combatEfficacy * (player.damageDealt / 100));
        //Create a budget and choose where to spend assistance
        float assistBudget = player.combatAssist;
        //Damage Dealt, Damage Taken, Accuracy, Enemies Encountered
        //Gets a ratio of damage dealt per life to determine the ratios of combat based DDA
        player.healthAssist = player.avgDamageDealtPerLife / player.avgDamageTakenPerLife;
        player.speedAssist = player.Health / player.avgDamageDealtPerLife;
        player.accuracyAssist = player.speedAssist;
        Vector3 normalised = new Vector3(player.healthAssist, player.speedAssist, player.accuracyAssist).normalized;

        Debug.Log(normalised);

        player.healthAssist = normalised.x;
        player.speedAssist = normalised.y;
        player.accuracyAssist = normalised.z;
    }

    void CalculateNavigationDDA(Character player)
    {
        int navDeaths = 0;
        if (player.deaths != 0)
        {
            if (player.reasonForDeath.ContainsKey("Out of Bounds"))
            {
                navDeaths = player.reasonForDeath["Out of Bounds"];
            }
            //Navigation DDA is for getting players back into the fight quicker
            //And to help them traverse the map, like making jumping gaps easier
            //reducing respawn timers and spawning you closer to your teammates
            if (navDeaths / player.deaths >= navDeathMinRatio)
            {
                player.coyoteLimit = (navDeaths / player.deaths) * (1 + navDeathMinRatio);
                if (player.causeOfLastDeath == "Out of Bounds") player.respawnLimit /= 2;
            }

            //If you spend a long time not fighting anyone
            //The game will spawn you closer to enemies and allies
            player.spawnKindness = player.previousLifeSpan / player.avgDamageDealtPerLife;
        }
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
