using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public GameObject lookTarget;
    public Transform lookLerpTarget;
    //public Transform orbitTarget;
    public OrbiterScript orbiter;
    public Vector3 followOffset;

    public float moveLerpFactor = 1f;
    public float lookLerpFactor = 1f;

    public float orbitLerpFactor = 10f;
    //public Vector3 orbitOffset;

    public GameObject coolVisuals;

    public void Update()
    {
        if (ClientGameLogic.clientGameLogic.clientState == ClientGameLogic.ClientState.playing)
        {
            coolVisuals.SetActive(false);
            PlayMode();
        }
        else if (ClientGameLogic.clientGameLogic.clientState == ClientGameLogic.ClientState.respawning)
        {
            RespawnMode();
            coolVisuals.SetActive(false);
        }
        else
        {
            OrbitMode();
            coolVisuals.SetActive(true);

        }

        //if ()
    }
    public void PlayMode()
    {
        if (ClientGameLogic.clientGameLogic.currentTank != null)
        {
            lookTarget = ClientGameLogic.clientGameLogic.currentTank.gameObject;
            transform.position = Vector3.Lerp(transform.position, lookTarget.transform.position + followOffset, Time.deltaTime * moveLerpFactor);
            lookLerpTarget.transform.position = Vector3.Lerp(lookLerpTarget.transform.position, lookTarget.transform.position, Time.deltaTime * lookLerpFactor);
            transform.LookAt(lookLerpTarget);
        }
    }
    public void OrbitMode()
    {
    //    if (ClientGameLogic.clientGameLogic.currentTank != null)
     //   {
            lookTarget = orbiter.looktargetAlternative;
            transform.position = Vector3.Lerp(transform.position, orbiter.orbitPoint.transform.position, Time.deltaTime * orbitLerpFactor);
            lookLerpTarget.transform.position = Vector3.Lerp(lookLerpTarget.transform.position, lookTarget.transform.position, Time.deltaTime * lookLerpFactor);
            transform.LookAt(lookLerpTarget);
     //   }
    }

    public void RespawnMode()
    {
        //    if (ClientGameLogic.clientGameLogic.currentTank != null)
        //   {
        lookTarget = orbiter.gameObject;
        transform.position = Vector3.Lerp(transform.position, orbiter.orbitPoint.transform.position, Time.deltaTime * orbitLerpFactor);
        lookLerpTarget.transform.position = Vector3.Lerp(lookLerpTarget.transform.position, lookTarget.transform.position, Time.deltaTime * lookLerpFactor);
        transform.LookAt(lookLerpTarget);
        //   }
    }
}
