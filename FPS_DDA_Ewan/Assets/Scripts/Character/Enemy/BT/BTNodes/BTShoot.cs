using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class BTShoot : BTNode
{
    public BaseWeapon gun;
    public float accuracy;

    private Character target = null;
    private Character self;

    private float timer;

    public BTShoot()
    {
        target = EnemyBT.self.GetComponent<FieldOfView>().primaryTarget;
        accuracy = EnemyBT.self.accuracy;
        self = EnemyBT.self;
    }

    public override NodeState Evaluate()
    {
        Debug.Log("Start Shoot");
        if(target == null)
        {
            target = (Character)parent.GetData("enemy");
        }
        if (target != null)
        {
            Debug.Log("Shot");
            self.gameObject.transform.LookAt(target.transform);
            self.weapons[self.currentWeapon].FireWeapon(self.gunPoint);
            
        }
        if(target.playerDead)
        {
            parent.ClearData("enemy");
            target = null;
            EnemyBT.self.GetComponent<FieldOfView>().primaryTarget = null;
        }
        state = NodeState.RUNNING;
        return state;
    }

    private void AIAccuracy()
    {

    }
}