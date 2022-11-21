using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//HEALTH INDICATOR
//It aint no game if there aint no health bars
//Also has name tag functionality!
//WRITTEN BY LIAM SHELTON
public class HealthIndicator : MonoBehaviour
{
    public Slider healthBarMain;
    public Slider healthBarLerp;
    public float lerpFactor = 1f;
    public Text nameText;

    public DestructionNetSyncClient myNetSync;


    // Start is called before the first frame update
    void Start()
    {
       // myNetSync = gameObject.GetComponentInParent<DestructionNetSyncClient>();

    }

    // Update is called once per frame
    void Update()
    {
        if (myNetSync != null)
        {
           // healthBarMain.maxValue = myNetSync.healthMax;
            healthBarMain.value = myNetSync.healthCurrent;

            //healthBarLerp.maxValue = myNetSync.healthMax;
            healthBarLerp.value = Mathf.Lerp(healthBarLerp.value, healthBarMain.value, Time.deltaTime * lerpFactor);

            if (nameText.text != myNetSync.gameObject.name)
            {
                Debug.LogWarning($"nametag adjusted from {nameText.text} to {myNetSync.gameObject.name}");
                nameText.text = myNetSync.gameObject.name;
            }
        }
        else
        {
            myNetSync = gameObject.GetComponentInParent<DestructionNetSyncClient>();
            healthBarMain.maxValue = myNetSync.healthMax;
            healthBarLerp.maxValue = myNetSync.healthMax;
            nameText.text = myNetSync.gameObject.name;
        }

    }
}
