using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class BTShoot : BTNode
{
    public BaseWeapon gun;
    public float inaccuracy;
    private Vector3 t;

    private Character target = null;
    private Character self;

    private float timer;

    public BTShoot(Character _self)
    {
        target = _self.GetComponent<FieldOfView>().primaryTarget;
        inaccuracy = 1-_self.accuracy;
        self = _self;
    }

    public override NodeState Evaluate()
    {
        //Debug.Log("Start Shoot");
        if(target == null)
        {
            target = (Character)parent.GetData("enemy");
        }
        if (target != null)
        {
            //Debug.Log("Shot");
            
            self.gameObject.transform.LookAt(target.transform.position);
            t = target.transform.forward + (Random.insideUnitSphere * inaccuracy);
            self.weapons[self.currentWeapon].FireWeapon(self.gunPoint,t);
            state = NodeState.SUCCESS;
            //Debug.Log("Returned Shoot State Success");
            return state;
            
        }
        if(target.playerDead)
        {
            parent.ClearData("enemy");
            target = null;
            self.GetComponent<FieldOfView>().primaryTarget = null;
        }
        state = NodeState.RUNNING;
       // Debug.Log("Returned Shoot State RUNNING");
        return state;
    }

    private void AIAccuracy()
    {

    }
}
