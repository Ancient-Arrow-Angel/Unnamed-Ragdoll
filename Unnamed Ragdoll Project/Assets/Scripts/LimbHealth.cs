using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class LimbHealth : MonoBehaviour
{
    public float Health;
    public float MaxHealth = 100;
    public int Defence;

    public float HealthContribution;
    public Image LimbIcon;
    public bool PlayerAttached;
    public GameObject Attached;
    SpriteRenderer LimbSprite;

    public GameObject[] Connects;

    Rigidbody2D rb;
    Color color;
    bool Hit;

    AudioSource Sound;
    Color FlashAmount;

    public void TakeDamage(float damage)
    {
        if (damage >= 0)
        {
            Health -= damage;
            Hit = true;
        }
    }

    private void Start()
    {
        LimbSprite = GetComponent<SpriteRenderer>();
        Sound = GetComponent<AudioSource>();

        color = new Color(0, 0, 0, 255);
        rb = GetComponent<Rigidbody2D>();
        FlashAmount = new Color(255, 255, 255, 255);
        Health = MaxHealth;
    }

    static Vector2 ComputeTotalImpulse(Collision2D collision)
    {
        Vector2 impulse = Vector2.zero;

        int contactCount = collision.contactCount;
        for (int i = 0; i < contactCount; i++)
        {
            var contact = collision.GetContact(i);
            impulse += contact.normal * contact.normalImpulse;
            impulse.x += contact.tangentImpulse * contact.normal.y;
            impulse.y -= contact.tangentImpulse * contact.normal.x;
        }

        return impulse;
    }

    void Update()
    {
        FlashAmount.g += 2 * Time.deltaTime;
        FlashAmount.b += 2 * Time.deltaTime;
        LimbSprite.color = FlashAmount;

        if (Health > MaxHealth)
        {
            Health = MaxHealth;
        }

        if (Attached.GetComponent<HuminoidEnemyAI>() != null && Attached.GetComponent<HuminoidEnemyAI>().Pursuing || Attached.GetComponent<Player>() != null)
        {
            if (LimbIcon != null && Health > 0)
            {
                color = new Color((MaxHealth - Health) / MaxHealth, (Health) / MaxHealth, 0, 255);
                LimbIcon.color = color;
            }
        }
        else
        {
            if (LimbIcon != null && Health > 0)
            {
                color = new Color(0, 0, 0, 0);
                LimbIcon.color = color;
            }
        }
        if(Health <= 0)
        {
            FlashAmount.b = 0;
            FlashAmount.g = 0;
            FlashAmount.r = 255;
            FlashAmount.r = 255;
            LimbSprite.color = FlashAmount;

            if (PlayerAttached)
            {
                Attached.GetComponent<Player>().Health -= HealthContribution;
            }
            else
            {
                Attached.GetComponent<EnemyStats>().Health -= HealthContribution;
            }

            if(GetComponent<Balance>() != null)
            {
                GetComponent<Balance>().enabled = false;
            }

            if (GetComponent<Arms>() != null)
            {
                GetComponent<Arms>().enabled = false;
            }

            //if (Hit == true)
            //{
                if (GetComponent<HingeJoint2D>() != null)
                {
                    GetComponent<HingeJoint2D>().useLimits = false;
                }

                //if (GetComponent<FixedJoint2D>() != null)
                //{
                //    GetComponent<FixedJoint2D>().enabled = false;
                //}
            //}

            for (int i = 0; i < Connects.Length; i++)
            {
                Connects[i].GetComponent<LimbHealth>().Health = 0;
            }

            if (Attached.GetComponent<HuminoidEnemyAI>() != null && Attached.GetComponent<HuminoidEnemyAI>().Pursuing || Attached.GetComponent<Player>() != null)
            {
                if (LimbIcon != null)
                {
                    color = new Color(0, 0, 0, 255);
                    LimbIcon.color = color;
                }
            }
            else
            {
                if (LimbIcon != null)
                {
                    color = new Color(0, 0, 0, 0);
                    LimbIcon.color = color;
                }
            }

            this.enabled = false;
        }
        Hit = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 normal = collision.GetContact(0).normal;
        Vector2 impulse = ComputeTotalImpulse(collision);

        // Both bodies see the same impulse. Flip it for one of the bodies.
        if (Vector3.Dot(normal, impulse) < 0f)
            impulse *= -1f;

        Vector2 myIncidentVelocity = rb.velocity - impulse / rb.mass;

        Vector2 otherIncidentVelocity = Vector3.zero;
        var otherBody = collision.rigidbody;
        if (otherBody != null)
        {
            otherIncidentVelocity = otherBody.velocity;
            if (!otherBody.isKinematic)
                otherIncidentVelocity += impulse / otherBody.mass;
        }

        // Compute how fast each one was moving along the collision normal,
        // Or zero if we were moving against the normal.
        float myApproach = Mathf.Max(0f, Vector3.Dot(myIncidentVelocity, normal));
        float otherApproach = Mathf.Max(0f, Vector3.Dot(otherIncidentVelocity, normal));

        float damage = Mathf.Max(0f, otherApproach - myApproach - 1);

        //Sound.volume = damage * 0.005f;
        FlashAmount.b = 0;
        FlashAmount.g = 0;
        //Sound.Play();

        if (collision.transform.CompareTag("Weapon"))
        {
            TakeDamage(damage * collision.gameObject.GetComponent<item>().Damage - Defence);

            if(collision.gameObject.GetComponent<item>().Held)
                rb.AddForce((transform.position - collision.transform.position).normalized * collision.gameObject.GetComponent<item>().Knockback);
        }
        else
        {
            TakeDamage(damage * 0.5f - Defence);
        }
    }
}