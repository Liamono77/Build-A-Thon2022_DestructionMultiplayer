using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankScript : DestructionNetSync
{
    public PlayerConnection myConnection;

    public Rigidbody rigidBody;

    public Transform myTurret;

    public float turretLerpFactor = 1f;

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
        //ServerGameLogic.serverGameLogic.server.CallRPC("SetTankName", networkID, myConnection.Name);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        //RotateTowardsInput();
        FaceCursor();
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
//        Vector3 lookDir = myConnection.cursorPosition - gameObject.transform.position;
//         float angle = Mathf.Atan2(lookDir.x, lookDir.z) * Mathf.Rad2Deg;
//        Quaternion theQuat = Quaternion.Euler(0, angle, 0);


      //  myTurret.localRotation = theQuat;
        // myTurret.localRotation = Quaternion.Lerp(myTurret.localRotation, theQuat, Time.deltaTime * turretLerpFactor);
        // myTurret.rotation = theQuat;
        // Vector3 euler = myTurret.rotation.eulerAngles;
        //  euler.x = 0;
        // euler.z = 0;

        // myTurret.rotation = Quaternion.Euler(euler);
        //   Quaternion rot = myTurret.rotation;
        //   rot.y = 0;
        //   rot.x = 0;
        //   myTurret.rotation = rot;

        //Quaternion rot = myTurret.rotation;
        //myTurret.LookAt(myConnection.cursorPosition);
        //myTurret.R
        //Quaternion.lo

        Vector3 lookDir = myConnection.cursorPosition - myTurret.position;
        //lookDir.y = turretLerpFactor;
        lookDir.y = 0;


        //myTurret.rotation = Quaternion.LookRotation(lookDir);
        //myTurret.rotation = Quaternion.Lerp(myTurret.rotation, Quaternion.LookRotation(lookDir), Time.deltaTime * turretLerpFactor);//Quaternion.LookRotation(lookDir);
        myTurret.rotation = Quaternion.RotateTowards(myTurret.rotation, Quaternion.LookRotation(lookDir), Time.deltaTime * turretLerpFactor);//Quaternion.LookRotation(lookDir);


        Vector3 vectlol = myTurret.localRotation.eulerAngles;
        vectlol.y = 0;
        vectlol.x = vectlol.x * -1;
        vectlol.z = vectlol.z * -1;
        //Debug.Log($"attempting to rotate by {vectlol}");
        myTurret.Rotate(vectlol);
    }
}
