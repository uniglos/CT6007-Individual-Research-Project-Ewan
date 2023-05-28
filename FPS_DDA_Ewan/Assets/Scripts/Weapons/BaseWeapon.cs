using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWeapon : MonoBehaviour
{
    //public int[] recoilArray;
    public ParticleSystem muzzleFlash;

    protected Character owner;

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

    public float maxBulletMagnetism = 0.6f;

    //private int bulletsHit;
    //private int bulletsFired;
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
        //temporary
        if (owner == null)
        {
            owner = transform.root.GetComponent<Character>();
        }
    }
    protected void OnPickup(Character _owner)
    {
        owner = _owner;
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

    public virtual void FireWeapon(Transform _fpsCamera, Vector3? AIDeviance = null)
    {
        Vector3 appliedDeviance;
        if(owner == null)
        {
            owner = _fpsCamera.transform.root.GetComponent<Character>();
        }
        if (rofTimer >= 10 / bulletsPer10Seconds)
        {
            RaycastHit hit;
            //Layer Mask currently just set to team 2
            //Change to take in which team the player is part of and look at other team
            
            muzzleFlash.Play();
            owner.bulletsFired += 1;
            if (AIDeviance != null) 
            {
                appliedDeviance = _fpsCamera.forward + (Vector3)AIDeviance;
            }
            else
            {
                appliedDeviance = _fpsCamera.forward;
            }
            //Sphere Cast with its radius directly tied to Accuracy Assist will increase leniency of bullets hit box
            if (Physics.SphereCast(_fpsCamera.position, maxBulletMagnetism * owner.accuracyAssist, appliedDeviance, out hit, Mathf.Infinity, layermask))
            {
                //Debug.Log("Rifle Fired");
                Debug.DrawRay(_fpsCamera.position, _fpsCamera.forward*10, Color.red, 5.0f);
                
                Debug.Log(hit.collider.gameObject.name);
                if (hit.collider.gameObject.transform.root.GetComponent<Character>() != null)
                {
                    owner.bulletsHit += 1;
                    owner.currentLifeData.accuracy = (owner.bulletsHit / owner.bulletsFired);
                    DealDamage(hit.collider.gameObject.GetComponent<Character>(), owner);
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
