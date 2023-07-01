using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lidgren.Network;

public class ListTransmitterPrototype : MonoBehaviour
{
    public List<Quaternion> theList = new List<Quaternion>();
    public bool testButton;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (testButton)
        {
            sendList();
            testButton = false;
        }
    }

    public void sendList()
    {
        ServerGameLogic.serverGameLogic.server.CallRPC("receiveList", theList);
    }
    public void receiveList(NetConnection sender, List<Quaternion> listOfPositions)
    {
        theList = listOfPositions;
    }
}
