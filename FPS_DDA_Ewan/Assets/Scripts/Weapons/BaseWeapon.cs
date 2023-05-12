using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWeapon : MonoBehaviour
{
    //public int[] recoilArray;

    public int ammo;
    public int magCounter;
    public int magSize;
    public float reloadTime;
    public float reloadTimer;

    public int damage;
    public int rateOfFire;
    public int weaponPower;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(magCounter < magSize && (magCounter <= 0 || Input.GetKeyDown(KeyCode.R)))
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

    public virtual void FireWeapon()
    {

    }
}
