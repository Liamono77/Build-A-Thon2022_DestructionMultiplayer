using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lidgren.Network;
public class TankManager : MonoBehaviour
{
    public GameObject tankPrefab;
    //public List<TankScript> Team1Tanks;
    //public List<TankScript> Team2Tanks;
    // public List<>

    public TeamManager team0;
    public TeamManager team1;
    public ServerGameLogic serverLogic;
    // Start is called before the first frame update
    void Start()
    {
        serverLogic = ServerGameLogic.serverGameLogic;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartTheGame()
    {
        bool TeamToggler = false;
        foreach (PlayerConnection player in serverLogic.playerConnections)
        {
            if (TeamToggler == true)
            {
                AssignPlayerToTeam(player, 0);
            }
            if (TeamToggler == false)
            {
                AssignPlayerToTeam(player, 1);
            }
        }
    }
    public void AssignPlayerToTeam(PlayerConnection player, int TeamID)
    {
        player.teamID = TeamID;
        serverLogic.server.CallRPC("SetMyTeamID", player.connection, TeamID);
    }

    //RPC requesting to spawn
    public void SpawnRequest(NetConnection sender)
    {
        PlayerConnection senderPlayer = ServerGameLogic.serverGameLogic.GetPlayer(sender);
        if (senderPlayer.teamID == 0)
        {
            team0.SpawnTank(senderPlayer);
        }
        if (senderPlayer.teamID == 1)
        {
            team1.SpawnTank(senderPlayer);
        }
    }
}


