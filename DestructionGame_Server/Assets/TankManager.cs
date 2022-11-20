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
       // bool TeamToggler = false;
        foreach (PlayerConnection player in serverLogic.playerConnections)
        {
            AssignPlayerToTeam(player);//, 12);
      //      if (TeamToggler == true)
      //      {
     //           AssignPlayerToTeam(player, 0);
     //       }
     //       if (TeamToggler == false)
    //        {
    //            AssignPlayerToTeam(player, 1);
   //         }
        }
    }
    public void AssignPlayerToTeam(PlayerConnection player)//, int TeamID)
    {
        int team0Count = 0;
        int team1Count = 0;
        foreach (PlayerConnection play in serverLogic.playerConnections)
        {
            Debug.Log($"checking team of player of ID {player.connection.RemoteUniqueIdentifier}...");
            if (play.teamID == 0)
            {
                team0Count++;
            }
            if (play.teamID == 1)
            {
                team1Count++;
            }
        }

        if (team0Count >= team1Count)
        {
            player.teamID = 1;
            serverLogic.server.CallRPC("SetMyTeamID", player.connection, 1);
        }
        if (team1Count > team0Count)
        {
            player.teamID = 0;
            serverLogic.server.CallRPC("SetMyTeamID", player.connection, player.teamID);

            //serverLogic.server.CallRPC("SetMyTeamID", player.connection, 1);

        }


        //  player.teamID = TeamID;
        // serverLogic.server.CallRPC("SetMyTeamID", player.connection, TeamID);
    }

    //RPC requesting to spawn
    public void SpawnRequest(NetConnection sender)
    {
        PlayerConnection senderPlayer = ServerGameLogic.serverGameLogic.GetPlayer(sender);
        if (senderPlayer.teamID == 0)
        {
            team0.SpawnTank(senderPlayer);
            //serverLogic.server.CallRPC("SetCurrentTank", )
        }
        if (senderPlayer.teamID == 1)
        {
            team1.SpawnTank(senderPlayer);
        }
    }
}


