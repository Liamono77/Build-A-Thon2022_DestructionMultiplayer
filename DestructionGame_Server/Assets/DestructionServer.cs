using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lidgren.Network;
public class DestructionServer : DestructionNetwork
{
    public int port = 603;

    // Start is called before the first frame update
    void Start()
    {
        InitializeServerNet();
    }

    // Update is called once per frame

    protected override void Update()
    {
        base.Update();

    }

    public void InitializeServerNet()
    {
        netConfiguration = new NetPeerConfiguration("DestructionNetwork")
        {
            Port = 603,
        };
        netPeerKinda = new NetServer(netConfiguration);
        netPeerKinda.Start();
    }
}
