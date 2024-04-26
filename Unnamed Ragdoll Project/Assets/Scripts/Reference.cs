using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class Reference : MonoBehaviour
{
    public List<ItemType> Items;

    public CraftingMenu Craft;
    public SlotMaker Inventory;
    public TileMaker grid;
    public GameObject Bass;

    public LayerMask PlayerMask;
    public LayerMask GroundMask;
    public LayerMask FurnitureMask;
    public LayerMask ItemMask;
    public int TileRange;
    public int CameraSize;

    public int ChunkSize;
    public int TileLoadOffset;

    public int StackAmount;

    public ArmorHandle Armor;
    public Player player;

    public float ItemCombineRange;

    public bool TilesChanged;
    public float CraftingRange;
    public int NonTileItems;

    private void Awake()
    {
        Physics2D.reuseCollisionCallbacks = true;

        NonTileItems = Items.Count;

        Inventory = GameObject.Find("Slot Maker").GetComponent<SlotMaker>();
        grid = GameObject.Find("Grid").GetComponent<TileMaker>();

        Armor = GameObject.Find("Armor Handle").GetComponent<ArmorHandle>();

        for (int i = 1; i < grid.TileTypes.Length; i++)
        {
            GameObject TileItem = Instantiate(Bass, new Vector3(999999, 999999, 999999), transform.rotation);
            TileItem.GetComponent<item>().ItemID = Items.Count;
            TileItem.GetComponent<item>().BlockID = i;
            TileItem.GetComponent<SpriteRenderer>().sprite = grid.TileTypes[i].tile.sprite;
            TileItem.name = grid.TileTypes[i].tile.sprite.name;

            Items.Add(new ItemType());
            Items[Items.Count - 1].Stackable = true;
            Items[Items.Count - 1].icon = grid.TileTypes[i].tile.sprite;
            Items[Items.Count - 1].Item = TileItem;

            if (grid.TileTypes[i].Drop == null && !grid.TileTypes[i].DontDrop)
            {
                grid.TileTypes[i].Drop = TileItem;
            }
        }

        for (int i = 1; i < grid.FurnitureTypes.Length; i++)
        {
            grid.FurnitureTypes[i].FurnitureID = i;

            if (grid.FurnitureTypes[i].LeftWallMount != null)
            {
                grid.FurnitureTypes[i].LeftWallMount.GetComponent<Furniture>().ID = i;
            }
            if (grid.FurnitureTypes[i].RightWallMount != null)
            {
                grid.FurnitureTypes[i].RightWallMount.GetComponent<Furniture>().ID = i;
            }
            if (grid.FurnitureTypes[i].GroundPlaced != null)
            {
                grid.FurnitureTypes[i].GroundPlaced.GetComponent<Furniture>().ID = i;
            }
            if (grid.FurnitureTypes[i].BackgroundMount != null)
            {
                grid.FurnitureTypes[i].BackgroundMount.GetComponent<Furniture>().ID = i;
            }
            if (grid.FurnitureTypes[i].CeilingMount != null)
            {
                grid.FurnitureTypes[i].CeilingMount.GetComponent<Furniture>().ID = i;
            }
        }

        for (int i = 1; i < Items.Count; i++)
        {
            Items[i].Item.GetComponent<item>().ItemID = i;

            Items[i].Description = "\n";

            if (Items[i].Item.GetComponent<item>().HeavyDamage > 0)
                Items[i].Description += "\n" + (Items[i].Item.GetComponent<item>().HeavyDamage * player.DamageMultiplier * player.HeavyDamageMultiplier).ToString() + " Heavy Damage";
            if (Items[i].Item.GetComponent<item>().LightDamage > 0)
                Items[i].Description += "\n" + (Items[i].Item.GetComponent<item>().LightDamage * player.DamageMultiplier * player.LightDamageMultiplier).ToString() + " Light Damage";
            if (Items[i].Item.GetComponent<item>().MagicDamage > 0)
                Items[i].Description += "\n" + (Items[i].Item.GetComponent<item>().MagicDamage * player.DamageMultiplier * player.MagicDamageMultiplier).ToString() + " Magic Damage";

            if (Items[i].Item.GetComponent<item>().MiningDamage > 0)
            {
                Items[i].Description += "\n" + Items[i].Item.GetComponent<item>().MiningDamage.ToString() + " Mining Damage";
            }
            if (Items[i].Item.GetComponent<item>().MiningPower > 0)
            {
                Items[i].Description += "\n" + Items[i].Item.GetComponent<item>().MiningPower.ToString() + " Mining Power";
            }
        }
    }

    private void FixedUpdate()
    {

    }

    public int CanAdd(int ID, int Amount)
    {
        int Added = 0;
        int[] SlotNums = null;

        SlotNums = (int[])Inventory.SlotNumbers.Clone();

        for (int j = 0; j < Amount; j++)
        {
            for (int i = 1; i < Inventory.SlotIDs.Length; i++)
            {
                if (Inventory.SlotIDs[i] == ID && Items[Inventory.SlotIDs[i]].Stackable && SlotNums[i] < StackAmount)
                {
                    Added++;
                    SlotNums[i] += 1;
                    i = 99999;
                }

                else if (Inventory.SlotIDs[i] == 0)
                {
                    Added++;
                    SlotNums[i] += 1;
                    i = 99999;
                }
            }
        }

        return Added;
    }

    public void AddItem(int ID)
    {
        int EarliestEmpty = -1;

        for (int i = 1; i < Inventory.SlotIDs.Length; i++)
        {
            if (Inventory.SlotIDs[i] == 0 && ID > 0)
            {
                Inventory.SlotIDs[i] = ID;
                Inventory.SlotNumbers[i] = 1;
                return;
            }
            else if (Inventory.SlotIDs[i] == ID &&
                Items[ID].Stackable &&
                Inventory.SlotNumbers[i] < StackAmount)
            {
                Inventory.SlotNumbers[i] += 1;
                return;
            }
        }
    }

    public void AddItems(int ID, int Number)
    {
        for (int i = 0; i < Number; i++)
        {
            AddItem(ID);
        }
    }

    void RemoveItem(int ID)
    {
        for (int i = 1; i < Inventory.SlotIDs.Length; i++)
        {
            if (Inventory.SlotIDs[i] == ID && Inventory.SlotNumbers[i] > 0)
            {
                Inventory.SlotNumbers[i]--;
                return;
            }
        }
    }

    public void RemoveItems(int ID, int Amount)
    {
        for (int i = 0; i < Amount; i++)
        {
            RemoveItem(ID);
        }
    }

    public void Load(GameObject ToLoad)
    {
        ToLoad.SetActive(true);
    }

    public void Unload(GameObject ToUnload)
    {
        ToUnload.SetActive(false);
    }
}

[System.Serializable]
public class ItemType
{
    public GameObject Item;
    [Multiline]
    public string Description;
    public Sprite icon;
    public bool Stackable;
}