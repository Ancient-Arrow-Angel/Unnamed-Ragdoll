using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Levatate : MonoBehaviour
{
    public float Force;

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") ||
            other.CompareTag("Grab") ||
            other.CompareTag("Enemy Weapon") ||
            other.CompareTag("Grab Enemy Weapon"))
        {
            other.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, Force));
        }
    }
}