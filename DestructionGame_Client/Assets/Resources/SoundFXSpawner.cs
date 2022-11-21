using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFXSpawner : MonoBehaviour
{
    public GameObject soundToSpawn;
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(soundToSpawn, transform.position, transform.rotation);
    }
}
