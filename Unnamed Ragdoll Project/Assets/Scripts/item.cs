using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class item : MonoBehaviour
{
    public int ItemID;
    bool Held = false;
    bool LeftHand = true;

    GameObject SettingsOB;
    settings SettingsS;

    Grab HandS;

    Rigidbody2D rb;
    Collider2D Coll;
    Collider2D Coll2;

    Camera cam;
    float offset = 180;
    int speed = 50;

    GameObject PlayerOB;
    Player PlayerS;

    bool InGround;


    void NoColl(bool Active)
    {
        Collider2D[] Colliders = PlayerOB.transform.GetComponentsInChildren<Collider2D>();
        for (int i = 0; i < Colliders.Length; i++)
        {
            for (int k = i + 1; k < Colliders.Length; k++)
            {
                Physics2D.IgnoreCollision(Colliders[i], Coll, Active);
                if(Coll2 != null)
                    Physics2D.IgnoreCollision(Colliders[i], Coll2, Active);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SettingsOB = GameObject.Find("Global Settings");
        SettingsS = SettingsOB.GetComponent<settings>();
        rb = GetComponent<Rigidbody2D>();
        Coll = GetComponent<Collider2D>();
        Coll2 = GetComponent<Collider2D>();
        cam = Camera.main;
        PlayerOB = GameObject.Find("Player");
        PlayerS = PlayerOB.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        InGround = Physics2D.OverlapCircle(transform.position, 0.05f, PlayerS.Ground);

        if (InGround)
        {
            Coll.isTrigger = true;
        }
        else
        {
            Coll.isTrigger = false;
        }

        if (Input.GetKey(KeyCode.Q) && Held == true)
        {
            if (Input.GetKeyUp(KeyCode.Mouse0) && LeftHand)
            {
                Destroy(GetComponent<FixedJoint2D>());
                HandS.Active = true;
                Held = false;
                NoColl(false);
            }
            else if (Input.GetKeyUp(KeyCode.Mouse1) && LeftHand == false)
            {
                Destroy(GetComponent<FixedJoint2D>());
                HandS.Active = true;
                Held = false;
                NoColl(false);
            }
        }

        Vector3 playerpos = new Vector3(cam.ScreenToWorldPoint(Input.mousePosition).x, cam.ScreenToWorldPoint(Input.mousePosition).y, 0);
        Vector3 difference = playerpos - transform.position;
        float rotationZ = Mathf.Atan2(difference.x, -difference.y) * Mathf.Rad2Deg;

        if (Input.GetKey(KeyCode.Mouse0) && LeftHand == true && Held)
        {
            rb.MoveRotation(Mathf.LerpAngle(rb.rotation, rotationZ + offset, speed * Time.fixedDeltaTime));
        }
        else if (Input.GetKey(KeyCode.Mouse1) && LeftHand == false && Held)
        {
            rb.MoveRotation(Mathf.LerpAngle(rb.rotation, rotationZ + offset, speed * Time.fixedDeltaTime));
        }

        if (LeftHand)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (LeftHand == false)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Left Hand" && Input.GetKey(KeyCode.Mouse0) && Held == false)
        {
            HandS = collision.gameObject.GetComponent<Grab>();
            if (HandS != null && HandS.Active)
            {
                NoColl(true);

                Rigidbody2D rb2 = collision.transform.GetComponent<Rigidbody2D>();
                FixedJoint2D fj = transform.gameObject.AddComponent(typeof(FixedJoint2D)) as FixedJoint2D;
                fj.connectedBody = rb2;
                Held = true;
                LeftHand = true;
                HandS.Active = false;

                transform.position = collision.gameObject.transform.position;
            }
        }
        else if (collision.gameObject.tag == "Right Hand" && Input.GetKey(KeyCode.Mouse1) && Held == false)
        {
            HandS = collision.gameObject.GetComponent<Grab>();
            if(HandS != null && HandS.Active)
            {
                NoColl(true);
            
                Rigidbody2D rb2 = collision.transform.GetComponent<Rigidbody2D>();
                FixedJoint2D fj = transform.gameObject.AddComponent(typeof(FixedJoint2D)) as FixedJoint2D;
                fj.connectedBody = rb2;
                Held = true;
                LeftHand = false;
                HandS.Active = false;

                transform.position = collision.gameObject.transform.position;
            }
        }
    }
}