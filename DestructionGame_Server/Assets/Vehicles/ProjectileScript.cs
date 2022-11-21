using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : DestructionNetSync
{
    public GameObject ContactFX;
    public float speedFactor = 10f;
    public float duration = 1f;
    public int networkIDOfShooter = -1;

    public float projectileDamage = 10f;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Destroy(gameObject, duration);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        transform.Translate(Vector3.forward * Time.deltaTime * speedFactor, Space.Self);
    }

    public void OnTriggerEnter(Collider other)
    {
        //if (other.gameObject.tag == "")
        GameObject.Instantiate(ContactFX, gameObject.transform.position, gameObject.transform.rotation);

        if (other.gameObject.tag == "TankHurtbox")
        {
            TankScript tankToDamage = other.gameObject.GetComponentInParent<TankScript>();
            tankToDamage.TakeDamage(projectileDamage, networkIDOfShooter);
        }

        Destroy(gameObject);
    }
}
