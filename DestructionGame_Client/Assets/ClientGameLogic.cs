using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lidgren.Network;
using UnityEngine.UI;
public class ClientGameLogic : MonoBehaviour
{
    public static ClientGameLogic clientGameLogic;

    public InputField inputFieldAdd;
    public InputField inputFieldPort;

    public GameObject connectScreen;
    public GameObject connectingScreen;
    public GameObject lobbyScreen;
    //public 

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
        respawning,

    }

    public int teamID;
    //public 


    // Start is called before the first frame update
    void Start()
    {
        clientGameLogic = this;
    }

    public void RespawnButton()
    {

    }

    public void ConnectButton()
    {
        // client.InitializeClientNet(inputFieldAdd.text, int.Parse(inputFieldPort.text));
        client.CallRPC("ConnectionRequest", inputFieldAdd.text);
        clientState = ClientState.connecting;
    }
    public void ReadyButton(Toggle theTog)
    {
        client.CallRPC("ReadyRequest", theTog.isOn);
    }

    // Update is called once per frame
    void Update()
    {
        connectScreen.SetActive(clientState == ClientState.notConnected);
        connectingScreen.SetActive(clientState == ClientState.connecting);
        lobbyScreen.SetActive(clientState == ClientState.Lobby);

        //connec
        if (testBool)
        {
            testBool = false;
            client.CallRPC("DemoMakeAPlayer", "Johnson", false, 2);
            client.CallRPC("DemoMakeAPlayer", "Hector", true, 1);

        }


    }

    //RPC to start a game
    public void StartGame(NetConnection serverConnection)
    {
        clientState = ClientState.respawning;

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
}
