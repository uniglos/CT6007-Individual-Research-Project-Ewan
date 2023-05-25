using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWeapon : MonoBehaviour
{
    //public int[] recoilArray;
    public ParticleSystem muzzleFlash;

    public bool isAutomatic;
    public int ammo;
    public int magCounter;
    public int magSize;
    public float reloadTime;
    public float reloadTimer;

    public int damage;
    public float bulletsPer10Seconds; // bullets fired per 10 seconds
    public float rofTimer;
    public int weaponPower;
    protected LayerMask layermask;



    void Start()
    {
        layermask = LayerMask.GetMask("Ground", "Team1", "Team2");
        //layermask 
    }

    //void OnPickup();

    protected virtual void Update()
    {
        rofTimer += Time.deltaTime;
        if (magCounter < magSize && (magCounter <= 0 || Input.GetKeyDown(KeyCode.R)))
        {
            ReloadWeapon();
        }
    }

    protected void ReloadWeapon()
    {
        
        if( reloadTimer >= reloadTime)
        {
            magCounter = magSize;
            //ammo -= magSize;
        }
        else
        {
            reloadTimer += Time.deltaTime;
        }
    }

    public virtual void FireWeapon(Transform _fpsCamera)
    {
        if (rofTimer >= 10 / bulletsPer10Seconds)
        {
            RaycastHit hit;
            //Layer Mask currently just set to team 2
            //Change to take in which team the player is part of and look at other team
            
            muzzleFlash.Play();
            if (Physics.Raycast(_fpsCamera.position, _fpsCamera.forward, out hit, Mathf.Infinity,layermask))
            {
                Debug.Log("Rifle Fired");
                Debug.DrawRay(_fpsCamera.position, _fpsCamera.forward, Color.red, 5.0f);
                Debug.Log(hit.collider.gameObject.name);
                if (hit.collider.gameObject.GetComponent<Character>() != null)
                {
                    DealDamage(hit.collider.gameObject.GetComponent<Character>(), _fpsCamera.transform.root.GetComponent<Character>());
                }
                //Animate gun recoil
                //Muzzle flash
                //Hit Marker
            }
            rofTimer = 0.0f;
        }
    }

    public void DealDamage(Character target,Character self)
    {
        target.TakeDamage(damage,self);
    }
}
