using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageInput : MonoBehaviour
{
    public float Defence;
    public EnemyStats Attached;

    Rigidbody2D rb;
    AudioSource Sound;

    public void TakeDamage(float damage)
    {
        if (damage >= 0)
        {
            Attached.Health -= damage;
        }
    }

    private void Start()
    {
        Sound = GetComponent<AudioSource>();

        rb = GetComponent<Rigidbody2D>();
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

        if (collision.transform.CompareTag("Weapon"))
        {
            if (collision.gameObject.GetComponent<item>().Held)
            {
                TakeDamage(damage * (collision.gameObject.GetComponent<item>().HeavyDamage * collision.gameObject.GetComponent<item>().HeavyDamageMod) - Defence);
                TakeDamage(damage * (collision.gameObject.GetComponent<item>().LightDamage * collision.gameObject.GetComponent<item>().LightDamageMod) - Defence);
                TakeDamage(damage * (collision.gameObject.GetComponent<item>().MagicDamage * collision.gameObject.GetComponent<item>().MagicDamageMod) - Defence);

                rb.AddForce((transform.position - collision.transform.position).normalized * collision.gameObject.GetComponent<item>().Knockback);
            }
            else if (!collision.gameObject.GetComponent<item>().Grabable)
            {
                TakeDamage(damage * collision.gameObject.GetComponent<item>().HeavyDamage - Defence);
                TakeDamage(damage * collision.gameObject.GetComponent<item>().LightDamage - Defence);
                TakeDamage(damage * collision.gameObject.GetComponent<item>().MagicDamage - Defence);

                rb.AddForce((transform.position - collision.transform.position).normalized * collision.gameObject.GetComponent<item>().Knockback);
            }
        }
    }
}