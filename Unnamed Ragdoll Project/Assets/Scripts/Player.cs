using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Animator anim;
    public Rigidbody2D rb;
    public float JumpForce;
    public float PlayerSpeed;
    public Vector2 JumpHeight;
    bool IsGrounded;
    public float PositionRadius;
    public LayerMask Ground;
    public Transform PlayerPos;

    float speed1 = 0;
    float speed2 = 0;


    public float Health = 100;
    //public Healthbar healthbar;


    //public GameObject LeftArm;
    //public GameObject RightArm;
    //public GameObject LeftHand;
    //public GameObject RightHand;

    public GameObject LeftLeg;
    public GameObject RightLeg;
    public GameObject LeftFoot;
    public GameObject RightFoot;

    //public GameObject Hips;

    public Collider2D[] colliders;

    public void TakeDamage(float damage)
    {
        //healthbar.SetHealth(Health);
        if (damage > 0)
        {
            Health -= damage;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        colliders = transform.GetComponentsInChildren<Collider2D>();
        for (int i = 0; i < colliders.Length; i++)
        {
            for (int k = i + 1; k < colliders.Length; k++)
            {
                Physics2D.IgnoreCollision(colliders[i], colliders[k]);
            }
        }

        //healthbar.SetMaxHealth(Health);
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.lockState = CursorLockMode.Confined;

        speed2 = speed1;
        speed1 = rb.velocity.magnitude;

        if (speed1 - speed2 < -12)
        {
            TakeDamage(-(speed1 - speed2) * 5);
        }

        //if (Health <= 0)
        //{
        //    this.GetComponent<Player>().enabled = false;
        //    LeftArm.GetComponent<Arms>().enabled = false;
        //    RightArm.GetComponent<Arms>().enabled = false;

        //    LeftHand.GetComponent<Arms>().enabled = false;
        //    RightHand.GetComponent<Arms>().enabled = false;


        //    LeftLeg.GetComponent<Balance>().enabled = false;
        //    RightLeg.GetComponent<Balance>().enabled = false;

        //    LeftFoot.GetComponent<Balance>().enabled = false;
        //    RightFoot.GetComponent<Balance>().enabled = false;


        //    Hips.GetComponent<Balance>().enabled = false;
        //}

        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            if (Input.GetAxisRaw("Horizontal") > 0)
            {
                anim.Play("WalkForward");
                rb.AddForce(Vector2.right * PlayerSpeed * Time.deltaTime);
            }
            else
            {
                anim.Play("WalkBackward");
                rb.AddForce(Vector2.left * PlayerSpeed * Time.deltaTime);
            }
        }
        else
        {
            anim.Play("Idle");
        }

        if(Input.GetKey(KeyCode.S))
        {
            rb.AddForce(Vector2.down * PlayerSpeed * Time.deltaTime);
            LeftLeg.GetComponent<Balance>().force = 100;
            RightLeg.GetComponent<Balance>().force = 100;

            LeftFoot.GetComponent<Balance>().force = 100;
            RightFoot.GetComponent<Balance>().force = 100;


            LeftLeg.GetComponent<HingeJoint2D>().useLimits = false;
            RightLeg.GetComponent<HingeJoint2D>().useLimits = false;

            LeftFoot.GetComponent<HingeJoint2D>().useLimits = false;
            RightFoot.GetComponent<HingeJoint2D>().useLimits = false;
        }
        else
        {
            LeftLeg.GetComponent<Balance>().force = 200;
            RightLeg.GetComponent<Balance>().force = 200;

            LeftFoot.GetComponent<Balance>().force = 200;
            RightFoot.GetComponent<Balance>().force = 200;


            LeftLeg.GetComponent<HingeJoint2D>().useLimits = true;
            RightLeg.GetComponent<HingeJoint2D>().useLimits = true;

            LeftFoot.GetComponent<HingeJoint2D>().useLimits = true;
            RightFoot.GetComponent<HingeJoint2D>().useLimits = true;
        }

        IsGrounded = Physics2D.OverlapCircle(PlayerPos.position, PositionRadius, Ground);
        if (IsGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector2.up * JumpForce);
        }
    }
}