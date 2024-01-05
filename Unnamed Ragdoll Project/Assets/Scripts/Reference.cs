using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Reference : MonoBehaviour
{
    public List<ItemType> Items;

    public SlotMaker Inventory;
    public TileMaker grid;
    public GameObject Bass;

    private void Start()
    {
        Inventory = GameObject.Find("Slot Maker").GetComponent<SlotMaker>();
        grid = GameObject.Find("Grid").GetComponent<TileMaker>();

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
    }

    public void AddItem(int ID, int Num)
    {
        for (int i = 1; i < Inventory.SlotIDs.Length; i++)
        {
            if (Inventory.SlotIDs[i] == ID && Items[Inventory.SlotIDs[i]].Stackable)
            {
                Inventory.SlotNumbers[i] += Num;
                return;
            }
            else if (Inventory.SlotIDs[i] == 0)
            {
                Inventory.SlotIDs[i] = ID;
                Inventory.SlotNumbers[i] = Num;
                return;
            }
        }
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