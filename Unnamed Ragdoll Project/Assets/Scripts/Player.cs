using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Player : MonoBehaviour
{
    public Animator anim;
    public Rigidbody2D rb;
    public Vector2 JumpHeight;
    bool IsGrounded;
    public float CheckHeight;
    public LayerMask Ground;
    public LayerMask EverthingBut;
    public Transform PlayerPos;
    public TileMaker TileS;

    [Header("Stats")]
    public float MiningCooldown;
    public int MiningDamage;
    public int MiningPower;

    [Header("Left Stats")]
    public float LeftMiningCooldown;
    public int LeftMiningDamage;
    public int LeftMiningPower;

    [Header("Right Stats")]
    public float RightMiningCooldown;
    public int RightMiningDamage;
    public int RightMiningPower;

    public ParticleSystem DeathPart;

    public float PlayerSpeed;
    public float JumpForce;

    [Header("Items")]
    public int LeftID;
    public int RightID;


    public float MaxHealth = 100;
    public float Health;
    public TextMeshProUGUI HealthPercent;

    public float FallAmount;
    public float FallMultyplyer;

    public GameObject Menu;

    //public GameObject LeftArm;
    //public GameObject RightArm;
    //public GameObject LeftHand;
    //public GameObject RightHand;

    public GameObject LeftLeg;
    public GameObject RightLeg;
    public GameObject LeftFoot;
    public GameObject RightFoot;

    public Collider2D[] colliders;

    float MineCooldown;

    public GameObject LeftESkill;
    public GameObject LeftShiftSkill;

    public GameObject RightESkill;
    public GameObject RightShiftSkill;

    public bool RightEActive;
    public bool LeftEActive;

    public bool RightShiftActive;
    public bool LeftShiftActive;

    public GameObject DevTool;

    void Awake()
    {
        Health = MaxHealth;

        TileS.enabled = true;
        //transform.position = new Vector3(TileS.WorldWidth / 2, TileS.WorldHeight + 2);

        colliders = transform.GetComponentsInChildren<Collider2D>();
        for (int i = 0; i < colliders.Length; i++)
        {
            for (int k = i; k < colliders.Length; k++)
            {
                Physics2D.IgnoreCollision(colliders[i], colliders[k]);
            }
        }

        LeftLeg.GetComponent<HingeJoint2D>().useLimits = false;
        RightLeg.GetComponent<HingeJoint2D>().useLimits = false;
        LeftFoot.GetComponent<HingeJoint2D>().useLimits = false;
        RightFoot.GetComponent<HingeJoint2D>().useLimits = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            Health = 0;
        }

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            Vector2 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Instantiate(DevTool, MousePos, transform.rotation).GetComponent<item>();
        }

        TileS.enabled = true;
        MineCooldown += Time.deltaTime;
        Cursor.lockState = CursorLockMode.Confined;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        IsGrounded = Grounded();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(Menu.activeSelf)
            {
                Menu.SetActive(false);
            }
            else
            {
                Menu.SetActive(true);
            }
        }

        if (Input.GetKey(KeyCode.E))
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                if (LeftESkill == null)
                {
                    if (TileS.TileTypes[TileS.CheckTile((int)Mathf.Round(worldPosition.x), (int)Mathf.Round(worldPosition.y))].IsLiquid && !Physics2D.OverlapCircle(new Vector2((int)Mathf.Round(worldPosition.x), (int)Mathf.Round(worldPosition.y)), 0.45f, EverthingBut) && LeftID != 0)
                    {
                        TileS.SetTile((int)Mathf.Round(worldPosition.x), (int)Mathf.Round(worldPosition.y), LeftID, true);
                    }
                    else if (LeftID == 0 && MineCooldown >= MiningCooldown + LeftMiningCooldown)
                    {
                        MineCooldown = 0;
                        TileS.DamageTile((int)Mathf.Round(worldPosition.x), (int)Mathf.Round(worldPosition.y), MiningDamage + LeftMiningDamage, true, MiningPower + LeftMiningPower);
                    }
                }
                else if (Input.GetKeyDown(KeyCode.E))
                {
                    LeftEActive = true;
                }
            }
            else if (Input.GetKey(KeyCode.Mouse1))
            {
                if (RightESkill == null)
                {
                    if (TileS.TileTypes[TileS.CheckTile((int)Mathf.Round(worldPosition.x), (int)Mathf.Round(worldPosition.y))].IsLiquid && !Physics2D.OverlapCircle(new Vector2((int)Mathf.Round(worldPosition.x), (int)Mathf.Round(worldPosition.y)), 0.45f, EverthingBut) && RightID != 0)
                    {
                        TileS.SetTile((int)Mathf.Round(worldPosition.x), (int)Mathf.Round(worldPosition.y), RightID, true);
                    }
                    else if (RightID == 0 && MineCooldown >= MiningCooldown + RightMiningCooldown)
                    {
                        MineCooldown = 0;
                        TileS.DamageTile((int)Mathf.Round(worldPosition.x), (int)Mathf.Round(worldPosition.y), MiningDamage + RightMiningDamage, true, MiningPower + RightMiningPower);
                    }
                }
                else if (Input.GetKeyDown(KeyCode.E))
                {
                    RightEActive = true;
                }
            }
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                if(LeftShiftSkill == null)
                {
                    if (LeftID > 0 && TileS.CheckBackTile((int)Mathf.Round(worldPosition.x), (int)Mathf.Round(worldPosition.y)) == 0)
                    {
                        TileS.SetTile((int)Mathf.Round(worldPosition.x), (int)Mathf.Round(worldPosition.y), LeftID, false);
                    }
                    else if (LeftID == 0 && MineCooldown >= MiningCooldown + LeftMiningCooldown)
                    {
                        MineCooldown = 0;
                        TileS.DamageTile((int)Mathf.Round(worldPosition.x), (int)Mathf.Round(worldPosition.y), MiningDamage + LeftMiningDamage, false, MiningPower + LeftMiningPower);
                    }
                }
                else if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    LeftShiftActive = true;
                }
            }
            else if (Input.GetKey(KeyCode.Mouse1))
            {
                if (RightShiftSkill == null && TileS.CheckBackTile((int)Mathf.Round(worldPosition.x), (int)Mathf.Round(worldPosition.y)) == 0)
                {
                    if (RightID > 0)
                    {
                        TileS.SetTile((int)Mathf.Round(worldPosition.x), (int)Mathf.Round(worldPosition.y), RightID, false);
                    }
                    else if (RightID == 0 && MineCooldown >= MiningCooldown + RightMiningCooldown)
                    {
                        MineCooldown = 0;
                        TileS.DamageTile((int)Mathf.Round(worldPosition.x), (int)Mathf.Round(worldPosition.y), MiningDamage + RightMiningDamage, false, MiningPower + RightMiningPower);
                    }
                }
                else if(Input.GetKeyDown(KeyCode.LeftShift))
                {
                    RightShiftActive = true;
                }
            }
        }


        if (Input.GetKey(KeyCode.Space) && rb.GetComponent<WaterBuoyncy>().InWater)
        {
            rb.AddForce(-Vector2.down * PlayerSpeed * Time.deltaTime);
        }

        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            if (Input.GetAxisRaw("Horizontal") > 0)
            {
                anim.Play("WalkForward");
                rb.AddForce(PlayerSpeed * Time.deltaTime * Vector2.right);
            }
            else
            {
                anim.Play("WalkBackward");
                rb.AddForce(PlayerSpeed * Time.deltaTime * Vector2.left);
            }
        }
        else
        {
            anim.Play("Idle");
        }

        if(Input.GetKey(KeyCode.S))
        {
            rb.AddForce(PlayerSpeed * Time.deltaTime * Vector2.down);
            if(LeftLeg.GetComponent<LimbHealth>().Health > 0)
            {
                LeftLeg.GetComponent<Balance>().force = 30;
            }
            if (RightLeg.GetComponent<LimbHealth>().Health > 0)
            {
                RightLeg.GetComponent<Balance>().force = 30;
            }

            if (LeftFoot.GetComponent<LimbHealth>().Health > 0)
            {
                LeftFoot.GetComponent<Balance>().force = 30;
            }
            if (RightFoot.GetComponent<LimbHealth>().Health > 0)
            {
                RightFoot.GetComponent<Balance>().force = 30;
            }
        }
        else
        {
            if (LeftLeg.GetComponent<LimbHealth>().Health > 0)
            {
                LeftLeg.GetComponent<Balance>().force = 200;
            }
            if (RightLeg.GetComponent<LimbHealth>().Health > 0)
            {
                RightLeg.GetComponent<Balance>().force = 200;
            }

            if (LeftFoot.GetComponent<LimbHealth>().Health > 0)
            {
                LeftFoot.GetComponent<Balance>().force = 200;
            }
            if (RightFoot.GetComponent<LimbHealth>().Health > 0)
            {
                RightFoot.GetComponent<Balance>().force = 200;
            }
        }

        if (IsGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(JumpForce * Vector2.up);
        }

        HealthPercent.text = Health.ToString() + "%";

        if (Health <= 0)
        {
            DeathPart.Play();
            anim.Play("Game Over");
            GetComponentInChildren<LimbHealth>().Health = 0;
            this.enabled = false;
        }
    }

    bool Grounded()
    {
        bool hit = Physics2D.Raycast(PlayerPos.position, transform.TransformDirection(Vector2.down), CheckHeight, Ground);
        return hit;
    }
}