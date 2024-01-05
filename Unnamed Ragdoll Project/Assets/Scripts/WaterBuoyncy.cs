using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class WaterBuoyncy : MonoBehaviour
{
    public bool InWater;
    public float Buoyncy = 1;
    public float WaterDrag;
    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if (InWater)
        {
            rb.AddForce(new Vector2(0, 2000 * Buoyncy * Time.deltaTime));
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Water")
        {
            InWater = true;
            rb.drag += WaterDrag;
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Water")
        {
            InWater = false;
            rb.drag -= WaterDrag;
        }
    }
}