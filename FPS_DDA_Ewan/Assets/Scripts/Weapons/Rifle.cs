using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : BaseWeapon
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        

    }

    public override void FireWeapon(Transform _fpsCamera)
    {
        if(rofTimer >= 10/rateOfFire)
        {
            RaycastHit hit;
            //Layer Mask currently just set to team 2
            //Change to take in which team the player is part of and look at other team
            LayerMask layermask = 7;
            if(Physics.Raycast(_fpsCamera.position, _fpsCamera.forward, out hit, Mathf.Infinity))
            {
                Debug.Log("Rifle Fired");
                Debug.DrawRay(_fpsCamera.position, _fpsCamera.forward, Color.red,1.0f);
                Debug.Log(hit.collider.gameObject.name);
            }
            rofTimer = 0.0f;
        }
    }
}
