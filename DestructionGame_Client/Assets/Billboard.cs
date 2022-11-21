using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//BILLBOARD
//typical Unity billbord. Can't wait for them to make a built-in form of this lol
//WRITTEN BY LIAM SHELTON
public class Billboard : MonoBehaviour
{
    private void FixedUpdate()
    {
        transform.LookAt(Camera.main.transform);
    }
}
