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
            if (netSync.netSyncType == DestructionNetSync.NetSyncType.tank)
            {
                //float xPos = netSync.
                //ServerGameLogic.serverGameLogic.server.CallRPC("SyncUpdateTank", NetDeliveryMethod.UnreliableSequenced, netSync.networkID, );
                //TankScript tankToUpdate = netSync.gameObject.GetComponent<TankScript>();
                TankScript tankToUpdate = netSync as TankScript;
                ServerGameLogic.serverGameLogic.server.CallRPC("SyncUpdateTank", NetDeliveryMethod.UnreliableSequenced, tankToUpdate.networkID, tankToUpdate.transform.position.x, tankToUpdate.transform.position.y, tankToUpdate.transform.position.z, tankToUpdate.transform.rotation.w, tankToUpdate.transform.rotation.x, tankToUpdate.transform.rotation.y, tankToUpdate.transform.rotation.z, tankToUpdate.myTurret.rotation.w, tankToUpdate.myTurret.rotation.x, tankToUpdate.myTurret.rotation.y, tankToUpdate.myTurret.rotation.z);
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
        //string originalName = netSync.gameObject.name.Replace("(Clone)", "");

        //ServerGameLogic.serverGameLogic.server.CallRPC("InstantiateNetObject", GetPrefabName(netSync), netSync.networkID, netSync.transform.position.x, netSync.transform.position.y, netSync.transform.position.z, netSync.transform.rotation.w, netSync.transform.rotation.x, netSync.transform.rotation.y, netSync.transform.rotation.z);
        SendNetworkObject(netSync);
        netSyncs.Add(netSync);
    }
    public void RemoveNetSyncObject(DestructionNetSync netSync)
    {

        ServerGameLogic.serverGameLogic.server.CallRPC("DestroyNetObject", netSync.networkID);

        netSyncs.Remove(netSync);

    }
    public void UpdateSyncObjectHealth(DestructionNetSync netSync, int sourceNetworkID)
    {
        ServerGameLogic.serverGameLogic.server.CallRPC("UpdateHealth", netSync.networkID, netSync.healthCurrent, sourceNetworkID);

    }

    public void SendSyncObjects(NetConnection sender)
    {
        foreach (DestructionNetSync netSync in netSyncs)
        {
            //ServerGameLogic.serverGameLogic.server.CallRPC("InstantiateNetObject", sender, netSync.originalPrefab.name, netSync.networkID, netSync.transform.position.x, netSync.transform.position.y, netSync.transform.position.z, netSync.transform.rotation.w, netSync.transform.rotation.x, netSync.transform.position.y, netSync.transform.position.z);
            SendNetworkObject(netSync, sender);
        }
    }

    //RPC from clients requesting a network object they're getting NRE's for
    public void RequestObject(NetConnection sender, int networkID)
    {
        //server.CallRPC("")
        foreach (DestructionNetSync netSync in netSyncs)
        {
            if (netSync.networkID == networkID)
            {
                //ServerGameLogic.serverGameLogic.server.CallRPC("InstantiateNetObject", sender, netSync.originalPrefab.name, netSync.networkID, netSync.transform.position.x, netSync.transform.position.y, netSync.transform.position.z, netSync.transform.rotation.w, netSync.transform.rotation.x, netSync.transform.position.y, netSync.transform.position.z);
                SendNetworkObject(netSync, sender);
            }
        }
    }

    //RPC to set a tank name
    public void RequestRename(NetConnection sender, int networkID)
    {
        //ServerGameLogic.serverGameLogic.GetPlayer(sender).Name
        // PlayerConnection thePlayer
        string theName = null;
       foreach (DestructionNetSync netSync in netSyncs)
        {
            if (netSync.networkID == networkID)
            {
                if (netSync.netSyncType == DestructionNetSync.NetSyncType.tank)
                {
                    TankScript theTank = netSync as TankScript;
                    theName = theTank.myConnection.Name;
                }
            }
        }
       if (theName != null)
        {
            Debug.LogWarning($"Attempting to send rename response to client of ID{sender.RemoteUniqueIdentifier} for object of ID{networkID}");
            ServerGameLogic.serverGameLogic.server.CallRPC("SetTankName", sender, networkID, theName);
        }
        else
        {
            Debug.LogError($"Attempt to find a player connection associated with network ID {networkID} has failed. Sending error response...");
            ServerGameLogic.serverGameLogic.server.CallRPC("RemoteDebugLog", sender, $"The server was unable to find a player for network ID {networkID}", 2);
        }

    }

    //RPC to make a tank fire!
    public void RequestFire(NetConnection sender)
    {
        PlayerConnection requestingPlayer = ServerGameLogic.serverGameLogic.GetPlayer(sender);
        if (requestingPlayer != null)
        {
            if (requestingPlayer.currentTank != null)
            {
                requestingPlayer.currentTank.AttemptToFire();
            }
        }


    }

    public void SendNetworkObject(DestructionNetSync netSync, NetConnection recipient) //This is the one 
    {
        ServerGameLogic.serverGameLogic.server.CallRPC("InstantiateNetObject", recipient, GetPrefabName(netSync), netSync.networkID, netSync.transform.position.x, netSync.transform.position.y, netSync.transform.position.z, netSync.transform.rotation.w, netSync.transform.rotation.x, netSync.transform.rotation.y, netSync.transform.rotation.z);
        Debug.LogWarning($"An instantiation request from client of ID{recipient} for object of ID {netSync.networkID} has been responded to");
    }
    public void SendNetworkObject(DestructionNetSync netSync)
    {
        ServerGameLogic.serverGameLogic.server.CallRPC("InstantiateNetObject", GetPrefabName(netSync), netSync.networkID, netSync.transform.position.x, netSync.transform.position.y, netSync.transform.position.z, netSync.transform.rotation.w, netSync.transform.rotation.x, netSync.transform.rotation.y, netSync.transform.rotation.z);

    }
    public string GetPrefabName(DestructionNetSync netSync)
    {
        string originalName = netSync.gameObject.name.Replace("(Clone)", "");
        return originalName;
    }
}
