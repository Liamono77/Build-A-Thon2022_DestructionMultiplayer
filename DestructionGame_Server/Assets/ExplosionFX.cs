using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
