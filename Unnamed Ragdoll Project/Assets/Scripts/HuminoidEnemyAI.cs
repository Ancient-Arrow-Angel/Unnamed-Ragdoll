using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class HuminoidEnemyAI : MonoBehaviour
{
    public float JumpForce;
    public float Speed;
    public float MinJumpCooldown;
    public float MaxJumpCooldown;

    public float JumpAwayChance;
    public float JumpFowardChance;

    public float SeeRange;

    float CurrentCooldown;

    Rigidbody2D Prb;
    Player player;
    public Rigidbody2D rb;
    public Rigidbody2D Head;
    public LayerMask Ground;
    public LayerMask Player;

    public bool Pursuing;

    public GameObject ViewObj;

    public MusicHandler Music;

    public int BossSong;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        Music = GameObject.Find("Songs").GetComponent<MusicHandler>();
        Prb = player.rb;
    }

    // Update is called once per frame
    void Update()
    {
        if (Pursuing)
        {
            if(BossSong > 0)
            {
                Music.SongOn = 4;
            }

            CurrentCooldown -= Time.deltaTime;

            if (Prb.transform.position.x > rb.transform.position.x)
            {
                rb.AddForce(Speed * Time.deltaTime * Vector2.right);
            }
            else
            {
                rb.AddForce(Speed * Time.deltaTime * Vector2.left);
            }

            if (Grounded() && CurrentCooldown <= 0)
            {
                rb.AddForce(JumpForce * Vector2.up);
                if (Random.Range(1, 101) <= JumpAwayChance)
                {
                    if (Prb.transform.position.x > rb.transform.position.x)
                    {
                        rb.AddForce(JumpForce * Time.deltaTime * Vector2.left);
                    }
                    else
                    {
                        rb.AddForce(JumpForce * Time.deltaTime * Vector2.right);
                    }
                }
                else if (Random.Range(1, 101) <= JumpFowardChance)
                {
                    if (Prb.transform.position.x > rb.transform.position.x)
                    {
                        rb.AddForce(JumpForce * Time.deltaTime * Vector2.right);
                    }
                    else
                    {
                        rb.AddForce(JumpForce * Time.deltaTime * Vector2.left);
                    }
                }
                CurrentCooldown = Random.Range(MinJumpCooldown, MaxJumpCooldown);
            }
        }
        else
        {
            if (SeePlayer())
            {
                Pursuing = true;
            }
        }
    }

    bool Grounded()
    {
        bool hit = Physics2D.Raycast(rb.position, transform.TransformDirection(Vector2.down), 2.5f, Ground);
        return hit;
    }

    bool SeePlayer()
    {
        bool Ret = false;
        Vector2 Pos = Vector2.zero;
        Pos = transform.TransformDirection(player.rb.position - Head.position).normalized + new Vector3(Head.position.x, Head.position.y, 0);
        if (Physics2D.OverlapCircle(Pos, 0.25f * SeeRange, Player))
        {
            for (int i = 0; i < SeeRange; i++)
            {
                if (!Ret)
                {
                    Pos = i * 0.25f * transform.TransformDirection(player.rb.position - Head.position).normalized + new Vector3(Head.position.x, Head.position.y, 0);
                    //GameObject Obj = Instantiate(ViewObj, Pos, Quaternion.identity);
                    //Destroy(Obj, 0.05f);

                    if (Physics2D.OverlapCircle(Pos, 0.1f, Ground))
                    {
                        Ret = true;
                        return false;
                    }
                    else if (Physics2D.OverlapCircle(Pos, 0.1f, Player))
                    {
                        Ret = true;
                        return true;
                    }
                }
            }
        }
        return false;
    }
}