using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

public class SoundMaker : MonoBehaviour
{
    AudioSource Sound;
    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Sound = GetComponent<AudioSource>();
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
        if(Sound != null && rb != null)
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

            Sound.volume = damage / 50;
            Sound.Play();
        }
    }
}