using UnityEngine;

public class item : MonoBehaviour
{
    [Header("Stats")]
    public int ItemID;
    public int BlockID;
    public int MiningDamage;
    public int MiningPower;
    public float MiningCooldown;
    public float HeavyDamage;
    public float LightDamage;
    public float MagicDamage;
    public float Knockback;
    public float HitManaMod = 0;
    [HideInInspector]
    public float HeavyDamageMod = 1;
    [HideInInspector]
    public float LightDamageMod = 1;
    [HideInInspector]
    public float MagicDamageMod = 1;

    [Header("Skill Stats")]
    public GameObject ESkill;
    public float ECooldown;
    public int EManaCost;
    public bool EHold;
    public GameObject ShiftSkill;
    public float ShiftCooldown;
    public int ShiftManaCost;
    public bool ShiftHold;
    public GameObject HeavySkill;
    public float HeavyForce;
    float ETimer;
    float ShiftTimer;

    [Header("Armor Stats")]
    public byte ArmorType;
    public float Defence;
    public float Health;
    public Sprite[] ArmorSprites;

    [Header("Furniture Options")]
    public bool IsFurniture;
    public int FurnitureID;
    public int Width = 1;
    public int Height = 1;
    public GameObject GhostObject;
    public GameObject Unplaced;
    public GameObject GroundPlaced;
    public GameObject LeftWallMount;
    public GameObject RightWallMount;
    public GameObject BackgroundMount;
    public GameObject CeilingMount;

    [Header("Other")]
    public bool Consumable;
    public bool Grabable = true;
    public bool HurtAll;
    public int ItemNum = 1;
    
    [HideInInspector]
    public bool Held = false;
    [HideInInspector]
    public bool LeftHand = true;

    [HideInInspector]
    public Grab HandS;

    Rigidbody2D rb;
    Collider2D[] Coll;
    BoxCollider2D InnerColl;

    Camera cam;
    float offset = 180;
    public int speed = 50;

    GameObject PlayerOB;
    [HideInInspector]
    public Player PlayerS;

    bool InGround;

    SlotMaker Inventory;
    Reference Ref;
    SoundMaker Sounds;

    public ParticleSystem OnHitParts;

    Rigidbody2D rb2;

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
    void Awake()
    {
        Ref = GameObject.Find("Global Ref").GetComponent<Reference>();
        Inventory = Ref.Inventory;

        rb = GetComponent<Rigidbody2D>();
        Coll = GetComponents<Collider2D>();
        cam = Camera.main;
        PlayerOB = GameObject.Find("Player");
        PlayerS = PlayerOB.GetComponent<Player>();

        if(Grabable)
        {
            InnerColl = gameObject.AddComponent<BoxCollider2D>();
            InnerColl.offset = Coll[0].offset;
            InnerColl.size = new Vector2(Coll[0].bounds.size.x - Coll[0].bounds.size.x / 1.5f, Coll[0].bounds.size.y - Coll[0].bounds.size.y / 1.5f);
            InnerColl.isTrigger = true;
        }
    }

    private void FixedUpdate()
    {
        if(PlayerS.Health > 0)
        {
            Vector3 playerpos = new Vector3(cam.ScreenToWorldPoint(Input.mousePosition).x, cam.ScreenToWorldPoint(Input.mousePosition).y, 0);
            Vector3 difference = playerpos - transform.position;
            float rotationZ = Mathf.Atan2(difference.x, -difference.y) * Mathf.Rad2Deg;

            if (Input.GetKey(KeyCode.Mouse0) && LeftHand == true && Held && speed > 0)
            {
                rb.MoveRotation(Mathf.LerpAngle(rb.rotation, rotationZ + offset, speed * Time.fixedDeltaTime));
            }
            else if (Input.GetKey(KeyCode.Mouse1) && LeftHand == false && Held && speed > 0)
            {
                rb.MoveRotation(Mathf.LerpAngle(rb.rotation, rotationZ + offset, speed * Time.fixedDeltaTime));
            }
        }

        if (ItemNum <= 0)
        {
            if (Held)
            {
                HandS.Active = true;

                if (LeftHand)
                {
                    PlayerS.LeftMiningDamage = 0;
                    PlayerS.LeftMiningPower = 0;
                    PlayerS.LeftMiningCooldown = 0;

                    PlayerS.LeftID = 0;

                    HandS.HeldID = 0;
                    HandS.HeldAmount = 0;

                    PlayerS.LeftItem = null;

                    PlayerS.LeftECool.SetValue(0);

                    PlayerS.LeftShiftCool.SetValue(0);
                }
                else
                {
                    PlayerS.RightMiningDamage = 0;
                    PlayerS.RightMiningPower = 0;
                    PlayerS.RightMiningCooldown = 0;

                    PlayerS.RightID = 0;

                    HandS.HeldID = 0;
                    HandS.HeldAmount = 0;

                    PlayerS.RightItem = null;

                    PlayerS.RightECool.SetValue(0);

                    PlayerS.RightShiftCool.SetValue(0);
                }

                HandS.HeldID = 0;
            }

            DestroyImmediate(this.gameObject);
        }

        if(this != null)
        {
            if (Physics2D.OverlapCircle(transform.position, Ref.ItemCombineRange, Ref.ItemMask))
            {
                item OtherItem = Physics2D.OverlapCircle(transform.position, Ref.ItemCombineRange, Ref.ItemMask).GetComponent<item>();
                if (OtherItem.ItemID > 0 && OtherItem.ItemID == ItemID && OtherItem != this)
                {
                    if (!OtherItem.Held)
                    {
                        if (OtherItem.ItemNum + ItemNum > Ref.StackAmount)
                        {
                            int Aint = ItemNum;
                            ItemNum += OtherItem.ItemNum - (OtherItem.ItemNum + ItemNum - Ref.StackAmount);
                            OtherItem.ItemNum -= (ItemNum - Aint);
                        }
                        else
                        {
                            ItemNum += OtherItem.ItemNum;
                            OtherItem.ItemNum = 0;
                        }
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        if (Grabable)
        {
            if (InnerColl.IsTouchingLayers(Ref.GroundMask))
            {
                if (Held)
                {
                    Coll[0].isTrigger = true;
                }
            }
            else
            {
                Coll[0].isTrigger = false;
            }
        }

        HeavyDamageMod = PlayerS.HeavyDamageMultiplier * PlayerS.DamageMultiplier;
        LightDamageMod = PlayerS.LightDamageMultiplier * PlayerS.DamageMultiplier;
        MagicDamageMod = PlayerS.MagicDamageMultiplier * PlayerS.DamageMultiplier;

        if (ItemNum <= 0)
        {
            if(Held)
            {
                HandS.Active = true;

                if (LeftHand)
                {
                    PlayerS.LeftMiningDamage = 0;
                    PlayerS.LeftMiningPower = 0;
                    PlayerS.LeftMiningCooldown = 0;

                    PlayerS.LeftID = 0;

                    HandS.HeldID = 0;
                    HandS.HeldAmount = 0;

                    PlayerS.LeftItem = null;

                    PlayerS.LeftECool.SetValue(0);

                    PlayerS.LeftShiftCool.SetValue(0);
                }
                else
                {
                    PlayerS.RightMiningDamage = 0;
                    PlayerS.RightMiningPower = 0;
                    PlayerS.RightMiningCooldown = 0;

                    PlayerS.RightID = 0;

                    HandS.HeldID = 0;
                    HandS.HeldAmount = 0;

                    PlayerS.RightItem = null;

                    PlayerS.RightECool.SetValue(0);

                    PlayerS.RightShiftCool.SetValue(0);
                }

                HandS.HeldID = 0;
            }

            DestroyImmediate(this.gameObject);
        }

        if (GhostObject != null)
        {
            GhostObject.SetActive(false);
        }

        if(Grabable && this != null)
        {
            ETimer -= Time.deltaTime;
            ShiftTimer -= Time.deltaTime;
        }

        if (Grabable && this != null && PlayerS.Health > 0)
        {
            if (Held)
            {
                HandS.HeldAmount = ItemNum;

                if (Sounds != null)
                {
                    Sounds.enabled = true;
                }

                if (Input.GetKey(KeyCode.Mouse0) && LeftHand)
                {
                    if(GhostObject != null)
                    {
                        GhostObject.SetActive(true);
                        GhostObject.transform.position = new Vector2(Mathf.Round(mousePos.x), Mathf.Round(mousePos.y));
                        GhostObject.transform.rotation = Quaternion.Euler(0, 0, 0);
                    }

                    if (Input.GetKeyDown(KeyCode.E) && IsFurniture)
                    {
                        bool CanGroundPlace = true;
                        bool CanLeftPlace = true;
                        bool CanRightPlace = true;
                        bool CanTopPlace = true;
                        bool CanBackPlace = true;

                        for (int i = 0; i < Height; i++)
                        {
                            for (int j = 0; j < Width; j++)
                            {
                                if (Ref.grid.CheckTile(j + (int)Mathf.Round(mousePos.x), i + (int)Mathf.Round(mousePos.y)) > 0)
                                {
                                    CanGroundPlace = false;
                                    CanLeftPlace = false;
                                    CanRightPlace = false;
                                    CanTopPlace = false;
                                    CanBackPlace = false;
                                }
                                else if (Physics2D.OverlapCircle(new Vector2(j + (int)Mathf.Round(mousePos.x), i + (int)Mathf.Round(mousePos.y)), 0.45f, Ref.FurnitureMask))
                                {
                                    CanGroundPlace = false;
                                    CanLeftPlace = false;
                                    CanRightPlace = false;
                                    CanTopPlace = false;
                                    CanBackPlace = false;
                                }
                            }
                        }

                        for (int i = 0; i < Width; i++)
                        {
                            if (Ref.grid.CheckTile(i + (int)Mathf.Round(mousePos.x), (int)Mathf.Round(mousePos.y) - 1) == 0 || Ref.grid.TileTypes[Ref.grid.CheckTile(i + (int)Mathf.Round(mousePos.x), (int)Mathf.Round(mousePos.y) - 1)].IsLiquid)
                            {
                                CanGroundPlace = false;
                            }
                        }
                        for (int i = 0; i < Width; i++)
                        {
                            if (Ref.grid.CheckTile(i + (int)Mathf.Round(mousePos.x), (int)Mathf.Round(mousePos.y) + Height) == 0 || Ref.grid.TileTypes[Ref.grid.CheckTile(i + (int)Mathf.Round(mousePos.x), (int)Mathf.Round(mousePos.y) + Height)].IsLiquid)
                            {
                                CanTopPlace = false;
                            }
                        }
                        for (int i = 0; i < Height; i++)
                        {
                            if (Ref.grid.CheckTile((int)Mathf.Round(mousePos.x) - 1, i + (int)Mathf.Round(mousePos.y)) == 0 || Ref.grid.TileTypes[Ref.grid.CheckTile((int)Mathf.Round(mousePos.x) - 1, i + (int)Mathf.Round(mousePos.y))].IsLiquid)
                            {
                                CanLeftPlace = false;
                            }
                        }
                        for (int i = 0; i < Height; i++)
                        {
                            if (Ref.grid.CheckTile((int)Mathf.Round(mousePos.x) + Width, i + (int)Mathf.Round(mousePos.y)) == 0 || Ref.grid.TileTypes[Ref.grid.CheckTile((int)Mathf.Round(mousePos.x) - Width, i + (int)Mathf.Round(mousePos.y))].IsLiquid)
                            {
                                CanRightPlace = false;
                            }
                        }
                        for (int i = 0; i < Height; i++)
                        {
                            for (int j = 0; j < Width; j++)
                            {
                                if (Ref.grid.CheckBackTile(j + (int)Mathf.Round(mousePos.x), i + (int)Mathf.Round(mousePos.y)) == 0)
                                {
                                    CanBackPlace = false;
                                }
                            }
                        }

                        if (CanLeftPlace && LeftWallMount != null)
                        {
                            LeftWallMount.SetActive(true);
                            GameObject NewPlaced = Instantiate(LeftWallMount, new Vector2(Mathf.Round(mousePos.x), Mathf.Round(mousePos.y)), Quaternion.Euler(0, 0, 0));
                            Ref.grid.FurnitureIDS[(int)Mathf.Round(mousePos.x) + (int)Mathf.Round(mousePos.y) * Ref.grid.WorldWidth] = FurnitureID;
                            ItemNum--;
                            LeftWallMount.SetActive(false);
                        }
                        else if (CanRightPlace && RightWallMount != null)
                        {
                            RightWallMount.SetActive(true);
                            GameObject NewPlaced = Instantiate(RightWallMount, new Vector2(Mathf.Round(mousePos.x), Mathf.Round(mousePos.y)), Quaternion.Euler(0, 0, 0));
                            Ref.grid.FurnitureIDS[(int)Mathf.Round(mousePos.x) + (int)Mathf.Round(mousePos.y) * Ref.grid.WorldWidth] = FurnitureID;
                            ItemNum--;
                            RightWallMount.SetActive(false);
                        }
                        else if (CanGroundPlace && GroundPlaced != null)
                        {
                            GroundPlaced.SetActive(true);
                            GameObject NewPlaced = Instantiate(GroundPlaced, new Vector2(Mathf.Round(mousePos.x), Mathf.Round(mousePos.y)), Quaternion.Euler(0, 0, 0));
                            Ref.grid.FurnitureIDS[(int)Mathf.Round(mousePos.x) + (int)Mathf.Round(mousePos.y) * Ref.grid.WorldWidth] = FurnitureID;
                            ItemNum--;
                            GroundPlaced.SetActive(false);
                        }
                        else if (CanBackPlace && BackgroundMount != null)
                        {
                            BackgroundMount.SetActive(true);
                            GameObject NewPlaced = Instantiate(BackgroundMount, new Vector2(Mathf.Round(mousePos.x), Mathf.Round(mousePos.y)), Quaternion.Euler(0, 0, 0));
                            Ref.grid.FurnitureIDS[(int)Mathf.Round(mousePos.x) + (int)Mathf.Round(mousePos.y) * Ref.grid.WorldWidth] = FurnitureID;
                            ItemNum--;
                            BackgroundMount.SetActive(false);
                        }
                        else if (CanTopPlace && CeilingMount != null)
                        {
                            CeilingMount.SetActive(true);
                            GameObject NewPlaced = Instantiate(CeilingMount, new Vector2(Mathf.Round(mousePos.x), Mathf.Round(mousePos.y)), Quaternion.Euler(0, 0, 0));
                            Ref.grid.FurnitureIDS[(int)Mathf.Round(mousePos.x) + (int)Mathf.Round(mousePos.y) * Ref.grid.WorldWidth] = FurnitureID;
                            ItemNum--;
                            CeilingMount.SetActive(false);
                        }
                    }

                    if (Input.GetKeyDown(KeyCode.E) && ArmorType > 0)
                    {
                        if (ArmorType == 1)
                        {
                            for (int i = 0; i < PlayerS.Legs.Length; i++)
                            {
                                PlayerS.Legs[i].Defence += Defence;
                                PlayerS.Legs[i].MaxHealth += Health;
                                PlayerS.Legs[i].GetComponentsInChildren<SpriteRenderer>()[1].sprite = ArmorSprites[i];
                            }
                            Ref.Armor.SlotIDS[2] = ItemID;
                        }
                        else if (ArmorType == 2)
                        {
                            for (int i = 0; i < PlayerS.Chest.Length; i++)
                            {
                                PlayerS.Chest[i].Defence += Defence;
                                PlayerS.Chest[i].MaxHealth += Health;
                                PlayerS.Chest[i].GetComponentsInChildren<SpriteRenderer>()[1].sprite = ArmorSprites[i];
                            }
                            Ref.Armor.SlotIDS[1] = ItemID;
                        }
                        else if (ArmorType == 3)
                        {
                            PlayerS.Head.Defence += Defence;
                            PlayerS.Head.MaxHealth += Health;
                            PlayerS.Head.GetComponentsInChildren<SpriteRenderer>()[1].sprite = ArmorSprites[0];

                            Ref.Armor.SlotIDS[0] = ItemID;
                        }
                        HandS.Active = true;
                        Destroy(this.gameObject);

                        PlayerS.LeftID = 0;

                        PlayerS.LeftMiningDamage = 0;

                        HandS.HeldID = 0;

                        PlayerS.LeftItem = null;
                    }
                    if (Input.GetKeyDown(KeyCode.E) && ESkill != null && ETimer <= 0 && EHold == false && PlayerS.Mana - EManaCost >= 0 || Input.GetKey(KeyCode.E) && ESkill != null && ETimer <= 0 && EHold == true && PlayerS.Mana - EManaCost >= 0)
                    {
                        PlayerS.Mana -= EManaCost;

                        if (Consumable)
                            ItemNum--;

                        Instantiate(ESkill, transform.position, transform.rotation).GetComponent<GetWeapon>().Creator = this;
                        ETimer = ECooldown;
                    }
                    else if (Input.GetKeyDown(KeyCode.LeftShift) && ShiftSkill != null && ShiftTimer < 0 && ShiftHold == false && PlayerS.Mana - ShiftManaCost >= 0 || Input.GetKey(KeyCode.LeftShift) && ShiftSkill != null && ShiftTimer < 0 && ShiftHold == true && PlayerS.Mana - ShiftManaCost >= 0)
                    {
                         PlayerS.Mana -= ShiftManaCost;

                        if (Consumable)
                            ItemNum--;

                        Instantiate(ShiftSkill, transform.position, transform.rotation).GetComponent<GetWeapon>().Creator = this;
                        ShiftTimer = ShiftCooldown;
                    }

                    if (Input.GetKeyDown(KeyCode.T))
                    {
                        int PreNum = Ref.CanAdd(ItemID, ItemNum);
                        if (PreNum > 0)
                        {
                            Ref.AddItems(ItemID, PreNum);
                            ItemNum -= PreNum;
                            PlayerS.PlayFail = false;

                            Instantiate(Inventory.SpawnPart, transform.position, transform.rotation);

                            return;
                        }
                        else
                        {
                            PlayerS.PlayFail = true;
                            Instantiate(Inventory.FailSpawnPart, transform.position, transform.rotation);
                        }
                    }
                }
                if (Input.GetKey(KeyCode.Mouse1) && !LeftHand)
                {
                    if (GhostObject != null)
                    {
                        GhostObject.SetActive(true);
                        GhostObject.transform.position = new Vector2(Mathf.Round(mousePos.x + Width - 1), Mathf.Round(mousePos.y));
                        GhostObject.transform.rotation = Quaternion.Euler(0, 0, 0);
                    }

                    if (Input.GetKeyDown(KeyCode.E) && IsFurniture)
                    {
                        bool CanGroundPlace = true;
                        bool CanLeftPlace = true;
                        bool CanRightPlace = true;
                        bool CanTopPlace = true;
                        bool CanBackPlace = true;

                        for (int i = 0; i < Height; i++)
                        {
                            for (int j = 0; j < Width; j++)
                            {
                                if (Ref.grid.CheckTile(j + (int)Mathf.Round(mousePos.x), i + (int)Mathf.Round(mousePos.y)) > 0)
                                {
                                    CanGroundPlace = false;
                                    CanLeftPlace = false;
                                    CanRightPlace = false;
                                    CanTopPlace = false;
                                    CanBackPlace = false;
                                }
                                else if (Physics2D.OverlapCircle(new Vector2(j + (int)Mathf.Round(mousePos.x), i + (int)Mathf.Round(mousePos.y)), 0.45f, Ref.FurnitureMask))
                                {
                                    CanGroundPlace = false;
                                    CanLeftPlace = false;
                                    CanRightPlace = false;
                                    CanTopPlace = false;
                                    CanBackPlace = false;
                                }
                            }
                        }

                        for (int i = 0; i < Width; i++)
                        {
                            if (Ref.grid.CheckTile(i + (int)Mathf.Round(mousePos.x), (int)Mathf.Round(mousePos.y) - 1) == 0 || Ref.grid.TileTypes[Ref.grid.CheckTile(i + (int)Mathf.Round(mousePos.x), (int)Mathf.Round(mousePos.y) - 1)].IsLiquid)
                            {
                                CanGroundPlace = false;
                            }
                        }
                        for (int i = 0; i < Width; i++)
                        {
                            if (Ref.grid.CheckTile(i + (int)Mathf.Round(mousePos.x), (int)Mathf.Round(mousePos.y) + Height) == 0 || Ref.grid.TileTypes[Ref.grid.CheckTile(i + (int)Mathf.Round(mousePos.x), (int)Mathf.Round(mousePos.y) + Height)].IsLiquid)
                            {
                                CanTopPlace = false;
                            }
                        }
                        for (int i = 0; i < Height; i++)
                        {
                            if (Ref.grid.CheckTile((int)Mathf.Round(mousePos.x) - 1, i + (int)Mathf.Round(mousePos.y)) == 0 || Ref.grid.TileTypes[Ref.grid.CheckTile((int)Mathf.Round(mousePos.x) - 1, i + (int)Mathf.Round(mousePos.y))].IsLiquid)
                            {
                                CanLeftPlace = false;
                            }
                        }
                        for (int i = 0; i < Height; i++)
                        {
                            if (Ref.grid.CheckTile((int)Mathf.Round(mousePos.x) + Width, i + (int)Mathf.Round(mousePos.y)) == 0 || Ref.grid.TileTypes[Ref.grid.CheckTile((int)Mathf.Round(mousePos.x) - Width, i + (int)Mathf.Round(mousePos.y))].IsLiquid)
                            {
                                CanRightPlace = false;
                            }
                        }
                        for (int i = 0; i < Height; i++)
                        {
                            for (int j = 0; j < Width; j++)
                            {
                                if (Ref.grid.CheckBackTile(j + (int)Mathf.Round(mousePos.x), i + (int)Mathf.Round(mousePos.y)) == 0)
                                {
                                    CanBackPlace = false;
                                }
                            }
                        }

                        if (CanLeftPlace && LeftWallMount != null)
                        {
                            LeftWallMount.SetActive(true);
                            GameObject NewPlaced = Instantiate(LeftWallMount, new Vector2(Mathf.Round(mousePos.x), Mathf.Round(mousePos.y)), Quaternion.Euler(0, 0, 0));
                            Ref.grid.FurnitureIDS[(int)Mathf.Round(mousePos.x) + (int)Mathf.Round(mousePos.y) * Ref.grid.WorldWidth] = FurnitureID;
                            ItemNum--;
                            LeftWallMount.SetActive(false);
                        }
                        else if (CanRightPlace && RightWallMount != null)
                        {
                            RightWallMount.SetActive(true);
                            GameObject NewPlaced = Instantiate(RightWallMount, new Vector2(Mathf.Round(mousePos.x), Mathf.Round(mousePos.y)), Quaternion.Euler(0, 0, 0));
                            Ref.grid.FurnitureIDS[(int)Mathf.Round(mousePos.x) + (int)Mathf.Round(mousePos.y) * Ref.grid.WorldWidth] = FurnitureID;
                            ItemNum--;
                            RightWallMount.SetActive(false);
                        }
                        else if (CanGroundPlace && GroundPlaced != null)
                        {
                            GroundPlaced.SetActive(true);
                            GameObject NewPlaced = Instantiate(GroundPlaced, new Vector2(Mathf.Round(mousePos.x), Mathf.Round(mousePos.y)), Quaternion.Euler(0, 0, 0));
                            Ref.grid.FurnitureIDS[(int)Mathf.Round(mousePos.x) + (int)Mathf.Round(mousePos.y) * Ref.grid.WorldWidth] = FurnitureID;
                            ItemNum--;
                            GroundPlaced.SetActive(false);
                        }
                        else if (CanBackPlace && BackgroundMount != null)
                        {
                            BackgroundMount.SetActive(true);
                            GameObject NewPlaced = Instantiate(BackgroundMount, new Vector2(Mathf.Round(mousePos.x), Mathf.Round(mousePos.y)), Quaternion.Euler(0, 0, 0));
                            Ref.grid.FurnitureIDS[(int)Mathf.Round(mousePos.x) + (int)Mathf.Round(mousePos.y) * Ref.grid.WorldWidth] = FurnitureID;
                            ItemNum--;
                            BackgroundMount.SetActive(false);
                        }
                        else if (CanTopPlace && CeilingMount != null)
                        {
                            CeilingMount.SetActive(true);
                            GameObject NewPlaced = Instantiate(CeilingMount, new Vector2(Mathf.Round(mousePos.x), Mathf.Round(mousePos.y)), Quaternion.Euler(0, 0, 0));
                            Ref.grid.FurnitureIDS[(int)Mathf.Round(mousePos.x) + (int)Mathf.Round(mousePos.y) * Ref.grid.WorldWidth] = FurnitureID;
                            ItemNum--;
                            CeilingMount.SetActive(false);
                        }
                    }

                    if (Input.GetKeyDown(KeyCode.E) && ArmorType > 0)
                    {
                        if (ArmorType == 1)
                        {
                            for (int i = 0; i < PlayerS.Legs.Length; i++)
                            {
                                PlayerS.Legs[i].Defence += Defence;
                                PlayerS.Legs[i].MaxHealth += Health;
                                PlayerS.Legs[i].GetComponentsInChildren<SpriteRenderer>()[1].sprite = ArmorSprites[i];
                            }
                            Ref.Armor.SlotIDS[2] = ItemID;
                        }
                        else if (ArmorType == 2)
                        {
                            for (int i = 0; i < PlayerS.Chest.Length; i++)
                            {
                                PlayerS.Chest[i].Defence += Defence;
                                PlayerS.Chest[i].MaxHealth += Health;
                                PlayerS.Chest[i].GetComponentsInChildren<SpriteRenderer>()[1].sprite = ArmorSprites[i];
                            }
                            Ref.Armor.SlotIDS[1] = ItemID;
                        }
                        else
                        {
                            PlayerS.Head.Defence += Defence;
                            PlayerS.Head.MaxHealth += Health;
                            PlayerS.Head.GetComponentsInChildren<SpriteRenderer>()[1].sprite = ArmorSprites[0];

                            Ref.Armor.SlotIDS[0] = ItemID;
                        }
                        HandS.Active = true;
                        Destroy(this.gameObject);

                        PlayerS.RightID = 0;

                        PlayerS.RightMiningDamage = 0;

                        HandS.HeldID = 0;

                        PlayerS.RightItem = null;
                    }
                    if (Input.GetKeyDown(KeyCode.E) && ESkill != null && ETimer <= 0 && EHold == false && PlayerS.Mana - EManaCost >= 0 || Input.GetKey(KeyCode.E) && ESkill != null && ETimer <= 0 && EHold == true && PlayerS.Mana - EManaCost >= 0)
                    {
                        PlayerS.Mana -= EManaCost;

                        if (Consumable)
                            ItemNum--;

                        Instantiate(ESkill, transform.position, transform.rotation).GetComponent<GetWeapon>().Creator = this;
                        ETimer = ECooldown;
                    }
                    else if (Input.GetKeyDown(KeyCode.LeftShift) && ShiftSkill != null && ShiftTimer < 0 && ShiftHold == false && PlayerS.Mana - ShiftManaCost >= 0 || Input.GetKey(KeyCode.LeftShift) && ShiftSkill != null && ShiftTimer < 0 && ShiftHold == true && PlayerS.Mana - ShiftManaCost >= 0)
                    {
                        PlayerS.Mana -= ShiftManaCost;

                        if (Consumable)
                            ItemNum--;

                        Instantiate(ShiftSkill, transform.position, transform.rotation).GetComponent<GetWeapon>().Creator = this;
                        ShiftTimer = ShiftCooldown;
                    }

                    if (Input.GetKeyDown(KeyCode.T))
                    {
                        int PreNum = Ref.CanAdd(ItemID, ItemNum);
                        if (PreNum > 0)
                        {
                            Ref.AddItems(ItemID, PreNum);
                            ItemNum -= PreNum;
                            PlayerS.PlayFail = false;

                            Instantiate(Inventory.SpawnPart, transform.position, transform.rotation);

                            return;
                        }
                        else
                        {
                            PlayerS.PlayFail = true;
                            Instantiate(Inventory.FailSpawnPart, transform.position, transform.rotation);
                        }
                    }
                }

                if (Input.GetKey(KeyCode.Q) && Held == true)
                {
                    if (Input.GetKeyUp(KeyCode.Mouse0) && LeftHand)
                    {
                        PlayerS.LeftECool.SetValue(0);

                        PlayerS.LeftShiftCool.SetValue(0);

                        Destroy(GetComponent<FixedJoint2D>());
                        HandS.Active = true;
                        Held = false;
                        NoColl(false);

                        PlayerS.LeftID = 0;

                        PlayerS.LeftMiningDamage = 0;
                        PlayerS.LeftMiningCooldown = 0;
                        PlayerS.LeftMiningPower = 0;

                        HandS.HeldID = 0;

                        PlayerS.LeftItem = null;
                    }
                    else if (Input.GetKeyUp(KeyCode.Mouse1) && LeftHand == false)
                    {
                        PlayerS.RightECool.SetValue(0);

                        PlayerS.RightShiftCool.SetValue(0);

                        Destroy(GetComponent<FixedJoint2D>());
                        HandS.Active = true;
                        Held = false;
                        NoColl(false);

                        PlayerS.RightID = 0;

                        PlayerS.RightMiningDamage = 0;
                        PlayerS.RightMiningCooldown = 0;
                        PlayerS.RightMiningPower = 0;

                        HandS.HeldID = 0;

                        PlayerS.RightItem = null;
                    }
                }
            }
            else
            {
                if (this != null && Input.GetKey(KeyCode.R) && Physics2D.OverlapCircle(transform.position, 3, Ref.PlayerMask))
                {
                    int PreNum = Ref.CanAdd(ItemID, ItemNum);
                    if (PreNum > 0)
                    {
                        ItemNum -= PreNum;
                        Ref.AddItems(ItemID, PreNum);

                        Instantiate(Inventory.SpawnPart, transform.position, transform.rotation);

                        return;
                    }
                    else
                    {
                        PlayerS.PlayFail = true;
                    }
                }
                if (Sounds != null)
                {
                    Sounds.enabled = false;
                }
            }
        }

        if (LeftHand && Grabable && Held)
        {
            PlayerS.LeftECool.SetMaxValue(ECooldown);
            PlayerS.LeftECool.SetValue(ETimer);

            PlayerS.LeftShiftCool.SetMaxValue(ShiftCooldown);
            PlayerS.LeftShiftCool.SetValue(ShiftTimer);
        }
        else if (!LeftHand && Grabable && Held)
        {
            PlayerS.RightECool.SetMaxValue(ECooldown);
            PlayerS.RightECool.SetValue(ETimer);

            PlayerS.RightShiftCool.SetMaxValue(ShiftCooldown);
            PlayerS.RightShiftCool.SetValue(ShiftTimer);
        }
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        rb2 = collision.rigidbody;

        Vector2 normal = collision.GetContact(0).normal;
        Vector2 impulse = ComputeTotalImpulse(collision);

        // Both bodies see the same impulse. Flip it for one of the bodies.
        if (Vector3.Dot(normal, impulse) < 0f)
            impulse *= -1f;

        Vector2 myIncidentVelocity = rb2.velocity - impulse / rb2.mass;

        Vector2 otherIncidentVelocity = Vector3.zero;
        var otherBody = rb;

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

        if (Held && OnHitParts != null && !collision.gameObject.CompareTag("Ground") && !collision.gameObject.CompareTag("Item") && !collision.gameObject.CompareTag("Weapon"))
        {
            OnHitParts.Play();

            if (damage >= HeavyForce && HeavySkill != null)
            {
                Instantiate(HeavySkill, transform.position, transform.rotation).GetComponent<GetWeapon>().Creator = this;
            }

            if (HitManaMod > 0)
            {
                PlayerS.Mana += damage * HitManaMod / 10;
            }
        }

        if (this != null && Grabable && PlayerS.Health > 0)
        {
            if (collision.gameObject.tag == "Left Hand" && Input.GetKey(KeyCode.Mouse0) && Held == false)
            {
                HandS = collision.gameObject.GetComponent<Grab>();
                if (HandS != null && HandS.Active)
                {
                    PlayerS.LeftItem = GetComponent<item>();

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

                    PlayerS.LeftID = BlockID;

                    HandS.HeldID = ItemID;
                    HandS.HeldAmount = ItemNum;

                    transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
                }
            }
            else if (collision.gameObject.tag == "Right Hand" && Input.GetKey(KeyCode.Mouse1) && Held == false)
            {
                HandS = collision.gameObject.GetComponent<Grab>();
                if (HandS != null && HandS.Active)
                {
                    PlayerS.RightItem = GetComponent<item>();

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

                    PlayerS.RightID = BlockID;

                    HandS.HeldID = ItemID;
                    HandS.HeldAmount = ItemNum;

                    transform.localScale = new Vector2(-Mathf.Abs(transform.localScale.x), transform.localScale.y);
                }
            }
        }
    }
}