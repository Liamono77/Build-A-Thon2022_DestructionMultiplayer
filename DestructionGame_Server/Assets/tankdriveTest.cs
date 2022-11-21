using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//TANK DRIVE TEST
//This is a prototyping space for the tank control scheme. Good for testing wheel colliders. no longer used but could be useful in the future
//WRITTEN BY LIAM SHELTON
public class tankdriveTest : MonoBehaviour
{
    public List<WheelCollider> leftWheels = new List<WheelCollider>();
    public List<WheelCollider> rightWheels = new List<WheelCollider>();

    public Vector2 inputVector;

    public float wheelMultiplier = 2000;
    public float breakTorque = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        inputVector.x = Input.GetAxisRaw("Horizontal");
        inputVector.y = Input.GetAxisRaw("Vertical");
        //inputVector = inputVector.normalized;
        if (inputVector.magnitude > 1)
        {
            inputVector = inputVector.normalized;
        }

        if (inputVector.y >= 0f)
        {

        }
        //if (Input.GetAxis("Horizontal"))
        //leftDrive = Input.GetAxis("Horizontal");
        //rightDrive = Innput.GetAxis("Vertical")
        foreach (WheelCollider wheel in leftWheels)
        {
            wheel.motorTorque = (inputVector.y + inputVector.x) * wheelMultiplier;
            wheel.brakeTorque = breakTorque;
        }
        foreach (WheelCollider wheel in rightWheels)
        {
            wheel.motorTorque = (inputVector.y - inputVector.x) * wheelMultiplier;
            wheel.brakeTorque = breakTorque;
        }
    }
}
