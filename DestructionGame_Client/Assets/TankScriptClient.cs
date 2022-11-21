using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//TANK SCRIPT (CLIENT)
//A simple extension to the DestructionNetSync to make setting the turret rotation easier. COuld also be used for wheels
//WRITTEN BY LIAM SHELTON
public class TankScriptClient : DestructionNetSyncClient
{
    public Transform myTurret;
    public Quaternion latestTurretRotation;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        ClientGameLogic.clientGameLogic.client.CallRPC("RequestRename", networkID);
        Debug.Log($"Sent rename request to server for tank with network ID {networkID}");
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        myTurret.rotation = Quaternion.RotateTowards(myTurret.rotation, latestTurretRotation, Time.deltaTime * lerpFactor);
    }
}
