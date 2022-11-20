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

    public void InstantiateNetObject(NetConnection connection, string prefabName, int networkID, float xPos, float yPos, float zPos, float wRot, float xRot, float yRot, float zRot)
    {
        Vector3 position = new Vector3(xPos, yPos, zPos);
        Quaternion rotation = new Quaternion(wRot, xRot, yRot, zRot);
        DestructionNetSyncClient newSync = GameObject.Instantiate(Resources.Load<GameObject>(prefabName), position, rotation).GetComponent<DestructionNetSyncClient>();
        newSync.networkID = networkID;
        netSyncs.Add(newSync);
    }
}
