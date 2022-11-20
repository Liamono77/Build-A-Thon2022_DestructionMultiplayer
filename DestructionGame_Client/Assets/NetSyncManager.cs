using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lidgren.Network;

public class NetSyncManager : MonoBehaviour
{
    public List<DestructionNetSyncClient> netSyncs = new List<DestructionNetSyncClient>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //RPC to instantiate network objects
    public void InstantiateNetObject(NetConnection connection, string prefabName, int networkID, float xPos, float yPos, float zPos, float wRot, float xRot, float yRot, float zRot)
    {

        Debug.Log($"Received request to instantiate an object with prefab name {prefabName}");
        Vector3 position = new Vector3(xPos, yPos, zPos);
        Quaternion rotation = new Quaternion(xRot, yRot, zRot, wRot);
        DestructionNetSyncClient newSync = GameObject.Instantiate(Resources.Load<GameObject>(prefabName), position, rotation).GetComponent<DestructionNetSyncClient>();
        newSync.latestRotation = rotation;
        newSync.latestPosition = position;
        newSync.networkID = networkID;
        netSyncs.Add(newSync);
    }

    public void SyncUpdateTank(NetConnection server, int networkID, float xPos, float yPos, float zPos, float wRot, float xRot, float yRot, float zRot)
    {
        DestructionNetSyncClient tankToUpdate = GetNetSync(networkID);
        tankToUpdate.latestPosition = new Vector3(xPos, yPos, zPos);
        tankToUpdate.latestRotation = new Quaternion(xRot, yRot, zRot, wRot);
    }

    public DestructionNetSyncClient GetNetSync(int ID)
    {
        foreach (DestructionNetSyncClient netSync in netSyncs)
        {
            if (netSync.networkID == ID)
            {
                return netSync;
            }
        }
        Debug.LogError($"Client has attempted to get a NetSync of ID {ID}, which doesn't exist");
        return null;
    }
}
