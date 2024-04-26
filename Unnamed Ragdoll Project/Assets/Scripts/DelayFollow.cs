using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayFollow : MonoBehaviour
{
    public Transform Target;
    public float Speed;

    // Start is called before the first frame update
    void Start()
    {
        transform.parent = null;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Target == null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Target.rotation, Speed);
            transform.position = Vector3.Lerp(transform.position, Target.position, Speed);
        }
    }
}