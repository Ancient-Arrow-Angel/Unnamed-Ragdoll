using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

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

    [Header("Base Stats")]
    public float MiningCooldown;
    public int MiningDamage;
    public int MiningPower;
    public int DamageMultiplier;
    public int HeavyDamageMultiplier = 1;
    public int LightDamageMultiplier = 1;
    public int MagicDamageMultiplier = 1;

    [Header("Left Stats")]
    public float LeftMiningCooldown;
    public int LeftMiningDamage;
    public int LeftMiningPower;

    [Header("Right Stats")]
    public float RightMiningCooldown;
    public int RightMiningDamage;
    public int RightMiningPower;

    public ParticleSystem DeathPart;

    [Header("Movement Stats")]
    public float PlayerSpeed;
    public float JumpForce;
    public float MaxSpeed;

    [Header("Items")]
    public int LeftID;
    public int RightID;


    public float MaxHealth = 100;
    public float Health;
    public TextMeshProUGUI HealthPercent;

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

    public GameObject DevTool;

    public LimbHealth[] Legs;
    public LimbHealth[] Chest;
    public LimbHealth Head;

    public GameObject ChestMenu;
    GameObject[] ChestMenus;

    public item LeftItem;
    public item RightItem;

    public bool PlayFail;
    float FailCooldown;
    public GameObject FailedPickup;
    public GameObject CraftMenu;

    public Reference Ref;

    public CooldownBar LeftECool;
    public CooldownBar RightECool;
    public CooldownBar LeftShiftCool;
    public CooldownBar RightShiftCool;

    public Color NormalCoolColor;

    public Grab LeftHand;
    public Grab RightHand;

    void Awake() 
    {
        ChestMenus = new GameObject[ChestMenu.transform.GetComponentsInChildren<RectTransform>().Length -1];
        for (int i = 1; i < ChestMenus.Length; i++)
        {
            ChestMenus[i-1] = ChestMenu.transform.GetComponentsInChildren<RectTransform>()[i].gameObject;
        }

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
        if(LeftItem != null)
        {
            if (LeftItem.ESkill != null)
            {
                LeftECool.GetComponentInChildren<Image>().color = NormalCoolColor;
                LeftECool.transform.parent.GetComponent<Image>().color = Color.white;
            }
            else
            {
                LeftECool.GetComponentInChildren<Image>().color = Color.clear;
                LeftECool.transform.parent.GetComponent<Image>().color = Color.clear;
            }

            if (LeftItem.ShiftSkill != null)
            {
                LeftShiftCool.GetComponentInChildren<Image>().color = NormalCoolColor;
                LeftShiftCool.transform.parent.GetComponent<Image>().color = Color.white;
            }
            else
            {
                LeftShiftCool.GetComponentInChildren<Image>().color = Color.clear;
                LeftShiftCool.transform.parent.GetComponent<Image>().color = Color.clear;
            }
        }
        else
        {
            LeftECool.GetComponentInChildren<Image>().color = Color.clear;
            LeftECool.transform.parent.GetComponent<Image>().color = Color.clear;
            LeftShiftCool.GetComponentInChildren<Image>().color = Color.clear;
            LeftShiftCool.transform.parent.GetComponent<Image>().color = Color.clear;
        }
        if (RightItem != null)
        {
            if (RightItem.ESkill != null)
            {
                RightECool.GetComponentInChildren<Image>().color = NormalCoolColor;
                RightECool.transform.parent.GetComponent<Image>().color = Color.white;
            }
            else
            {
                RightECool.GetComponentInChildren<Image>().color = Color.clear;
                RightECool.transform.parent.GetComponent<Image>().color = Color.clear;
            }

            if (RightItem.ShiftSkill != null)
            {
                RightShiftCool.GetComponentInChildren<Image>().color = NormalCoolColor;
                RightShiftCool.transform.parent.GetComponent<Image>().color = Color.white;
            }
            else
            {
                RightShiftCool.GetComponentInChildren<Image>().color = Color.clear;
                RightShiftCool.transform.parent.GetComponent<Image>().color = Color.clear;
            }
        }
        else
        {
            RightECool.GetComponentInChildren<Image>().color = Color.clear;
            RightECool.transform.parent.GetComponent<Image>().color = Color.clear;
            RightShiftCool.GetComponentInChildren<Image>().color = Color.clear;
            RightShiftCool.transform.parent.GetComponent<Image>().color = Color.clear;
        }


        FailCooldown -= Time.deltaTime;

        if (PlayFail == true && FailCooldown <= 0)
        {
            Instantiate(FailedPickup, rb.transform.position, transform.rotation);
            PlayFail = false;
            FailCooldown = 2;
        }
        else
        {
            PlayFail = false;
        }

        if (Input.GetKeyDown(KeyCode.P))
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

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (LeftItem != null)
            {
                LeftItem.HandS = RightHand;

                Destroy(LeftItem.GetComponent<FixedJoint2D>());
                Rigidbody2D rb2 = RightHand.transform.GetComponent<Rigidbody2D>();
                FixedJoint2D fj = LeftItem.transform.gameObject.AddComponent(typeof(FixedJoint2D)) as FixedJoint2D;
                fj.connectedBody = rb2;

                LeftItem.Held = true;
                LeftItem.LeftHand = false;
                RightHand.Active = false;

                LeftItem.transform.position = RightHand.transform.position;

                RightMiningDamage = LeftItem.MiningDamage;
                RightMiningPower = LeftItem.MiningPower;
                RightMiningCooldown = LeftItem.MiningCooldown;

                RightID = LeftItem.BlockID;

                RightHand.HeldID = LeftItem.ItemID;
                RightHand.HeldAmount = LeftItem.ItemNum;

                LeftItem.transform.localScale = new Vector2(-LeftItem.transform.localScale.x, LeftItem.transform.localScale.y);
            }
            if (RightItem != null)
            {
                RightItem.HandS = LeftHand;

                Destroy(RightItem.GetComponent<FixedJoint2D>());
                Rigidbody2D rb2 = LeftHand.transform.GetComponent<Rigidbody2D>();
                FixedJoint2D fj = RightItem.transform.gameObject.AddComponent(typeof(FixedJoint2D)) as FixedJoint2D;
                fj.connectedBody = rb2;

                RightItem.Held = true;
                RightItem.LeftHand = true;
                LeftHand.Active = false;

                RightItem.transform.position = LeftHand.transform.position;

                LeftMiningDamage = RightItem.MiningDamage;
                LeftMiningPower = RightItem.MiningPower;
                LeftMiningCooldown = RightItem.MiningCooldown;

                LeftID = RightItem.BlockID;

                LeftHand.HeldID = RightItem.ItemID;
                LeftHand.HeldAmount = RightItem.ItemNum;

                RightItem.transform.localScale = new Vector2(-RightItem.transform.localScale.x, RightItem.transform.localScale.y);
            }

            if (LeftItem == null)
            {
                RightHand.Active = true;

                RightMiningDamage = 0;
                RightMiningPower = 0;
                RightMiningCooldown = 0;

                RightID = 0;

                RightHand.HeldID = 0;
                RightHand.HeldAmount = 0;
            }

            if (RightItem == null)
            {
                LeftHand.Active = true;

                LeftMiningDamage = 0;
                LeftMiningPower = 0;
                LeftMiningCooldown = 0;

                LeftID = 0;

                LeftHand.HeldID = 0;
                LeftHand.HeldAmount = 0;
            }

            item InterItem = null;
            InterItem = RightItem;
            RightItem = LeftItem;
            LeftItem = InterItem;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(Menu.activeSelf)
            {
                for (int i = 0; i < ChestMenus.Length - 1; i++)
                {
                    ChestMenus[i].SetActive(false);
                }
                Menu.SetActive(false);
                CraftMenu.SetActive(false);
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
                if (TileS.TileTypes[TileS.CheckTile((int)Mathf.Round(worldPosition.x), (int)Mathf.Round(worldPosition.y))].IsLiquid && !Physics2D.OverlapCircle(new Vector2((int)Mathf.Round(worldPosition.x), (int)Mathf.Round(worldPosition.y)), 0.45f, EverthingBut) && LeftID != 0)
                {
                    TileS.SetTile((int)Mathf.Round(worldPosition.x), (int)Mathf.Round(worldPosition.y), LeftID, true);

                    if(LeftItem != null)
                        LeftItem.ItemNum--;
                }
                else if(TileS.CheckTile((int)Mathf.Round(worldPosition.x), (int)Mathf.Round(worldPosition.y)) == 0 && !Physics2D.OverlapCircle(new Vector2((int)Mathf.Round(worldPosition.x), (int)Mathf.Round(worldPosition.y)), 0.45f, EverthingBut) && LeftID != 0)
                {
                    TileS.SetTile((int)Mathf.Round(worldPosition.x), (int)Mathf.Round(worldPosition.y), LeftID, true);

                    if (LeftItem != null)
                        LeftItem.ItemNum--;
                }
                else if (LeftID == 0 && MineCooldown >= MiningCooldown + LeftMiningCooldown && TileS.CheckTile((int)Mathf.Round(worldPosition.x), (int)Mathf.Round(worldPosition.y)) != 0)
                {
                    MineCooldown = 0;
                    TileS.DamageTile((int)Mathf.Round(worldPosition.x), (int)Mathf.Round(worldPosition.y), MiningDamage + LeftMiningDamage, true, MiningPower + LeftMiningPower);
                }
            }
            else if (Input.GetKey(KeyCode.Mouse1))
            {
                if (TileS.TileTypes[TileS.CheckTile((int)Mathf.Round(worldPosition.x), (int)Mathf.Round(worldPosition.y))].IsLiquid && !Physics2D.OverlapCircle(new Vector2((int)Mathf.Round(worldPosition.x), (int)Mathf.Round(worldPosition.y)), 0.45f, EverthingBut) && RightID != 0)
                {
                    TileS.SetTile((int)Mathf.Round(worldPosition.x), (int)Mathf.Round(worldPosition.y), RightID, true);

                    if (RightItem != null)
                        RightItem.ItemNum--;
                }
                else if (TileS.CheckTile((int)Mathf.Round(worldPosition.x), (int)Mathf.Round(worldPosition.y)) == 0 && !Physics2D.OverlapCircle(new Vector2((int)Mathf.Round(worldPosition.x), (int)Mathf.Round(worldPosition.y)), 0.45f, EverthingBut) && RightID != 0)
                {
                    TileS.SetTile((int)Mathf.Round(worldPosition.x), (int)Mathf.Round(worldPosition.y), RightID, true);

                    if (RightItem != null)
                        RightItem.ItemNum--;
                }
                else if (RightID == 0 && MineCooldown >= MiningCooldown + RightMiningCooldown && TileS.CheckTile((int)Mathf.Round(worldPosition.x), (int)Mathf.Round(worldPosition.y)) != 0) 
                {
                    MineCooldown = 0;
                    TileS.DamageTile((int)Mathf.Round(worldPosition.x), (int)Mathf.Round(worldPosition.y), MiningDamage + RightMiningDamage, true, MiningPower + RightMiningPower);
                }
            }
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                if (LeftID > 0 && TileS.CheckBackTile((int)Mathf.Round(worldPosition.x), (int)Mathf.Round(worldPosition.y)) == 0)
                {
                    TileS.SetTile((int)Mathf.Round(worldPosition.x), (int)Mathf.Round(worldPosition.y), LeftID, false);

                    if (LeftItem != null)
                        LeftItem.ItemNum--;
                }
                else if (LeftID == 0 && MineCooldown >= MiningCooldown + LeftMiningCooldown) 
                {
                    MineCooldown = 0;
                    TileS.DamageTile((int)Mathf.Round(worldPosition.x), (int)Mathf.Round(worldPosition.y), MiningDamage + LeftMiningDamage, false, MiningPower + LeftMiningPower);
                }
            }
            else if (Input.GetKey(KeyCode.Mouse1))
            {
                if (RightID > 0 && TileS.CheckBackTile((int)Mathf.Round(worldPosition.x), (int)Mathf.Round(worldPosition.y)) == 0)
                {
                    TileS.SetTile((int)Mathf.Round(worldPosition.x), (int)Mathf.Round(worldPosition.y), RightID, false);

                    if (RightItem != null)
                        RightItem.ItemNum--;
                }
                else if (RightID == 0 && MineCooldown >= MiningCooldown + RightMiningCooldown) 
                {
                    MineCooldown = 0;
                    TileS.DamageTile((int)Mathf.Round(worldPosition.x), (int)Mathf.Round(worldPosition.y), MiningDamage + RightMiningDamage, false, MiningPower + RightMiningPower);
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
                if (rb.velocity.x < MaxSpeed)
                {
                    rb.AddForce(PlayerSpeed * Time.deltaTime * Vector2.right);
                    anim.Play("WalkForward");
                }
                else
                {
                    anim.Play("Idle");
                }
            }
            else
            {
                if (rb.velocity.x > -MaxSpeed)
                {
                    anim.Play("WalkBackward");
                    rb.AddForce(PlayerSpeed * Time.deltaTime * Vector2.left);
                }
                else
                {
                    anim.Play("Idle");
                }
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
                LeftLeg.GetComponent<Balance>().force = 600;
            }
            if (RightLeg.GetComponent<LimbHealth>().Health > 0)
            {
                RightLeg.GetComponent<Balance>().force = 600;
            }

            if (LeftFoot.GetComponent<LimbHealth>().Health > 0)
            {
                LeftFoot.GetComponent<Balance>().force = 600;
            }
            if (RightFoot.GetComponent<LimbHealth>().Health > 0)
            {
                RightFoot.GetComponent<Balance>().force = 600;
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
            Menu.SetActive(false);
            CraftMenu.SetActive(false);
            anim.Play("Game Over");
            GetComponentInChildren<LimbHealth>().Health = 0;
            this.enabled = false;
        }

        //if (Input.GetKey(KeyCode.LeftShift))
        //{
        //    rb.velocity = Vector2.zero;
        //}
        if (Input.GetKey(KeyCode.W))
        {
            rb.velocity = new Vector2(rb.velocity.x, 20);
        }
    }

    bool Grounded()
    {
        bool hit = Physics2D.Raycast(rb.position, transform.TransformDirection(Vector2.down), CheckHeight, Ground);
        return hit;
    }
}