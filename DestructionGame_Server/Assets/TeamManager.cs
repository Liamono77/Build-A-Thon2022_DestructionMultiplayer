using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour
{
    public List<Transform> spawnLocations = new List<Transform>();
    public List<TankScript> tankScripts = new List<TankScript>();
    public List<BuildingScript> buildings = new List<BuildingScript>();

    public GameObject playerTankPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public Transform randomSpawnPoint()
    {
        int randomInt = Random.Range(0, spawnLocations.Count);
        return spawnLocations[randomInt];
    }
    public void SpawnTank(PlayerConnection connection)
    {
        if (connection.currentTank == null)
        {
            Transform spawnLocation = randomSpawnPoint();
            TankScript newTank = GameObject.Instantiate(playerTankPrefab, spawnLocation.position, spawnLocation.rotation).GetComponent<TankScript>();
            newTank.myConnection = connection;
            connection.currentTank = newTank;

            //ServerGameLogic.serverGameLogic.server.CallRPC("")
            //return newTank;
            tankScripts.Add(newTank);
        }
        else
        {
            Debug.LogWarning("Player has attempted to spawn a tank but already has one");
        }
    }
}
