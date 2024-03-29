using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAI : MonoBehaviour
{
    public float JumpForce;
    public float MinJumpTime;
    public float MaxJumpTime;
    public Rigidbody2D[] rbs;

    Transform Target;
    Reference Ref;
    float CurrentTime;

    private void Start()
    {
        Target = GameObject.Find("Player").GetComponent<Player>().rb.transform;
        Ref = GameObject.Find("Global Ref").GetComponent<Reference>();
    }

    void Update()
    {
        CurrentTime -= Time.deltaTime;
        if(CurrentTime < 0 && Grounded())
        {
            for (int i = 0; i < rbs.Length; i++)
            {
                rbs[i].AddForce(JumpForce * ((Vector2)Target.position - rbs[i].position).normalized);
                if(Random.Range(0, 2) == 1)
                {
                    rbs[i].AddForce(new Vector2(0, JumpForce / 2));
                }
                else if (Random.Range(0, 2) == 0)
                {
                    rbs[i].AddForce(new Vector2(0, JumpForce));
                }
            }

            CurrentTime = Random.Range(MinJumpTime, MaxJumpTime);
        }
    }

    bool Grounded()
    {
        bool hit = Physics2D.Raycast(rbs[0].position, transform.TransformDirection(Vector2.down), 2.5f, Ref.GroundMask);
        return hit;
    }
}
