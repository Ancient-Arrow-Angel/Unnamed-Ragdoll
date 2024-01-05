using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Speed;

    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = transform.up * Speed;
    }
}
