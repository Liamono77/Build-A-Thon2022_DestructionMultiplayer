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
    }

    bool isDestroyed;

    public NetSyncManager manager;

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
