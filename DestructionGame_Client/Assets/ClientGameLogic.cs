using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lidgren.Network;
using UnityEngine.UI;

//CLIENT GAME LOGIC
//This is where much of the logic specific to the game is located (but not all of it. This was for a buildathon)
//WRITTEN BY LIAM SHELTON
public class ClientGameLogic : MonoBehaviour
{
    public static ClientGameLogic clientGameLogic;

    public InputField inputFieldAdd; //Ignore this
    public InputField inputFieldPort; //Ignore this

    //The UI screens
    public GameObject connectScreen;
    public GameObject connectingScreen;
    public GameObject lobbyScreen;
    public GameObject spawnScreen;


    public NetSyncManager netSyncManager; 

    public DestructionClient client;

    //public 

    public bool testBool;

    public ClientState clientState;
    public enum ClientState
    {
        notConnected,
        connecting,
        Lobby,
        CountDown,
        dying,
        respawning,
        playing,
    }

    public int teamID;

    public DestructionNetSyncClient currentTank;
    //public 


    // Start is called before the first frame update
    void Start()
    {
        clientGameLogic = this;
    }

    public void RespawnButton()//The button that triggers this method should cause the server to spawn a new tank for this client
    {
        client.CallRPC("SpawnRequest");
    }

    public void ConnectButton() //Misnomer; this actually just sends the RPC 
    {
        client.CallRPC("ConnectionRequest", inputFieldAdd.text);
        clientState = ClientState.connecting;
    }
    public void ReadyButton(Toggle theTog)
    {
        client.CallRPC("ReadyRequest", theTog.isOn);
    }
    public void QuitButton()
    {
        Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {
        connectScreen.SetActive(clientState == ClientState.notConnected);
        connectingScreen.SetActive(clientState == ClientState.connecting);
        lobbyScreen.SetActive(clientState == ClientState.Lobby);
        spawnScreen.SetActive(clientState == ClientState.respawning);


        //quick logic for monitoring tank health
        if (clientState == ClientState.playing)
        {
            if (currentTank.healthCurrent <= 0)
            {
                clientState = ClientState.dying;
            }
        }
        if (clientState == ClientState.dying)
        {
            if (currentTank == null)
            {
                clientState = ClientState.respawning;
            }
        }

    }

    //RPC to start a game
    public void StartGame(NetConnection serverConnection)
    {
        clientState = ClientState.respawning;

    }

    //RPC from server to assign this client to a tank
    public void SetCurrentTank(NetConnection serverConnection, int networkID)
    {
        currentTank = netSyncManager.GetNetSync(networkID);
        if (currentTank != null)
        {
            clientState = ClientState.playing;
        }
        else
        {
            Debug.LogError($"Failed to set current tank from remote procedure call. Was not able to locate tank of ID {networkID}");
        }
    }

    //RPC to set team
    public void SetMyTeamID(NetConnection serverConnection, int ateamID)
    {
        teamID = ateamID;
        Debug.Log($"Assigned to team {teamID}");
    }

    //RPC from the server when connection is successful
    public void HandshakeFromServer(NetConnection serverconnection, string GameState)//, int team)
    {
        //teamID = team;
        if (GameState == "Lobby")
        {
            clientState = ClientState.Lobby;
        }
        else if(GameState == "CountDown")
        {
            clientState = ClientState.CountDown;
        }
        else if (GameState == "Playing")
        {
            clientState = ClientState.respawning;
        }
        else if (GameState == "GameOver")
        {
            Debug.LogWarning("Joined when game was starting");
            clientState = ClientState.Lobby;
        }
        else
        {
            Debug.LogError($"Failed to parse game state {GameState}!");
        }
    }

    //RPC from server to check if client is still connected. If this doesn't get called, the server will probably kick the client
    public void ConnectionCheck(NetConnection serverConnection)
    {
        Debug.LogWarning("Server has requested connection check. Sending response...");
        client.CallRPC("ConnectionCheckResponse");
    }
}
