using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : BaseWeapon
{
    // Start is called before the first frame update
    void Start()
    {
        isAutomatic = true;
        layermask = LayerMask.GetMask("Ground", "Team1", "Team2");
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
