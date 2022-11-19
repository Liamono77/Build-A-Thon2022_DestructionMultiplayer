using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lidgren.Network;

public class DestructionClient : DestructionNetwork
{
    NetClient netClient;
    public string address = "127.0.0.1";
    public int port = 603;

    public bool testBool;

    // Start is called before the first frame update
    void Start()
    {
        InitializeClientNet();
    }

    // Update is called once per frame

    protected override void Update()
    {
        base.Update();
        if (testBool)
        {
            testBool = false;
            CallRPC("DemoMakeAPlayer", "Johnson", false, 2);
            CallRPC("DemoMakeAPlayer", "Hector", true, 1);

        }
    }

    public void InitializeClientNet()
    {
        netConfiguration = new NetPeerConfiguration("DestructionNetwork");
        netPeerKinda = new NetClient(netConfiguration);
        netPeerKinda.Start();
        netPeerKinda.Connect(host: address, port: port);

        netClient = netPeerKinda as NetClient;
    }
    public void CallRPC(string theMessage, params object[] parameters)
    {
        CallRPC(theMessage, NetDeliveryMethod.ReliableOrdered, parameters);
    }
    public void CallRPC(string theMessage, NetDeliveryMethod netDeliveryMethod, params object[] parameters)
    {
        NetOutgoingMessage message = netPeerKinda.CreateMessage();
        message.Write(theMessage);
        WriteRPCParameters(message, parameters);
        netClient.SendMessage(message, netDeliveryMethod);
    }

    public void RemoteDebugLog(NetConnection serverConnection, string theLog, int severity)
    {
        if (severity == 0)
        {
            Debug.Log("SERVER REMOTE LOG: " + theLog);
        }
        if (severity == 1)
        {
            Debug.LogWarning("SERVER REMOTE LOG: " + theLog);
        }
        if (severity == 2)
        {
            Debug.LogError("SERVER REMOTE LOG: " + theLog);
        }
    }
}
