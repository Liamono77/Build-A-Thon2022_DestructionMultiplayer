using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//ORBITER SCRIPT
//typical spinning thingy for the camera to lerp to
//WRITTEN BY LIAM SHELTON
public class OrbiterScript : MonoBehaviour
{
    public GameObject target;
    public GameObject orbitPoint;
    public GameObject looktargetAlternative;
    public GameObject teamPoint0;
    public GameObject teamPoint1;

    public float speedFactor=10f;

    public ClientGameLogic clientGameLogic;
    // Start is called before the first frame update
    void Start()
    {
        //clientGameLogic = ClientGameLogic.clientGameLogic;
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            transform.position = target.transform.position;
        }
        transform.Rotate(Vector3.up * speedFactor * Time.deltaTime);

        if (clientGameLogic.clientState == ClientGameLogic.ClientState.respawning)
        {
            if (clientGameLogic.teamID == 0)
            {
                target = teamPoint0;
            }
            if (clientGameLogic.teamID == 1)
            {
                target = teamPoint1;
            }
        }
        if (clientGameLogic.clientState == ClientGameLogic.ClientState.dying)
        {
            if (clientGameLogic.currentTank != null)
            {
                target = clientGameLogic.currentTank.gameObject;
            }
        }
    }
}
