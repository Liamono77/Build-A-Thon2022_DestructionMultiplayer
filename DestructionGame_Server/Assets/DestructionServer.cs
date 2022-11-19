using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lidgren.Network;
public class DestructionServer : DestructionNetwork
{
    public int port = 603;


    public bool testbool;
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
    }

    public void InitializeServerNet()
    {
        netConfiguration = new NetPeerConfiguration("DestructionNetwork")
        {
            Port = port,
        };
        netPeerKinda = new NetServer(netConfiguration);
        netPeerKinda.Start();
    }

    public void ShowMeTheConnections()
    {
        NetServer netServer = netPeerKinda as NetServer;
        foreach (NetConnection connection in netServer.Connections)
        {
            Debug.Log($"player is connected with an ID of {connection.RemoteUniqueIdentifier}");
        }
    }

    public void GenericRPC()
    {
        Debug.Log($"This is a generic RPC that has been called remotely!");
    }
}
