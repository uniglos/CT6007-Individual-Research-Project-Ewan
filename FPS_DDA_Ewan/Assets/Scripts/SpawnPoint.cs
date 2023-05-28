using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public float team1Prescence;
    public float team2Prescence;

    public int numOfTeam1;
    public int numOfTeam2;

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(team1Prescence > numOfTeam1)
        {
            team1Prescence -= Time.deltaTime;
        }
        if (team2Prescence > numOfTeam2)
        {
            team2Prescence -= Time.deltaTime;
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Character>(out Character character) )
        {
            if(character.gameObject.layer == LayerMask.NameToLayer("Team1"))
            {
                team1Prescence += 1;
                numOfTeam1 += 1;
            }
            if (character.gameObject.layer == LayerMask.NameToLayer("Team2"))
            {
                team2Prescence += 1;
                numOfTeam2 += 1;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Character>(out Character character))
        {
            if (character.gameObject.layer == LayerMask.NameToLayer("Team1"))
            {
                numOfTeam1 -= 1;
            }
            if (character.gameObject.layer == LayerMask.NameToLayer("Team2"))
            {
                numOfTeam2 -= 1;
            }
            
        }
    }
}
