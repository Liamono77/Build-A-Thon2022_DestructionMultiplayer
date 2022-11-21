using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//EXPLOSION FX
//A simple extension to the DestructionNetSync to easily allow explosion effects to be set up
//WRITTEN BY LIAM SHELTON
public class ExplosionFX : DestructionNetSync
{
    public float timeBeforeDestroy = .25f;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Destroy(gameObject, timeBeforeDestroy);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
