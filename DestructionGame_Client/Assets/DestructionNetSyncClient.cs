using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//DESTRUCTION NET SYNC (CLIENT)
//This is the client version of my custom NetSync system. 
//Some aspects don't make much sense (projectiles have health?), but whatev. It workz
//WRITTEN BY LIAM SHELTON
public class DestructionNetSyncClient : MonoBehaviour
{
    public int networkID;

    public Vector3 latestPosition;
    public Quaternion latestRotation;

    public float lerpFactor = 2000f;

    public float healthCurrent = 50f;
    public float healthMax = 50f;

    public GameObject healthUIPrefab;

    public ObjectType objectType;
    public enum ObjectType
    {
        stationary,
        tank,
        projectile,
        effect,
        lerpedTransformPrototype,
    }

    public UpdateMode updateMode = UpdateMode.original;
    public enum UpdateMode
    {
        original,
        recordedInterpolation,
    }


    public List<Quaternion> positionsRecord = new List<Quaternion>();
    public float latestUpdateTime; //Server's time 
    public float latestUpdateTravelTime;

    public float expectedTime;
    public float timeSinceLastUpdate;
    public float timeOfLastUpdate;

    public float testfloat = .5f;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        if (objectType != ObjectType.projectile)
        {

        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //After some experimentation, I feel its best to just set transforms directly to the latest. I'll figure out smoothing later. 60hz sync updates to compensate
        if (objectType == ObjectType.tank || objectType == ObjectType.projectile)
        {
            //transform.position = latestPosition;
            //transform.rotation = latestRotation;



            //timeSinceLastUpdate = Time.time - timeOfLastUpdate;
           // expectedTime = latestUpdateTime + timeSinceLastUpdate - latestUpdateTravelTime;
            //expectedTime = latestUpdateTime + timeSinceLastUpdate - testfloat;



            //PrototypeUpdate();
            if (updateMode == UpdateMode.original)
            {
                OriginalUpdate();
            }
            if (updateMode == UpdateMode.recordedInterpolation)
            {
                PrototypeUpdate();
            }
        }
        if (objectType == ObjectType.lerpedTransformPrototype)
        {

        }
    }

    protected virtual void PrototypeUpdate()
    {
        timeSinceLastUpdate = Time.time - timeOfLastUpdate;
        expectedTime = latestUpdateTime + timeSinceLastUpdate - latestUpdateTravelTime;
        foreach (Quaternion timedPosition in positionsRecord)
        {
            if (expectedTime >= timedPosition.w)
            {
                Vector3 newPosition = new Vector3(timedPosition.x, timedPosition.y, timedPosition.z);
                transform.position = newPosition;
            }
        }
        transform.rotation = latestRotation;
    }

    protected virtual void OriginalUpdate()
    {
        transform.position = latestPosition;
        transform.rotation = latestRotation;
    }
}
