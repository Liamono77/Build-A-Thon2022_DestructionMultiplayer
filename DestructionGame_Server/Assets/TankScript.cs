using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankScript : MonoBehaviour
{
    public PlayerConnection myConnection;

    public Transform myTurret;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FaceCursor()
    {
        Vector3 lookDir = myConnection.cursorPosition - gameObject.transform.position;
        float angle = Mathf.Atan2(lookDir.x, lookDir.y) * Mathf.Rad2Deg;
        Quaternion theQuat = Quaternion.Euler(0, 0, -angle);
        myTurret.rotation = theQuat;
    }
}
