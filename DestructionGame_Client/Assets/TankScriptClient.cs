using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankScriptClient : DestructionNetSyncClient
{
    public Transform myTurret;
    public Quaternion latestTurretRotation;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        myTurret.rotation = Quaternion.RotateTowards(myTurret.rotation, latestTurretRotation, Time.deltaTime * lerpFactor);
    }
}
