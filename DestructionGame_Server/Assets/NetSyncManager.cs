using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lidgren.Network;
public class NetSyncManager : MonoBehaviour
{
    private int lastID = 1;
    [SerializeField]
    private List<DestructionNetSync> netSyncs;

    public float syncUpdateTimer;
    public float syncUpdateDelay;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (syncUpdateTimer < Time.time)
        {
            syncUpdateTimer = Time.time + syncUpdateDelay;
            syncUpdate();
        }
    }

    public void syncUpdate()
    {
        foreach (DestructionNetSync netSync in netSyncs)
        {
            if (netSync.isStationary == false)
            {
                //float xPos = netSync.
                //ServerGameLogic.serverGameLogic.server.CallRPC("SyncUpdate", NetDeliveryMethod.UnreliableSequenced, netSync.networkID, );
                //TankScript tankToUpdate = netSync.gameObject.GetComponent<TankScript>()
            }
        }
    }
    public int GetNewNetworkID()
    {
        lastID++;
        return lastID;
    }
    public void AddNetSyncObject(DestructionNetSync netSync)
    {
        netSync.networkID = GetNewNetworkID();

        ServerGameLogic.serverGameLogic.server.CallRPC("InstantiateNetObject", netSync.originalPrefab.name, netSync.networkID, netSync.transform.position.x, netSync.transform.position.y, netSync.transform.position.z, netSync.transform.rotation.w, netSync.transform.rotation.x, netSync.transform.position.y, netSync.transform.position.z);

        netSyncs.Add(netSync);
    }
    public void RemoveNetSyncObject(DestructionNetSync netSync)
    {

        ServerGameLogic.serverGameLogic.server.CallRPC("DestroyNetObject", netSync.networkID);

        netSyncs.Remove(netSync);

    }
    public void UpdateSyncObjectHealth(DestructionNetSync netSync)
    {
        ServerGameLogic.serverGameLogic.server.CallRPC("UpdateHealth", netSync.networkID, netSync.healthCurrent);

    }

    public void SendSyncObjects(NetConnection sender)
    {
        foreach (DestructionNetSync netSync in netSyncs)
        {
            ServerGameLogic.serverGameLogic.server.CallRPC("InstantiateNetObject", sender, netSync.originalPrefab.name, netSync.networkID, netSync.transform.position.x, netSync.transform.position.y, netSync.transform.position.z, netSync.transform.rotation.w, netSync.transform.rotation.x, netSync.transform.position.y, netSync.transform.position.z);
        }
    }
}
