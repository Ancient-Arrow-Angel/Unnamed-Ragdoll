using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class FurniturePlace : MonoBehaviour
{
    [Header("Stats")]
    public int Items;
    public int ItemID;

    [Header("Other")]
    public Vector2 CheckOffset;
    public Vector2 CheckSize;
    public bool Grabable = true;
    public GameObject OnObject;
    public SpriteRenderer GhostObject;
    public LayerMask EverthingBut;

    bool Held = false;
    bool LeftHand = true;

    Grab HandS;
    Color GhostColor;

    Rigidbody2D rb;
    Collider2D Coll;
    Collider2D Coll2;

    Camera cam;
    float offset = 180;
    public int speed = 50;

    GameObject PlayerOB;
    Player PlayerS;

    bool InGround;

    SlotMaker Inventory;
    Reference Ref;
    TileMaker grid;

    void NoColl(bool Active)
    {
        ItemID += Items;

        grid = GameObject.Find("Grid").GetComponent<TileMaker>();

        Collider2D[] Colliders = PlayerOB.transform.GetComponentsInChildren<Collider2D>();
        for (int i = 0; i < Colliders.Length; i++)
        {
            for (int k = i; k < Colliders.Length; k++)
            {
                Physics2D.IgnoreCollision(Colliders[i], Coll, Active);
                if (Coll2 != null)
                    Physics2D.IgnoreCollision(Colliders[i], Coll2, Active);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Ref = GameObject.Find("Global Ref").GetComponent<Reference>();
        Inventory = Ref.Inventory;

        rb = GetComponent<Rigidbody2D>();
        Coll = GetComponent<Collider2D>();
        Coll2 = GetComponent<Collider2D>();
        cam = Camera.main;
        PlayerOB = GameObject.Find("Player");
        PlayerS = PlayerOB.GetComponent<Player>();

        GhostColor = Color.white;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Grabable)
        {
            if (Held)
            {
                if (Input.GetKey(KeyCode.Mouse0) && LeftHand)
                {
                    if (Input.GetKey(KeyCode.T))
                    {
                        Ref.AddItem(ItemID, 1);
                        HandS.Active = true;
                        Destroy(this.gameObject);
                    }

                    if (Input.GetKey(KeyCode.E) && !Physics2D.OverlapBox(new Vector2(Mathf.Round(worldPosition.x) + CheckOffset.x, Mathf.Round(worldPosition.y) + CheckOffset.y), CheckSize, 0, EverthingBut) && Mathf.Round(worldPosition.y) <= grid.WorldHeight && !grid.TileTypes[grid.CheckTile((int)Mathf.Round(worldPosition.x), (int)Mathf.Round(worldPosition.y) - 1)].IsLiquid && !grid.TileTypes[grid.CheckTile((int)Mathf.Round(worldPosition.x) + 1, (int)Mathf.Round(worldPosition.y) - 1)].IsLiquid)
                    {
                        transform.parent.position = new Vector2(Mathf.Round(worldPosition.x), Mathf.Round(worldPosition.y));
                        OnObject.SetActive(true);
                        Destroy(GetComponent<FixedJoint2D>());
                        HandS.Active = true;
                        Held = false;
                        NoColl(false);
                        GhostObject.gameObject.SetActive(false);
                        this.gameObject.SetActive(false);
                    }
                }
                if (Input.GetKey(KeyCode.Mouse1) && !LeftHand)
                {
                    if (Input.GetKeyDown(KeyCode.T))
                    {
                        Ref.AddItem(ItemID, 1);
                        HandS.Active = true;
                        Destroy(this.gameObject);
                    }

                    if (Input.GetKey(KeyCode.E) && !Physics2D.OverlapBox(new Vector2(Mathf.Round(worldPosition.x) + CheckOffset.x, Mathf.Round(worldPosition.y) + CheckOffset.y), CheckSize, 0, EverthingBut) && Mathf.Round(worldPosition.y) <= grid.WorldHeight && !grid.TileTypes[grid.CheckTile((int)Mathf.Round(worldPosition.x), (int)Mathf.Round(worldPosition.y) - 1)].IsLiquid && !grid.TileTypes[grid.CheckTile((int)Mathf.Round(worldPosition.x) + 1, (int)Mathf.Round(worldPosition.y) - 1)].IsLiquid)
                    {
                        transform.parent.position = new Vector2(Mathf.Round(worldPosition.x), Mathf.Round(worldPosition.y));
                        OnObject.SetActive(true);
                        Destroy(GetComponent<FixedJoint2D>());
                        HandS.Active = true;
                        Held = false;
                        NoColl(false);
                        GhostObject.gameObject.SetActive(false);
                        this.gameObject.SetActive(false);
                    }
                }

                if (!Physics2D.OverlapBox(new Vector2(Mathf.Round(worldPosition.x) + CheckOffset.x, Mathf.Round(worldPosition.y) + CheckOffset.y), CheckSize, 0, EverthingBut) && Mathf.Round(worldPosition.y) <= grid.WorldHeight && !grid.TileTypes[grid.CheckTile((int)Mathf.Round(worldPosition.x), (int)Mathf.Round(worldPosition.y) -1)].IsLiquid && !grid.TileTypes[grid.CheckTile((int)Mathf.Round(worldPosition.x) + 1, (int)Mathf.Round(worldPosition.y) - 1)].IsLiquid)
                    GhostColor = Color.green;
                else
                    GhostColor = Color.red;
            }
            else
            {
                GhostColor = new Color(0, 0, 0, 0);
            }

            GhostObject.color = GhostColor;

            if (Input.GetKey(KeyCode.Q) && Held == true)
            {
                if (Input.GetKeyUp(KeyCode.Mouse0) && LeftHand)
                {
                    Destroy(GetComponent<FixedJoint2D>());
                    HandS.Active = true;
                    Held = false;
                    NoColl(false);
                }
                else if (Input.GetKeyUp(KeyCode.Mouse1) && LeftHand == false)
                {
                    Destroy(GetComponent<FixedJoint2D>());
                    HandS.Active = true;
                    Held = false;
                    NoColl(false);
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

            //if (LeftHand)
            //{
            //    transform.localScale = new Vector3(1, 1, 1);
            //}
            //else if (LeftHand == false)
            //{
            //    transform.localScale = new Vector3(-1, 1, 1);
            //}
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (Grabable)
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
                }
            }
        }
    }
}