using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Furniture : MonoBehaviour
{
    public int ID;
    public int[] Craftable;
    item FurnitureItem;
    Player player;
    public Reference Ref;
    float TimeAlive;

    // Start is called before the first frame update
    void Start()
    {
        Ref = GameObject.Find("Global Ref").GetComponent<Reference>();
        player = GameObject.Find("Player").GetComponent<Player>();

        transform.parent = null;

        FurnitureItem = Ref.grid.FurnitureTypes[ID];
    }
    private void Update()
    {
        if (Craftable.Length > 0)
        {
            if (player.rb.transform.position.x > transform.position.x - Ref.CraftingRange / 2 &&
                player.rb.transform.position.x < transform.position.x + Ref.CraftingRange / 2 &&
                player.rb.transform.position.y > transform.position.y - Ref.CraftingRange / 2 &&
                player.rb.transform.position.y < transform.position.y + Ref.CraftingRange / 2)
            {
                for (int i = 0; i < Craftable.Length; i++)
                {
                    bool CanAdd = true;
                    for (int j = 0; j < Ref.Craft.Craftable.Count; j++)
                    {
                        for (int k = 0; k < Craftable.Length; k++)
                        {
                            if (Craftable[k] == Ref.Craft.Craftable[j])
                            {
                                CanAdd = false;
                                j = 9999999;
                            }
                        }
                    }

                    if(CanAdd)
                        Ref.Craft.Craftable.Add(Craftable[i]);
                }
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        TimeAlive += Time.deltaTime;

        Vector2 mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        if (transform.position.x + Ref.TileRange / 2 < player.rb.transform.position.x ||
           transform.position.x - Ref.TileRange / 2 > player.rb.transform.position.x ||
           transform.position.y + Ref.TileRange / 2 < player.rb.transform.position.y ||
           transform.position.y - Ref.TileRange / 2 > player.rb.transform.position.y)
        {
            Destroy(this.gameObject);
        }

        if(Input.GetMouseButton(0) && Input.GetKey(KeyCode.E) && player.LeftMiningPower >= 0 && TimeAlive > 3)
        {
            if (Mathf.Round(mousePos.x) >= transform.position.x &&
                Mathf.Round(mousePos.x) <= transform.position.x + FurnitureItem.Width - 1 &&
                Mathf.Round(mousePos.y) >= transform.position.y &&
                Mathf.Round(mousePos.y) <= transform.position.y + FurnitureItem.Height - 1)
            {
                Instantiate(FurnitureItem, new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y)), Quaternion.Euler(0, 0, 0));
                Ref.grid.FurnitureIDS[(int)Mathf.Round(transform.position.x) + (int)Mathf.Round(transform.position.y) * Ref.grid.WorldWidth] = 0;
                Destroy(this.gameObject);
            }
        }
        else if (Input.GetMouseButton(1) && Input.GetKey(KeyCode.E) && player.RightMiningPower >= 0 && TimeAlive > 3)
        {
            if (Mathf.Round(mousePos.x) >= transform.position.x &&
                Mathf.Round(mousePos.x) <= transform.position.x + FurnitureItem.Width - 1 &&
                Mathf.Round(mousePos.y) >= transform.position.y &&
                Mathf.Round(mousePos.y) <= transform.position.y + FurnitureItem.Height - 1)
            {
                Instantiate(FurnitureItem, new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y)), Quaternion.Euler(0, 0, 0));
                Ref.grid.FurnitureIDS[(int)Mathf.Round(transform.position.x) + (int)Mathf.Round(transform.position.y) * Ref.grid.WorldWidth] = 0;
                Destroy(this.gameObject);
            }
        }

        if (Ref.TilesChanged)
        {
            bool CanGroundPlace = true;
            bool CanLeftPlace = true;
            bool CanRightPlace = true;
            bool CanTopPlace = true;
            bool CanBackPlace = true;

            for (int i = 0; i < FurnitureItem.Height; i++)
            {
                for (int j = 0; j < FurnitureItem.Width; j++)
                {
                    if (Ref.grid.CheckTile(j + (int)Mathf.Round(transform.position.x), i + (int)Mathf.Round(transform.position.y)) > 0)
                    {
                        CanGroundPlace = false;
                        CanLeftPlace = false;
                        CanRightPlace = false;
                        CanTopPlace = false;
                        CanBackPlace = false;
                    }
                }
            }

            for (int i = 0; i < FurnitureItem.Width; i++)
            {
                if (Ref.grid.CheckTile(i + (int)Mathf.Round(transform.position.x), (int)Mathf.Round(transform.position.y) - 1) == 0 || Ref.grid.TileTypes[Ref.grid.CheckTile(i + (int)Mathf.Round(transform.position.x), (int)Mathf.Round(transform.position.y) - 1)].IsLiquid)
                {
                    CanGroundPlace = false;
                }
            }
            for (int i = 0; i < FurnitureItem.Width; i++)
            {
                if (Ref.grid.CheckTile(i + (int)Mathf.Round(transform.position.x), (int)Mathf.Round(transform.position.y) + FurnitureItem.Height) == 0 || Ref.grid.TileTypes[Ref.grid.CheckTile(i + (int)Mathf.Round(transform.position.x), (int)Mathf.Round(transform.position.y) + FurnitureItem.Height)].IsLiquid)
                {
                    CanTopPlace = false;
                }
            }
            for (int i = 0; i < FurnitureItem.Height; i++)
            {
                if (Ref.grid.CheckTile((int)Mathf.Round(transform.position.x) - 1, i + (int)Mathf.Round(transform.position.y)) == 0 || Ref.grid.TileTypes[Ref.grid.CheckTile((int)Mathf.Round(transform.position.x) - 1, i + (int)Mathf.Round(transform.position.y))].IsLiquid)
                {
                    CanLeftPlace = false;
                }
            }
            for (int i = 0; i < FurnitureItem.Height; i++)
            {
                if (Ref.grid.CheckTile((int)Mathf.Round(transform.position.x) + FurnitureItem.Width, i + (int)Mathf.Round(transform.position.y)) == 0 || Ref.grid.TileTypes[Ref.grid.CheckTile((int)Mathf.Round(transform.position.x) - FurnitureItem.Width, i + (int)Mathf.Round(transform.position.y))].IsLiquid)
                {
                    CanRightPlace = false;
                }
            }
            for (int i = 0; i < FurnitureItem.Height; i++)
            {
                for (int j = 0; j < FurnitureItem.Width; j++)
                {
                    if (Ref.grid.CheckBackTile(j + (int)Mathf.Round(transform.position.x), i + (int)Mathf.Round(transform.position.y)) == 0)
                    {
                        CanBackPlace = false;
                    }
                }
            }

            if (CanLeftPlace && FurnitureItem.LeftWallMount != null && name != "Left Placed")
            {
                if (name != FurnitureItem.LeftWallMount.name + "(Clone)")
                {
                    FurnitureItem.LeftWallMount.SetActive(true);
                    GameObject NewPlaced = Instantiate(FurnitureItem.LeftWallMount, new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y)), Quaternion.Euler(0, 0, 0));
                    FurnitureItem.LeftWallMount.SetActive(false);
                    Destroy(this.gameObject);
                }
            }
            else if (CanRightPlace && FurnitureItem.RightWallMount != null && name != "Right Placed")
            {
                if (name != FurnitureItem.RightWallMount.name + "(Clone)")
                {
                    FurnitureItem.RightWallMount.SetActive(true);
                    GameObject NewPlaced = Instantiate(FurnitureItem.RightWallMount, new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y)), Quaternion.Euler(0, 0, 0));
                    FurnitureItem.RightWallMount.SetActive(false);
                    Destroy(this.gameObject);
                }
            }
            else if (CanGroundPlace && FurnitureItem.GroundPlaced != null)
            {
                if(name != FurnitureItem.GroundPlaced.name + "(Clone)")
                {
                    if (name != FurnitureItem.GroundPlaced.name + "(Clone)")
                    {
                        FurnitureItem.GroundPlaced.SetActive(true);
                        GameObject NewPlaced = Instantiate(FurnitureItem.GroundPlaced, new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y)), Quaternion.Euler(0, 0, 0));
                        FurnitureItem.GroundPlaced.SetActive(false);
                        Destroy(this.gameObject);
                    }
                }
            }
            else if (CanBackPlace && FurnitureItem.BackgroundMount != null && this.gameObject.name != "Back Placed")
            {
                if (name != FurnitureItem.BackgroundMount.name + "(Clone)")
                {
                    FurnitureItem.BackgroundMount.SetActive(true);
                    GameObject NewPlaced = Instantiate(FurnitureItem.BackgroundMount, new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y)), Quaternion.Euler(0, 0, 0));
                    FurnitureItem.BackgroundMount.SetActive(false);
                    Destroy(this.gameObject);
                }
            }
            else if (CanTopPlace && FurnitureItem.CeilingMount != null && name != "Ceiling Placed")
            {
                if (name != FurnitureItem.CeilingMount.name + "(Clone)")
                {
                    FurnitureItem.CeilingMount.SetActive(true);
                    GameObject NewPlaced = Instantiate(FurnitureItem.CeilingMount, new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y)), Quaternion.Euler(0, 0, 0));
                    FurnitureItem.CeilingMount.SetActive(false);
                    Destroy(this.gameObject);
                }
            }
            else
            {
                Instantiate(FurnitureItem, new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y)), Quaternion.Euler(0, 0, 0));
                Ref.grid.FurnitureIDS[(int)Mathf.Round(transform.position.x) + (int)Mathf.Round(transform.position.y) * Ref.grid.WorldWidth] = 0;
                Destroy(this.gameObject);
            }
        }
    }
}