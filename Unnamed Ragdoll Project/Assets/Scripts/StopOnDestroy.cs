using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopOnDestroy : MonoBehaviour
{
    public Transform Target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Target == null)
        {
            GetComponent<ParticleSystem>().Stop();
            Destroy(this);
        }
    }
}
