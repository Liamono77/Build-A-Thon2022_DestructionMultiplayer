using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lidgren.Network;

public class DestructionClient : DestructionNetwork
{
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
            TestSendMessage("HELLO");
        }
    }

    public void InitializeClientNet()
    {
        netConfiguration = new NetPeerConfiguration("DestructionNetwork");
        netPeerKinda = new NetClient(netConfiguration);
        netPeerKinda.Start();
        netPeerKinda.Connect(host: address, port: port);
    }
}
