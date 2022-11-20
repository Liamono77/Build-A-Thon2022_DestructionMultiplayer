using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lidgren.Network;
public class PlayerConnection
{
    public string Name;
    public NetConnection connection;
    public TankScript currentTank;

    public Vector3 cursorPosition;
    public Vector2 moveInput;
    // public 
}
public class ServerGameLogic : MonoBehaviour
{
    public DestructionServer server;

    public static ServerGameLogic serverGameLogic;

    public GameObject playerTankPrefab;

    public NetSyncManager netSyncManager;


    public List<PlayerConnection> playerConnections;

    //public 

    public GameState gameState;
    public enum GameState
    {
        Lobby,
        CountDown,
        Playing,
        GameOver,
    }

    private void Awake()
    {
        serverGameLogic = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [System.Serializable]
    public class DemonstrationPlayer
    {
        public long clientID;
        public string theName;
        public bool isbadguy;
        public int playerClass;
        public float health = 50f;
    }
    public List<DemonstrationPlayer> demonstrationPlayers = new List<DemonstrationPlayer>();

    public DemonstrationPlayer GetDemoPByID(long ID)
    {
        foreach (DemonstrationPlayer player in demonstrationPlayers)
        {
            if (player.clientID == ID)
            {
                return player;
            }
        }
        return null;
    }

    public void ConnectionRequest(NetConnection sender, string name)
    {
        if (GetPlayer(sender) == null)
        {
            PlayerConnection newPlayer = new PlayerConnection();
            newPlayer.connection = sender;
            newPlayer.Name = name;
            //newPlayer.myTank = GameObject.Instantiate(playerTankPrefab)
            //SpawnTank(newPlayer,)
        }

    }
    public void SpawnTank(PlayerConnection connection, Transform spawnLocation)
    {
        TankScript newTank = GameObject.Instantiate(playerTankPrefab, spawnLocation.position, spawnLocation.rotation).GetComponent<TankScript>();
        newTank.myConnection = connection;
        connection.currentTank = newTank;
        //return newTank;
    }
    public PlayerConnection GetPlayer(NetConnection netConnection)
    {
        foreach (PlayerConnection playerConnection in playerConnections)
        {
            if (playerConnection.connection == netConnection)
            {
                return playerConnection;
            }
        }
        return null;
    }

    public void DemoMakeAPlayer(NetConnection sender, string newName, bool isbadguy, int playerClass)
    {
        if (GetDemoPByID(sender.RemoteUniqueIdentifier) != null)//(netServer.Connections.Contains(sender))
        {
            Debug.LogWarning($"Player of ID {sender.RemoteUniqueIdentifier} attempted to create more players than allowed");
            server.CallRPC("RemoteDebugLog", sender, "SERVER MESSAGE: you have attempted to create more players than allowed. Dont do that or else!", 1);
            return;
        }
        DemonstrationPlayer newPlayer = new DemonstrationPlayer();
        newPlayer.clientID = sender.RemoteUniqueIdentifier;
        newPlayer.theName = newName;
        newPlayer.isbadguy = isbadguy;
        newPlayer.playerClass = playerClass;
        demonstrationPlayers.Add(newPlayer);
        //Debug.Log($"This is a generic RPC that has been called remotely!");
    }
}
