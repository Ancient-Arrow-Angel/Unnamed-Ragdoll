using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    public float DestroyTime;

    void Start()
    {
        Destroy(this.gameObject, DestroyTime);
    }
}