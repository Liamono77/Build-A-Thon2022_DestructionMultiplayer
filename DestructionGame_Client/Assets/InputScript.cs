using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//INPUT SCRIPT
//Simple script to take in player inputs and send them to the server. 
//Been a while since I've used the default input system lol. Figured it made sense since nobody's going to have a controller
//WRITTEN BY LIAM SHELTON
public class InputScript : MonoBehaviour
{
    public Vector3 thePos;
    public Vector2 moveVector;
    public GameObject indicator;

    public float inputUpdateDelay = .1f;
    public float inputUpdateTimer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        convertMouseToLook();
        GetWASD();
        if (inputUpdateTimer < Time.time)
        {
            inputUpdateTimer = Time.time + inputUpdateDelay;
            SendInputsToServer();
        }
        GetFireButton();
    }

    public void convertMouseToLook()
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("ControlPlane")))
        {
            thePos = hit.point;
        }
        indicator.SetActive(ClientGameLogic.clientGameLogic.clientState == ClientGameLogic.ClientState.playing);
        indicator.transform.position = thePos;
    }
    public void GetWASD()
    {
        moveVector.x = Input.GetAxis("Horizontal");
        moveVector.y = Input.GetAxis("Vertical");
    }

    public void GetFireButton()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ClientGameLogic.clientGameLogic.client.CallRPC("RequestFire");
        }
    }

    public void SendInputsToServer()
    {
        if (ClientGameLogic.clientGameLogic.clientState != ClientGameLogic.ClientState.notConnected)
        {
            ClientGameLogic.clientGameLogic.client.CallRPC("UpdateInputs", Lidgren.Network.NetDeliveryMethod.UnreliableSequenced, thePos.x, thePos.y, thePos.z, moveVector.x, moveVector.y);
        }
    }
}
