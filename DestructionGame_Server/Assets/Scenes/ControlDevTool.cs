using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlDevTool : MonoBehaviour
{
    public GameObject tankPrefab;

    public GameObject moveIndicator;
    public GameObject cursorIndicator;

    public PlayerConnection playerConnection = new PlayerConnection();

    public TankScript testTank;
    // Start is called before the first frame update
    void Start()
    {
        testTank = GameObject.Instantiate(tankPrefab, transform.position, transform.rotation).GetComponent<TankScript>();
        testTank.myConnection = playerConnection;
        playerConnection.currentTank = testTank;
    }

    // Update is called once per frame
    void Update()
    {
        moveIndicator.transform.position = testTank.myConnection.moveInput;
        cursorIndicator.transform.position = testTank.myConnection.cursorPosition;
    }
}
