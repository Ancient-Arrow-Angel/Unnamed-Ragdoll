using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashTo : MonoBehaviour
{
    public float Force;

    void Start()
    {
        transform.parent.GetComponent<GetWeapon>().Creator.transform.GetComponent<Rigidbody2D>().velocity = ((transform.position - transform.parent.GetComponent<GetWeapon>().Creator.transform.GetComponent<Rigidbody2D>().transform.position).normalized * Force);
        GameObject.Find("Player").GetComponent<Player>().rb.velocity = ((transform.position - GameObject.Find("Player").GetComponent<Player>().rb.transform.position).normalized * Force);
    }
}