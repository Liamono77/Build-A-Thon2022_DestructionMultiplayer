using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankScript : DestructionNetSync
{
    public PlayerConnection myConnection;

    public Transform myTurret;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        //This is extremely hacky but seems to be the only way to get this working.
        ServerGameLogic.serverGameLogic.server.CallRPC("SetCurrentTank", myConnection.connection, networkID);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

    }

    public void FaceCursor()
    {
        Vector3 lookDir = myConnection.cursorPosition - gameObject.transform.position;
        float angle = Mathf.Atan2(lookDir.x, lookDir.y) * Mathf.Rad2Deg;
        Quaternion theQuat = Quaternion.Euler(0, 0, -angle);
        myTurret.rotation = theQuat;
    }
}
