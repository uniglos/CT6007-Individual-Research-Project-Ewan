using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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

    private Vector3 sphere;
    bool doSphere;

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
                sphere = hit.point;
                DrawWireCapsule(_fpsCamera.position, _fpsCamera.rotation, maxBulletMagnetism * owner.accuracyAssist, 10.0f);
                if (hit.collider.gameObject.transform.root.GetComponent<Character>() != null
                    && hit.collider.gameObject.transform.root.GetComponent<Character>() != owner )
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

    public static void DrawWireCapsule(Vector3 _pos, Quaternion _rot, float _radius, float _height, Color _color = default(Color))
    {
        if (_color != default(Color))
            Handles.color = Gizmos.color;
        Matrix4x4 angleMatrix = Matrix4x4.TRS(_pos, _rot, Handles.matrix.lossyScale);
        using (new Handles.DrawingScope(angleMatrix))
        {
            var pointOffset = (_height - (_radius * 2)) / 2;

            //draw sideways
            Handles.DrawWireArc(Vector3.up * pointOffset, Vector3.left, Vector3.back, -180, _radius);
            Handles.DrawLine(new Vector3(0, pointOffset, -_radius), new Vector3(0, -pointOffset, -_radius));
            Handles.DrawLine(new Vector3(0, pointOffset, _radius), new Vector3(0, -pointOffset, _radius));
            Handles.DrawWireArc(Vector3.down * pointOffset, Vector3.left, Vector3.back, 180, _radius);
            //draw frontways
            Handles.DrawWireArc(Vector3.up * pointOffset, Vector3.back, Vector3.left, 180, _radius);
            Handles.DrawLine(new Vector3(-_radius, pointOffset, 0), new Vector3(-_radius, -pointOffset, 0));
            Handles.DrawLine(new Vector3(_radius, pointOffset, 0), new Vector3(_radius, -pointOffset, 0));
            Handles.DrawWireArc(Vector3.down * pointOffset, Vector3.back, Vector3.left, -180, _radius);
            //draw center
            Handles.DrawWireDisc(Vector3.up * pointOffset, Vector3.up, _radius);
            Handles.DrawWireDisc(Vector3.down * pointOffset, Vector3.up, _radius);

        }
    }
        private void OnDrawGizmos()
    {

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(sphere, maxBulletMagnetism * owner.accuracyAssist);
        doSphere = false;
    }

    protected void DealDamage(Character target,GameObject attacker)
    {
        target.TakeDamage(damage,attacker);
    }
}
