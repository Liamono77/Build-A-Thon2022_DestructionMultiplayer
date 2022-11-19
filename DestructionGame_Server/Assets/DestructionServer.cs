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

    [System.Serializable]
    public class DemonstrationPlayer
    {
        public string theName;
        public bool isbadguy;
        public int playerClass;
        public float health = 50f;
    }
    public List<DemonstrationPlayer> demonstrationPlayers = new List<DemonstrationPlayer>();
    public void DemoMakeAPlayer(string newName, bool isbadguy, int playerClass)
    {
        DemonstrationPlayer newPlayer = new DemonstrationPlayer();
        newPlayer.theName = newName;
        newPlayer.isbadguy = isbadguy;
        newPlayer.playerClass = playerClass;
        demonstrationPlayers.Add(newPlayer);
        //Debug.Log($"This is a generic RPC that has been called remotely!");
    }
}
