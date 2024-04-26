using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetRot : MonoBehaviour
{
    public Quaternion Rotation;

    void Start()
    {
        transform.parent.rotation = Rotation;
    }
}