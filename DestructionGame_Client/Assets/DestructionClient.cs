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
            //TestSendMessage("HELLO");
            //TestSendMessage("GenericRPC", 20, false, "LOL");
            //TestSendMessage("GenericRPC", "LOL", false, 20, 12.45f);
            TestSendMessage("DemoMakeAPlayer", "Johnson", false, 2);
            TestSendMessage("DemoMakeAPlayer", "Hector", true, 1);



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
    public void TestSendMessage(string theMessage, params object[] parameters)
    {
        NetOutgoingMessage message = netPeerKinda.CreateMessage();
        message.Write(theMessage);
        WriteRPCParameters(message, parameters);
        netClient.SendMessage(message, NetDeliveryMethod.ReliableOrdered);
    }
}
