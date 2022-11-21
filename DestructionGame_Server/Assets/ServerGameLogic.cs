using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lidgren.Network;
using UnityEngine.UI;
using System;

//PLAYER CONNECTION
//This class is the representation of players that are playing the game.
//WRITTEN BY LIAM SHELTON
[System.Serializable]
public class PlayerConnection
{
    public string Name;
    public int teamID = 20;
    public NetConnection connection;
    public bool readyStatus;

    public TankScript currentTank;

    public Vector3 cursorPosition;
    public Vector2 moveInput;

    public float fireTimer;
    public float fireDelay;

    public bool isQuedForRemoval;

  //  public void AttemptToFire()
  //  {
       // if (fireTimer < Time.time)
        //{
            //GameObject.Instantiate'

        //}
   // }
    // public 
}

//SERVER GAME LOGIC
//Much of the game functionality is here. Somewhat like a secondary game manager
//WRITTEN BY LIAM SHELTON
public class ServerGameLogic : MonoBehaviour
{
    public DestructionServer server;

    public static ServerGameLogic serverGameLogic;

    public GameObject playerTankPrefab;

    public NetSyncManager netSyncManager;

    public TankManager tankManager;


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

    public int minumumPlayersToStartGame = 1;
    //public bool teamToggler

    public bool kickingEnabled;
    public float kickTimer;
    public float kickDelay = 5f;

    //public InputField inputFieldAdd;
    public InputField inputFieldPort;
    public GameObject startScreen;


    private void Awake()
    {
        serverGameLogic = this;
        //gameState.to
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    public void StartButton() //this doesn't really get used if the server is built as a server
    {
        server.port = Int32.Parse(inputFieldPort.text);
        server.InitializeServerNet();
        startScreen.SetActive(false);
        //Camera.main.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (kickingEnabled)
        {
            KickUpdateFunct();
        }
    }
    
    //This checks if any clients have shut down their games. It kicks them if they fail to respond to a connection check RPC
    public void KickUpdateFunct()
    {
        if (kickTimer < Time.time)
        {
            kickTimer = Time.time + kickDelay;

            List<PlayerConnection> playersToKick = new List<PlayerConnection>();

            foreach (PlayerConnection player in playerConnections)
            {
                //Check if they were marked and did not respond
                if (player.isQuedForRemoval)
                {
                    if (player.currentTank != null)
                    {
                        Destroy(player.currentTank.gameObject);
                    }
                    //playerConnections.Remove(player);
                    playersToKick.Add(player);
                    Debug.LogWarning($"Player of name {player.Name}, ID {player.connection.RemoteUniqueIdentifier} has failed to respond to connection check. They've been kicked.");
                }

                player.isQuedForRemoval = true;
                server.CallRPC("ConnectionCheck", player.connection);
            }

            foreach (PlayerConnection player in playersToKick)
            {
                playerConnections.Remove(player);
            }
        }
    }

    [System.Serializable]
    public class DemonstrationPlayer //obsolete
    {
        public long clientID;
        public string theName;
        public bool isbadguy;
        public int playerClass;
        public float health = 50f;
    }
    public List<DemonstrationPlayer> demonstrationPlayers = new List<DemonstrationPlayer>();

    public DemonstrationPlayer GetDemoPByID(long ID) //obsolete
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


    //misnomer
    public void ConnectionRequest(NetConnection sender, string name)
    {
        if (GetPlayer(sender) == null)
        {
            PlayerConnection newPlayer = new PlayerConnection();
            newPlayer.connection = sender;
            newPlayer.Name = name;

            //newPlayer.myTank = GameObject.Instantiate(playerTankPrefab)
            //SpawnTank(newPlayer,)
            playerConnections.Add(newPlayer);
            Debug.LogWarning($"Player of ID {sender.RemoteUniqueIdentifier} has joined");
            server.CallRPC("HandshakeFromServer", sender, gameState.ToString());

            if (gameState == GameState.Playing)
            {
                tankManager.AssignPlayerToTeam(newPlayer);
               // server.CallRPC("RemoteDebugLog", sender, "SERVER MESSAGE: you have attempted to join an ongoing game", 1);
            }
        }
        else
        {
            Debug.LogWarning($"Player of ID {sender.RemoteUniqueIdentifier} has attempted to connect too many times");
        }

    }


    //RPC from clients in response to connection check request
    public void ConnectionCheckResponse(NetConnection sender)
    {
        PlayerConnection playerToUpdate = GetPlayer(sender);
        if (playerToUpdate != null)
        {
            playerToUpdate.isQuedForRemoval = false;
            Debug.Log($"Player of name {playerToUpdate.Name}, ID {playerToUpdate.connection.RemoteUniqueIdentifier} has responded to connection check");
        }
        else
        {
            Debug.LogError($"a connection check has been received from ID {sender.RemoteUniqueIdentifier} for a player that doesn't seem to exist. weird...");
        }
    }

    //RPC from client to update their PlayerConnection with their inputs. 
    public void UpdateInputs(NetConnection sender, float xPos1, float yPos1, float zPos1, float xPos2, float yPos2)
    {
        PlayerConnection playerToUpdate = GetPlayer(sender);
        Vector3 cursorPosition = new Vector3(xPos1, yPos1, zPos1);
        Vector2 inputVector = new Vector2(xPos2, yPos2);
        playerToUpdate.cursorPosition = cursorPosition;
        playerToUpdate.moveInput = inputVector;
        // if (player)
    }

    //This is obsolete now because the game is constantly running
    public void ReadyRequest(NetConnection sender, bool theBool)
    {
        if (gameState == GameState.Lobby)
        {
            GetPlayer(sender).readyStatus = theBool;

            int readycount = 0;
            foreach (PlayerConnection player in playerConnections)
            {
                if (player.readyStatus == true)
                {
                    readycount++;
                }
            }
            if (readycount >= minumumPlayersToStartGame)
            {
                StartGame();
            }
        }
    }



    public void StartGame()
    {
        server.CallRPC("StartGame");
        gameState = GameState.Playing;
        tankManager.StartTheGame();
        //Put all game start functionality here
    }

    public void AssignToTeam(PlayerConnection player)//obsolete
    {
        
    }
    public void SpawnTank(PlayerConnection connection, Transform spawnLocation)//obsolete
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

    public void DemoMakeAPlayer(NetConnection sender, string newName, bool isbadguy, int playerClass)//obsolete
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
