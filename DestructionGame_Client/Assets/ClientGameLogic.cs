using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lidgren.Network;

public class ClientGameLogic : MonoBehaviour
{
    public DestructionClient client;

    public bool testBool;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (testBool)
        {
            testBool = false;
            client.CallRPC("DemoMakeAPlayer", "Johnson", false, 2);
            client.CallRPC("DemoMakeAPlayer", "Hector", true, 1);

        }
    }
}
