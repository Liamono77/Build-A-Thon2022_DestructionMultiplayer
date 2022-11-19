using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lidgren.Network;
public class DestructionServer : DestructionNetwork
{
    NetServer netServer;
    public int port = 603;


    public bool testbool;
   // public bool testbool2;
    // Start is called before the first frame update
    void Start()
    {
        InitializeServerNet();
    }

    // Update is called once per frame

    protected override void Update()
    {
        base.Update();
        if (testbool)
        {
            testbool = false;
            ShowMeTheConnections();
        }
      //  if (testbool2)
     //   {
     //       testbool2 = false;
      //      CallRPC("RemoteDebugLog", "REMOTEDEBUGLOGLOLOLOL", 0);
      //  }
    }

    public void InitializeServerNet()
    {
        netConfiguration = new NetPeerConfiguration("DestructionNetwork")
        {
            Port = port,
        };
        netPeerKinda = new NetServer(netConfiguration);
        netPeerKinda.Start();

        netServer = netPeerKinda as NetServer;
    }

    public void ShowMeTheConnections()
    {
        NetServer netServer = netPeerKinda as NetServer;
        foreach (NetConnection connection in netServer.Connections)
        {
            Debug.Log($"player is connected with an ID of {connection.RemoteUniqueIdentifier}");
        }
    }

    //These two CallRPCS are for sending rpcs to specific clients
    public void CallRPC(string theMessage, NetConnection recipient, params object[] parameters)
    {
        CallRPC(theMessage, recipient, NetDeliveryMethod.ReliableOrdered, parameters);
    }
    public void CallRPC(string theMessage, NetConnection recipient, NetDeliveryMethod netDeliveryMethod, params object[] parameters)
    {
        NetOutgoingMessage message = netPeerKinda.CreateMessage();
        message.Write(theMessage);
        WriteRPCParameters(message, parameters);
        netServer.SendMessage(message, recipient, netDeliveryMethod);
        Debug.Log($"Sent RPC for function {theMessage} to player of ID {recipient.RemoteUniqueIdentifier}");
    }

    //These two are for sending rpcs all clients
    public void CallRPC(string theMessage, params object[] parameters)
    {
        CallRPC(theMessage, NetDeliveryMethod.ReliableOrdered, parameters);
    }
    public void CallRPC(string theMessage, NetDeliveryMethod netDeliveryMethod, params object[] parameters)
    {
        NetOutgoingMessage message = netPeerKinda.CreateMessage();
        message.Write(theMessage);
        WriteRPCParameters(message, parameters);
        netServer.SendToAll(message, netDeliveryMethod);
        Debug.Log($"Sent RPC for function {theMessage} to all players");
    }
}
