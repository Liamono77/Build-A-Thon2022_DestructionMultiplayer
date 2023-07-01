using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//TANK SCRIPT
//This is the script behind the tank objects. Rather typical character controller type stuff.
//The tanks in this game are programatically driven this time around. No animators here.
//WRITTEN BY LIAM SHELTON
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


    public float attackTimer;
    public float attackDelay = .25f;

    public bool Dead;
    public float deathModeDuration = 2f;

    public Transform barrelEnd;

    public GameObject projectilePrefab;
    public GameObject muzzleFlashFX;
    public GameObject deathExplosionPrefab;

    public List<Vector3> positionBufferPrototype = new List<Vector3>();


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

        if (healthCurrent > 0)
        {
            FaceCursor();
            DriveFunct();
        }
        else if (Dead == false)
        {
            Dead = true;
            Destroy(gameObject, deathModeDuration);
            GameObject.Instantiate(deathExplosionPrefab, transform.position, transform.rotation);
        }

        //bufferedSyncInfoUpdate();
    }

    public  void bufferedSyncInfoUpdate()
    {
        positionBufferPrototype.Add(gameObject.transform.position);
        if (positionBufferPrototype.Count > 3)
        {
            positionBufferPrototype.RemoveAt(4);
        }
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
        Vector3 lookDir = myConnection.cursorPosition - myTurret.position;
        lookDir.y = 0;
        myTurret.rotation = Quaternion.RotateTowards(myTurret.rotation, Quaternion.LookRotation(lookDir), Time.deltaTime * turretLerpFactor);


        Vector3 vectlol = myTurret.localRotation.eulerAngles;
        vectlol.y = 0;
        vectlol.x = vectlol.x * -1;
        vectlol.z = vectlol.z * -1;;
        myTurret.Rotate(vectlol);
    }

    public void AttemptToFire()
    {
        if (healthCurrent > 0)
        {
            if (attackTimer < Time.time)
            {
                attackTimer = Time.time + attackDelay;
                Debug.Log($"Tank with player of name{myConnection.Name} has fired");

                GameObject.Instantiate(muzzleFlashFX, barrelEnd.position, barrelEnd.rotation);
                GameObject.Instantiate(projectilePrefab, barrelEnd.position, barrelEnd.rotation);
            }
        }

    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        GameObject.Instantiate(deathExplosionPrefab, transform.position, transform.rotation);
    }
}
