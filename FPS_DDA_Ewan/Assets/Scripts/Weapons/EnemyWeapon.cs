using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : BaseWeapon
{
    private void Start()
    {
        isAutomatic = true;
        layermask = LayerMask.GetMask("Ground", "Team1", "Team2");
    }

    public override void FireWeapon(Transform _fpsCamera, Vector3? AIDeviance = null)
    {
        Vector3 appliedDeviance;
        if (owner == null)
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
                appliedDeviance = _fpsCamera.forward + ((Vector3)AIDeviance*owner.accuracyAssist);
            }
            else
            {
                appliedDeviance = _fpsCamera.forward;
            }
            //Sphere Cast with its radius directly tied to Accuracy Assist will increase leniency of bullets hit box
            if (Physics.SphereCast(_fpsCamera.position, 0.1f, appliedDeviance, out hit, Mathf.Infinity, layermask))
            {
                //Debug.Log("Rifle Fired");
                Debug.DrawRay(_fpsCamera.position, _fpsCamera.forward * 10, Color.red, 5.0f);

                //Debug.Log(hit.collider.gameObject.name);
                if (hit.collider.gameObject.transform.root.GetComponent<Character>() != null
                    && hit.collider.gameObject.transform.root.GetComponent<Character>() != owner)
                {
                    owner.bulletsHit += 1;
                    owner.currentLifeData.accuracy = (owner.bulletsHit / owner.bulletsFired);
                    owner.accuracy = (owner.bulletsHit / owner.bulletsFired);
                    DealDamage(hit.collider.gameObject.GetComponent<Character>(), owner.gameObject);
                }
                //Animate gun recoil
                //Muzzle flash
                //Hit Marker
            }
            rofTimer = 0.0f;
        }
    }
}
