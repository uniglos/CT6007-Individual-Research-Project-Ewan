using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWeapon : MonoBehaviour
{
    //public int[] recoilArray;
    public ParticleSystem muzzleFlash;

    public int ammo;
    public int magCounter;
    public int magSize;
    public float reloadTime;
    public float reloadTimer;

    public int damage;
    public float bulletsPer10Seconds; // bullets fired per 10 seconds
    public float rofTimer;
    public int weaponPower;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
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

    }
}
