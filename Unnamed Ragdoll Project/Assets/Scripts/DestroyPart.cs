using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyPart : MonoBehaviour
{
    ParticleSystem Self;

    void Start()
    {
        Self = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!Self.isEmitting)
        {
            Destroy(gameObject);
        }
    }
}