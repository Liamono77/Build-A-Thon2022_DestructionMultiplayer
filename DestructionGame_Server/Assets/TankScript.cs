using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankScript : DestructionNetSync
{
    public PlayerConnection myConnection;

    public Rigidbody rigidBody;

    public Transform myTurret;

    public Vector2 targetVector;


    //copied over from tankdrive test script
    public List<WheelCollider> leftWheels = new List<WheelCollider>();
    public List<WheelCollider> rightWheels = new List<WheelCollider>();

    public Vector2 inputVector;

    public float wheelMultiplier = 2000;
    public float breakTorque = 0f;




    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        rigidBody = GetComponent<Rigidbody>();
        //This is extremely hacky but seems to be the only way to get this working.
        ServerGameLogic.serverGameLogic.server.CallRPC("SetCurrentTank", myConnection.connection, networkID);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        //RotateTowardsInput();
        //FaceCursor();
        //rigidBody.AddForce()
        DriveFunct();
    }
    public void RotateTowardsInput()
    {
        //Vector2 currentDirection = transform.position.
        //targetVector = new Vector2(transform.position.x, transform.position.z) + myConnection.moveInput;

        //targetVector = targetVector + myConnection.moveInput;


    }

    //copied from tankdrive test script
    public void DriveFunct()
    {
        inputVector = myConnection.moveInput;
        if (inputVector.magnitude > 1)
        {
            inputVector = inputVector.normalized;
        }

        foreach (WheelCollider wheel in leftWheels)
        {
            wheel.motorTorque = (inputVector.y + inputVector.x) * wheelMultiplier;
            wheel.brakeTorque = breakTorque;
        }
        foreach (WheelCollider wheel in rightWheels)
        {
            wheel.motorTorque = (inputVector.y - inputVector.x) * wheelMultiplier;
            wheel.brakeTorque = breakTorque;
        }
    }



    public void FaceCursor()
    {
        Vector3 lookDir = myConnection.cursorPosition - gameObject.transform.position;
        float angle = Mathf.Atan2(lookDir.x, lookDir.y) * Mathf.Rad2Deg;
        Quaternion theQuat = Quaternion.Euler(0, 0, -angle);
        myTurret.rotation = theQuat;
    }
}
