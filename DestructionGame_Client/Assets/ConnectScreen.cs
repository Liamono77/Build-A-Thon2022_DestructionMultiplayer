using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//CONNECT SCREEN
//This is a quickly set up screen to allow clients to specify an address and port.
//Might come in handy if I cant figure out port forwarding in time for the closing ceremony
//WRITTEN BY LIAM SHELTON

public class ConnectScreen : MonoBehaviour
{
    public DestructionClient client;
    public InputField addressField;
    public InputField portField;

    public GameObject connectScreen;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ConnectButton()
    {
        client.address = addressField.text;
        client.port = System.Int32.Parse(portField.text);
        client.gameObject.SetActive(true);
        connectScreen.SetActive(false);
    }
}
