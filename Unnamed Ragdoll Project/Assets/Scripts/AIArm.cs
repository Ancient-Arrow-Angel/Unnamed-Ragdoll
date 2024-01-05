using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIArm : MonoBehaviour
{
    public float speed;
    public float OffsetSpeed;
    public float offset;
    float CurrentOffset;

    public float MinBoostCooldown;
    public float MaxBoostCooldown;
    public float BoostAmount;
    float BoostTimer;

    public HuminoidEnemyAI ConnectedAI;

    Rigidbody2D rb;
    Transform player;
    LimbHealth health;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player").GetComponent<Player>().rb.transform;
        health = GetComponent<LimbHealth>();
        CurrentOffset = offset;
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

                BoostTimer -= Time.deltaTime;

                if (BoostTimer <= 0)
                {
                    CurrentOffset = offset;
                    rb.AddForce((player.position - rb.transform.position).normalized * BoostAmount, ForceMode2D.Impulse);
                    BoostTimer = Random.Range(MinBoostCooldown, MaxBoostCooldown);
                }

                if (transform.position.x > player.position.x)
                    CurrentOffset += Time.deltaTime * OffsetSpeed;
                else
                    CurrentOffset -= Time.deltaTime * OffsetSpeed;

                Vector3 playerpos = player.position;
                Vector3 difference = playerpos - transform.position;
                float rotationZ = Mathf.Atan2(difference.x, -difference.y) * Mathf.Rad2Deg;

                rb.MoveRotation(Mathf.LerpAngle(rb.rotation, rotationZ + CurrentOffset, speed * Time.fixedDeltaTime));
            }
        }
        else
        {
            if (health.Health <= 0)
            {
                this.enabled = false;
            }

            BoostTimer -= Time.deltaTime;

            if (BoostTimer <= 0)
            {
                CurrentOffset = offset;
                rb.AddForce((player.position - rb.transform.position).normalized * BoostAmount, ForceMode2D.Impulse);
                BoostTimer = Random.Range(MinBoostCooldown, MaxBoostCooldown);
            }

            if (transform.position.x > player.position.x)
                CurrentOffset += Time.deltaTime * OffsetSpeed;
            else
                CurrentOffset -= Time.deltaTime * OffsetSpeed;

            Vector3 playerpos = player.position;
            Vector3 difference = playerpos - transform.position;
            float rotationZ = Mathf.Atan2(difference.x, -difference.y) * Mathf.Rad2Deg;

            rb.MoveRotation(Mathf.LerpAngle(rb.rotation, rotationZ + CurrentOffset, speed * Time.fixedDeltaTime));
        }
    }
}