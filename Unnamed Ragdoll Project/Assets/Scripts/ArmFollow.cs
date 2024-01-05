using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmFollow : MonoBehaviour
{
    public float speed;
    public float offset;

    Rigidbody2D rb;
    Transform player;
    LimbHealth health;

    public HuminoidEnemyAI ConnectedAI;

    public Transform Follow;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player").GetComponent<Player>().rb.transform;
        health = GetComponent<LimbHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        if(ConnectedAI != null)
        {
            if(ConnectedAI.Pursuing)
            {
                if (health.Health <= 0)
                {
                    this.enabled = false;
                }

                Vector3 difference = Follow.position - transform.position;
                float rotationZ = Mathf.Atan2(difference.x, -difference.y) * Mathf.Rad2Deg;

                rb.MoveRotation(Mathf.LerpAngle(rb.rotation, rotationZ + offset, speed * Time.fixedDeltaTime));
            }
        }
        else
        {
            if (health.Health <= 0)
            {
                this.enabled = false;
            }

            Vector3 difference = Follow.position - transform.position;
            float rotationZ = Mathf.Atan2(difference.x, -difference.y) * Mathf.Rad2Deg;

            rb.MoveRotation(Mathf.LerpAngle(rb.rotation, rotationZ + offset, speed * Time.fixedDeltaTime));
        }
    }
}