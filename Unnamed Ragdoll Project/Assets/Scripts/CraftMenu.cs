using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CraftMenu : MonoBehaviour
{
    public GameObject SelectSlot;

    public GameObject CraftSlot;
    public GameObject CraftIcon;
    public GameObject CraftNum;

    public int MenuLength;
    public int YGap;
    public List<Craft> Crafts;
    public List<int> StartCrafts;
    public List<int> Craftable;
    public SlotMaker Inventory;
    public Reference Ref;

    public Image[] Icons;
    public Image[] Slots;
    public TextMeshProUGUI[] ItemNums;

    public Image[] CraftIcons;
    public Image[] CraftSlots;
    public TextMeshProUGUI[] CraftItemNums;

    public int scroll;
    public Vector2 CraftSlotsOffset;
    public Vector2 CraftSlotsPos;

    public Vector2 CraftNumsPos;

    Vector2 pos;

    int amount;

    void Start()
    {
        Icons = new Image[MenuLength];
        Slots = new Image[MenuLength];
        ItemNums = new TextMeshProUGUI[MenuLength];

        CraftSlots = new Image[20];
        CraftIcons = new Image[20];
        CraftItemNums = new TextMeshProUGUI[20];

        for (int i = 0; i < MenuLength; i++)
        {
            pos = new Vector2(transform.position.x, transform.position.y + i * YGap);

            if(i == MenuLength / 2)
            {
                Slots[i] = Instantiate(SelectSlot, pos, Quaternion.identity, transform).GetComponent<Image>();

                for (int j = 0; j < 20; j++)
                {
                    CraftSlots[j] = Instantiate(CraftSlot, new Vector2(pos.x - Inventory.XSlotsGap + CraftSlotsOffset.x * j + CraftSlotsPos.x, pos.y + CraftSlotsOffset.y + CraftSlotsPos.y), Quaternion.identity, transform).GetComponent<Image>();
                    CraftIcons[j] = Instantiate(CraftIcon, CraftSlots[j].transform.position, Quaternion.identity, transform).GetComponent<Image>();
                    CraftItemNums[j] = Instantiate(CraftNum, new Vector2(pos.x - Inventory.XSlotsGap + CraftSlotsOffset.x * j + CraftNumsPos.x, pos.y + CraftSlotsOffset.y + CraftNumsPos.y), Quaternion.identity, transform).GetComponent<TextMeshProUGUI>();
                }
            }
            else
                Slots[i] = Instantiate(Inventory.Slot, pos, Quaternion.identity, transform).GetComponent<Image>();

            Icons[i] = Instantiate(Inventory.Icon, pos, Quaternion.identity, transform).GetComponent<Image>();
            ItemNums[i] = Instantiate(Inventory.Number, pos + new Vector2(Inventory.NumOffsetX, Inventory.NumOffsetY), Quaternion.identity, transform).GetComponent<TextMeshProUGUI>();
        }
        Refresh();
    }

    void Update()
    {
        Refresh();

        if(Input.GetKeyDown(KeyCode.Mouse0) && Inventory.CursorID == 0)
        {
            Vector3 mousePos = Input.mousePosition;
            for (int i = 0; i < MenuLength; ++i)
            {
                if (mousePos.x <= Icons[i].transform.position.x + Inventory.SlotSize &&
                    mousePos.x >= Icons[i].transform.position.x - Inventory.SlotSize &&
                    mousePos.y <= Icons[i].transform.position.y + Inventory.SlotSize &&
                    mousePos.y >= Icons[i].transform.position.y - Inventory.SlotSize)
                {
                    if (Slots[i].color.a > 0)
                    {
                        if (i == MenuLength / 2)
                        {
                            Inventory.CursorID = Crafts[Craftable[i + scroll]].ID;
                            Inventory.CursorNum = Crafts[Craftable[i + scroll]].Amount;
                            Inventory.Cancel = true;

                            for (int j = 0; j < Crafts[Craftable[scroll + MenuLength / 2]].Costs.Length; j++)
                            {
                                for (int k = 1; k < Inventory.Slots.Length; k++)
                                {
                                    if (Inventory.SlotIDs[k] == Crafts[Craftable[scroll + MenuLength / 2]].Costs[j].ID)
                                    {
                                        if (Inventory.SlotNumbers[k] >= Crafts[Craftable[scroll + MenuLength / 2]].Costs[j].Amount - amount)
                                        {
                                            Inventory.SlotNumbers[k] -= Crafts[Craftable[scroll + MenuLength / 2]].Costs[j].Amount - amount;
                                            amount = Crafts[Craftable[scroll + MenuLength / 2]].Costs[j].Amount;
                                        }
                                        else
                                        {
                                            amount += Inventory.SlotNumbers[k];
                                            Inventory.SlotNumbers[k] = 0;
                                        }

                                        if (amount == Crafts[Craftable[scroll + MenuLength / 2]].Costs[j].Amount)
                                        {
                                            amount = 0;
                                            k = 99999;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            Scroll(i - MenuLength / 2);
                        }
                    }
                    return;
                }
            }
        }
    }

    public void Refresh()
    {
        Craftable.Clear();
        for (int i = 0; i < StartCrafts.Count; i++)
        {
            Craftable.Add(StartCrafts[i]);
        }

        for (int i = 0; i < Craftable.Count; i++)
        {
            for(int j = 0; j < Crafts[Craftable[i]].Costs.Length; j++)
            {
                for (int k = 0; k < Inventory.SlotIDs.Length; k++)
                {
                    if (Inventory.SlotIDs[k] == Crafts[Craftable[i]].Costs[j].ID)
                    {
                        amount += Inventory.SlotNumbers[k];
                    }
                    if(amount >= Crafts[Craftable[i]].Costs[j].Amount)
                    {
                        k = 999999;
                    }
                }
                if(amount < Crafts[Craftable[i]].Costs[j].Amount)
                {
                    j = 999999;
                    //Scroll(-1);
                    Craftable.Remove(i + MenuLength / 2);
                }
                amount = 0;
            }
        }

        for (int i = 0; i < 20; i++)
        {
            if(i < Crafts[Craftable[scroll + MenuLength / 2]].Costs.Length)
            {
                CraftIcons[i].color = new Color(CraftIcons[i].color.r, CraftIcons[i].color.g, CraftIcons[i].color.b, 255);
                CraftSlots[i].color = new Color(CraftSlots[i].color.r, CraftSlots[i].color.g, CraftSlots[i].color.b, 255);

                CraftIcons[i].sprite = Inventory.Ref.Items[Crafts[Craftable[scroll + MenuLength / 2]].Costs[i].ID].icon;

                if(Crafts[Craftable[scroll + MenuLength / 2]].Costs[i].Amount > 1)
                {
                    CraftItemNums[i].text = Crafts[Craftable[scroll + MenuLength / 2]].Costs[i].Amount.ToString();
                }
                else
                {
                    CraftItemNums[i].text = "";
                }
            }
            else
            {
                CraftIcons[i].color = new Color(CraftIcons[i].color.r, CraftIcons[i].color.g, CraftIcons[i].color.b, 0);
                CraftSlots[i].color = new Color(CraftSlots[i].color.r, CraftSlots[i].color.g, CraftSlots[i].color.b, 0);
                CraftItemNums[i].text = "";
            }
        }

        for (int i = 0; i < MenuLength; i++)
        {
            if(i + scroll < Craftable.Count)
            {
                if (i + scroll < 0)
                {
                    Icons[i].color = new Color(0, 0, 0, 0);
                    Slots[i].color = new Color(Slots[i].color.r, Slots[i].color.g, Slots[i].color.b, 0);
                    ItemNums[i].text = "";
                }
                else
                {
                    Icons[i].color = new Color(255, 255, 255, 255);
                    Slots[i].color = new Color(Slots[i].color.r, Slots[i].color.g, Slots[i].color.b, 255);
                    Icons[i].sprite = Ref.Items[Crafts[Craftable[i + scroll]].ID].icon;
                    if (Crafts[Craftable[i + scroll]].Amount > 1)
                        ItemNums[i].text = Crafts[Craftable[i + scroll]].Amount.ToString();
                    else
                        ItemNums[i].text = "";
                }
            }
            else
            {
                Icons[i].color = new Color(0,0,0,0);
                Slots[i].color = new Color(Slots[i].color.r, Slots[i].color.g, Slots[i].color.b, 0);
                ItemNums[i].text = "";
            }
        }
    }

    public void Scroll(int ScrollAmount)
    {
        scroll += ScrollAmount;
    }
}

[System.Serializable]
public class Craft
{
    public int ID;
    public int Amount;
    public Cost[] Costs;
}

[System.Serializable]
public class Cost
{
    public int ID;
    public int Amount;
}