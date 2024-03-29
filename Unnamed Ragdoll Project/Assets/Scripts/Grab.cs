using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grab : MonoBehaviour
{
    public float Grip;
    public bool hold;
    public KeyCode mouseButton;
    public bool LeftHand;
    public bool Active = true;
    public Player player;

    public int HeldID;
    public int HeldAmount;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0) && LeftHand && player.LeftID == 0 && !Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.Mouse1) && LeftHand == false && player.RightID == 0 && !Input.GetKey(KeyCode.E))
        {
            hold = true;
        }
        else
        {
            hold = false;
            Destroy(GetComponent<FixedJoint2D>());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (Active && hold && collision.gameObject.tag == "Grab" || collision.transform.CompareTag("Grab Enemy Weapon") && Active && hold)
        {
            Rigidbody2D rb = collision.transform.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                FixedJoint2D fj = transform.gameObject.AddComponent(typeof(FixedJoint2D)) as FixedJoint2D;
                fj.connectedBody = rb;
                fj.breakForce = Grip;
            }
            else
            {
                FixedJoint2D fj = transform.gameObject.AddComponent(typeof(FixedJoint2D)) as FixedJoint2D;
                fj.breakForce = Grip;
            }
        }
    }
}