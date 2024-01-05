using UnityEngine;

public class item : MonoBehaviour
{
    [Header("Stats")]
    public int ItemID;
    public int BlockID;
    public int MiningDamage;
    public int MiningPower;
    public float MiningCooldown;
    public float Damage;
    public float Knockback;

    [Header("Skill Stats")]
    public GameObject ESkill;
    public GameObject ShiftSkill;

    [Header("Other")]
    public bool Grabable = true;
    public int Num = 1;

    public bool Held = false;
    bool LeftHand = true;

    Grab HandS;

    Rigidbody2D rb;
    Collider2D[] Coll;

    Camera cam;
    float offset = 180;
    public int speed = 50;

    GameObject PlayerOB;
    Player PlayerS;

    bool InGround;

    SlotMaker Inventory;
    Reference Ref;
    SoundMaker Sounds;

    void NoColl(bool Active)
    {
        Collider2D[] Colliders = PlayerOB.transform.GetComponentsInChildren<Collider2D>();
        for (int i = 0; i < Colliders.Length; i++)
        {
            for (int k = i; k < Colliders.Length; k++)
            {
                for (int l = 0; l < Coll.Length; l++)
                {
                    Physics2D.IgnoreCollision(Colliders[i], Coll[l], Active);
                }
            }
        }

        Sounds = GetComponent<SoundMaker>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Ref = GameObject.Find("Global Ref").GetComponent<Reference>();
        Inventory = Ref.Inventory;

        rb = GetComponent<Rigidbody2D>();
        Coll = GetComponents<Collider2D>();
        cam = Camera.main;
        PlayerOB = GameObject.Find("Player");
        PlayerS = PlayerOB.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Grabable)
        {
            if (Held)
            {
                if(Sounds != null)
                {
                    Sounds.enabled = true;
                }

                if (Input.GetKey(KeyCode.Mouse0) && LeftHand)
                {
                    if (Input.GetKeyDown(KeyCode.T))
                    {
                        Ref.AddItem(ItemID, Num);
                        HandS.Active = true;
                        Destroy(this.gameObject);

                        PlayerS.LeftID = 0;

                        PlayerS.LeftMiningDamage = 0;

                        PlayerS.LeftESkill = null;
                        PlayerS.LeftShiftSkill = null;

                        HandS.HeldID = 0;
                    }
                }
                if (Input.GetKey(KeyCode.Mouse1) && !LeftHand)
                {
                    if (Input.GetKeyDown(KeyCode.T))
                    {
                        Ref.AddItem(ItemID, Num);
                        HandS.Active = true;
                        Destroy(this.gameObject);

                        PlayerS.RightID = 0;

                        PlayerS.RightMiningDamage = 0;

                        PlayerS.RightESkill = null;
                        PlayerS.RightShiftSkill = null;

                        HandS.HeldID = 0;
                    }
                }

                if (Input.GetKey(KeyCode.Q) && Held == true)
                {
                    if (Input.GetKeyUp(KeyCode.Mouse0) && LeftHand)
                    {
                        Destroy(GetComponent<FixedJoint2D>());
                        HandS.Active = true;
                        Held = false;
                        NoColl(false);

                        PlayerS.LeftID = 0;

                        PlayerS.LeftMiningDamage = 0;
                        PlayerS.LeftMiningCooldown = 0;
                        PlayerS.LeftMiningPower = 0;

                        PlayerS.LeftESkill = null;
                        PlayerS.LeftShiftSkill = null;

                        HandS.HeldID = 0;
                    }
                    else if (Input.GetKeyUp(KeyCode.Mouse1) && LeftHand == false)
                    {
                        Destroy(GetComponent<FixedJoint2D>());
                        HandS.Active = true;
                        Held = false;
                        NoColl(false);

                        PlayerS.RightID = 0;

                        PlayerS.RightMiningDamage = 0;
                        PlayerS.RightMiningCooldown = 0;
                        PlayerS.RightMiningPower = 0;

                        PlayerS.RightESkill = null;
                        PlayerS.RightShiftSkill = null;

                        HandS.HeldID = 0;
                    }
                }

                //if (PlayerS.LeftEActive && LeftHand)
                //{
                //    Instantiate(ESkill, new Vector2(transform.position.x, transform.position.y), transform.rotation);
                //    PlayerS.LeftEActive = false;
                //}
                //else if (PlayerS.RightEActive && !LeftHand)
                //{
                //    Instantiate(ESkill, transform.position, transform.rotation);
                //    PlayerS.RightEActive = false;
                //}

                //if (PlayerS.LeftShiftActive && LeftHand)
                //{
                //    Instantiate(ESkill, new Vector2(transform.position.x, transform.position.y), transform.rotation);
                //    PlayerS.LeftShiftActive = false;
                //}
                //else if (PlayerS.RightShiftActive && !LeftHand)
                //{
                //    Instantiate(ShiftSkill, transform.position, transform.rotation);
                //    PlayerS.RightShiftActive = false;
                //}

                //if (LeftHand)
                //{
                //    transform.localScale = new Vector2(transform.localScale.x, transform.localScale.y);
                //}
                //else if (LeftHand == false)
                //{
                //    transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
                //}
            }
            else
            {
                if (Sounds != null)
                {
                    Sounds.enabled = false;
                }
            }

            Vector3 playerpos = new Vector3(cam.ScreenToWorldPoint(Input.mousePosition).x, cam.ScreenToWorldPoint(Input.mousePosition).y, 0);
            Vector3 difference = playerpos - transform.position;
            float rotationZ = Mathf.Atan2(difference.x, -difference.y) * Mathf.Rad2Deg;

            if (Input.GetKey(KeyCode.Mouse0) && LeftHand == true && Held)
            {
                rb.MoveRotation(Mathf.LerpAngle(rb.rotation, rotationZ + offset, speed * Time.fixedDeltaTime));
            }
            else if (Input.GetKey(KeyCode.Mouse1) && LeftHand == false && Held)
            {
                rb.MoveRotation(Mathf.LerpAngle(rb.rotation, rotationZ + offset, speed * Time.fixedDeltaTime));
            }
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(Grabable)
        {
            if (collision.gameObject.tag == "Left Hand" && Input.GetKey(KeyCode.Mouse0) && Held == false)
            {
                HandS = collision.gameObject.GetComponent<Grab>();
                if (HandS != null && HandS.Active)
                {
                    NoColl(true);

                    Rigidbody2D rb2 = collision.transform.GetComponent<Rigidbody2D>();
                    FixedJoint2D fj = transform.gameObject.AddComponent(typeof(FixedJoint2D)) as FixedJoint2D;
                    fj.connectedBody = rb2;
                    Held = true;
                    LeftHand = true;
                    HandS.Active = false;

                    transform.position = collision.gameObject.transform.position;

                    PlayerS.LeftMiningDamage = MiningDamage;
                    PlayerS.LeftMiningPower = MiningPower;
                    PlayerS.LeftMiningCooldown = MiningCooldown;

                    PlayerS.LeftESkill = ESkill;
                    PlayerS.LeftShiftSkill = ShiftSkill;

                    PlayerS.LeftID = BlockID;

                    HandS.HeldID = ItemID;
                    HandS.HeldAmount = Num;
                }
            }
            else if (collision.gameObject.tag == "Right Hand" && Input.GetKey(KeyCode.Mouse1) && Held == false)
            {
                HandS = collision.gameObject.GetComponent<Grab>();
                if (HandS != null && HandS.Active)
                {
                    NoColl(true);

                    Rigidbody2D rb2 = collision.transform.GetComponent<Rigidbody2D>();
                    FixedJoint2D fj = transform.gameObject.AddComponent(typeof(FixedJoint2D)) as FixedJoint2D;
                    fj.connectedBody = rb2;
                    Held = true;
                    LeftHand = false;
                    HandS.Active = false;

                    transform.position = collision.gameObject.transform.position;

                    PlayerS.RightMiningDamage = MiningDamage;
                    PlayerS.RightMiningPower = MiningPower;
                    PlayerS.RightMiningCooldown = MiningCooldown;

                    PlayerS.RightESkill = ESkill;
                    PlayerS.RightShiftSkill = ShiftSkill;

                    PlayerS.LeftID = BlockID;

                    HandS.HeldID = ItemID;
                    HandS.HeldAmount = Num;
                }
            }
        }
    }
}