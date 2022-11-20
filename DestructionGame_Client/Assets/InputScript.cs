using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputScript : MonoBehaviour
{
    public Vector3 thePos;
    public Vector2 moveVector;
    public GameObject indicator;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        convertMouseToLook();
        GetWASD();
    }

    public void convertMouseToLook()
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("ControlPlane")))
        {
            thePos = hit.point;
        }
        indicator.transform.position = thePos;
    }
    public void GetWASD()
    {
        moveVector.x = Input.GetAxis("Vertical");
        moveVector.y = Input.GetAxis("Horizontal");
    }

    public void SendInputsToServer()
    {
        ClientGameLogic.clientGameLogic.client.CallRPC("UpdateInputs", thePos.x, thePos.y, thePos.z, moveVector.x, moveVector.y);
    }
}
