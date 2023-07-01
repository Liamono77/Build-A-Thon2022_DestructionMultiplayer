using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//DESTRUCTION NET SYNC
//This is a server-oriented network synchronization script, with functionality to make instantiation/destruction of network objects automatic.
//It would probably make sense to decouple some data from this, but whatev. I was in a hurry
//WRITTEN BY LIAM SHELTON
public class DestructionNetSync : MonoBehaviour
{
    public int networkID;
    public GameObject originalPrefab;
    public float healthCurrent = 50f;
    public float healthMax = 50f;

    //public bool isStationary;

    public NetSyncType netSyncType;
    public enum NetSyncType
    {
        tank,
        building,
        projectile,
        explosionFX,
        //lerpedTransformPrototype,
    }

    bool isDestroyed;

    public NetSyncManager manager;

    public List<Quaternion> positionsRecord = new List<Quaternion>();
    public List<Quaternion> rotationsRecord = new List<Quaternion>();//

    protected virtual void Start()
    {
        //ServerGameLogic.serverGameLogic.netSyncManager.netSyncs.Add(this);
        manager = ServerGameLogic.serverGameLogic.netSyncManager;
        manager.AddNetSyncObject(this);
    }
    protected virtual void Update()
    {
        if (healthCurrent <= 0)
        {
            if (isDestroyed == false)
            {
                isDestroyed = true;
                DeathFunction();

            }
        }


        Quaternion newPosition = new Quaternion();
        newPosition.x = transform.position.x;
        newPosition.y = transform.position.y;
        newPosition.z = transform.position.z;
        newPosition.w = Time.time;
        positionsRecord.Add(newPosition);


        Quaternion newRotation = new Quaternion();
        //Vector3 eulerRotation = transform.rotation.eulerAngles;
        newRotation.x = transform.rotation.eulerAngles.x;
        newRotation.y = transform.rotation.eulerAngles.y;
        newRotation.z = transform.rotation.eulerAngles.z;
        newRotation.w = Time.time;
        positionsRecord.Add(newRotation);


    }

    public virtual void syncUpdateExtension()
    {
        positionsRecord.Clear();
        rotationsRecord.Clear();
    }

    public virtual void TakeDamage(float amount, int sourceNetworkID)
    {
        float newHealth = healthCurrent - amount;

        //clamp the health
        if (newHealth > healthMax)
        {
            newHealth = healthMax;
        }

        healthCurrent = newHealth;
        //ServerGameLogic.serverGameLogic.server.CallRPC("")
        manager.UpdateSyncObjectHealth(this, sourceNetworkID);
        
    }
    public virtual void DeathFunction()
    {

    }
    protected virtual void OnDestroy()
    {
        //ServerGameLogic.serverGameLogic.netSyncManager.netSyncs.Remove(this);
        manager.RemoveNetSyncObject(this);
    }
}
