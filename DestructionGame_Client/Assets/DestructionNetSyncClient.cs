using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructionNetSyncClient : MonoBehaviour
{
    public int networkID;

    public Vector3 latestPosition;
    public Quaternion latestRotation;

    public float lerpFactor = 2000f;

    public ObjectType objectType;
    public enum ObjectType
    {
        stationary,
        tank,
        projectile
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (objectType == ObjectType.tank)
        {
           // transform.position = Vector3.Lerp(transform.position, latestPosition, Time.deltaTime * lerpFactor);
          //  transform.rotation = Quaternion.Lerp(transform.rotation, latestRotation, Time.deltaTime * lerpFactor);

            transform.position = Vector3.MoveTowards(transform.position, latestPosition, Time.deltaTime * lerpFactor);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, latestRotation, Time.deltaTime * lerpFactor);
        }
    }
}
