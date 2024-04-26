using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class LimbHealth : MonoBehaviour
{
    public float Health;
    public float MaxHealth = 100;
    float PreHealth = 1;
    public float Defence;

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
    public Color FlashAmount;

    public float JumpContribution;
    public float SpeedContribution;

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
        if(Health > 0 && PreHealth <= 0)
        {
            if(PlayerAttached)
            {
                Attached.GetComponent<Player>().PlayerSpeed += SpeedContribution;
                Attached.GetComponent<Player>().JumpForce += JumpContribution;
            }
        }

        if (Health <= 0)
        {
            FlashAmount.b = 0;
            FlashAmount.g = 0;
            FlashAmount.r = 255;
            LimbSprite.color = FlashAmount;

            if (PlayerAttached)
            {
                Attached.GetComponent<Player>().PlayerSpeed -= SpeedContribution;
                Attached.GetComponent<Player>().JumpForce -= JumpContribution;
            }

            if (PlayerAttached)
            {
                Attached.GetComponent<Player>().Health -= HealthContribution;
            }
            else
            {
                if(Attached != null)
                    Attached.GetComponent<EnemyStats>().Health -= HealthContribution;
            }

            if (GetComponent<Balance>() != null)
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

        FlashAmount.r += 2 * Time.deltaTime;
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
        Hit = false;

        PreHealth = Health;
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

        if (collision.transform.CompareTag("Weapon") || collision.transform.CompareTag("Enemy Weapon") || collision.transform.CompareTag("Grab Enemy Weapon"))
        {
            if (collision.gameObject.GetComponent<item>().HurtAll)
            {
                FlashAmount.b = 0;
                FlashAmount.g = 0;

                TakeDamage(damage * collision.gameObject.GetComponent<item>().HeavyDamage - Defence);
                TakeDamage(damage * collision.gameObject.GetComponent<item>().LightDamage - Defence);
                TakeDamage(damage * collision.gameObject.GetComponent<item>().MagicDamage - Defence);

                rb.AddForce((transform.position - collision.transform.position).normalized * collision.gameObject.GetComponent<item>().Knockback);
            }
            else if (collision.gameObject.GetComponent<item>().Held)
            {
                //Sound.volume = damage * 0.005f;
                FlashAmount.b = 0;
                FlashAmount.g = 0;
                //Sound.Play();

                TakeDamage(damage * (collision.gameObject.GetComponent<item>().HeavyDamage * collision.gameObject.GetComponent<item>().HeavyDamageMod) - Defence);
                TakeDamage(damage * (collision.gameObject.GetComponent<item>().LightDamage * collision.gameObject.GetComponent<item>().LightDamageMod) - Defence);
                TakeDamage(damage * (collision.gameObject.GetComponent<item>().MagicDamage * collision.gameObject.GetComponent<item>().MagicDamageMod) - Defence);
                rb.AddForce((transform.position - collision.transform.position).normalized * collision.gameObject.GetComponent<item>().Knockback);
            }
            else if (!collision.gameObject.GetComponent<item>().Grabable)
            {
                //Sound.volume = damage * 0.005f;
                FlashAmount.b = 0;
                FlashAmount.g = 0;
                //Sound.Play();

                TakeDamage(damage * collision.gameObject.GetComponent<item>().HeavyDamage - Defence);
                TakeDamage(damage * collision.gameObject.GetComponent<item>().LightDamage - Defence);
                TakeDamage(damage * collision.gameObject.GetComponent<item>().MagicDamage - Defence);

                rb.AddForce((transform.position - collision.transform.position).normalized * collision.gameObject.GetComponent<item>().Knockback);
            }
        }
        else
        {
            if (damage > 20f || !PlayerAttached)
            {
                //Sound.volume = damage * 0.005f;
                FlashAmount.b = 0;
                FlashAmount.g = 0;
                //Sound.Play();
                if (damage * 0.15f - Defence >= 0)
                    TakeDamage(Mathf.Pow(damage * 0.15f - Defence, 2f) / 4);
            }
        }
    }
}