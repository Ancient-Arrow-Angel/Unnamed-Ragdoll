using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D.IK;
using UnityEngine.UI;

public class CraftingMenu : MonoBehaviour
{
    public Craft[] Crafts;
    public List<int> Craftable;
    public List<int> NormalCraftable = new List<int>();
    List<int> ActuallyCraftable = new List<int>();

    Image[] Slots;
    Image[] Icons;
    TextMeshProUGUI[] ItemNums;

    public Image Slot;
    public Image Icon;
    public TextMeshProUGUI ItemNum;

    public int Page;
    public int Selected = -1;
    public int XSlots;
    public int YSlots;

    public Image LeftButton;
    public Image RightButton;
    public Color NormalColor;
    public Color EndColor;

    public Color NormalSlotColor;
    public Color SelectedSlotColor;

    public Image[] CraftSlots;
    public Image[] CraftIcons;
    public TextMeshProUGUI[] CraftNums;

    Reference Ref;

    // Start is called before the first frame update
    void Start()
    {
        Ref = GameObject.Find("Global Ref").GetComponent<Reference>();
        
        Slots = new Image[XSlots * YSlots];
        Icons = new Image[XSlots * YSlots];
        ItemNums = new TextMeshProUGUI[XSlots * YSlots];

        for (int i = 0; i < YSlots; i++)
        {
            for (int j = 0; j < XSlots; j++)
            {
                Slots[j + i * XSlots] = Instantiate(Ref.Inventory.Slot, new Vector2(transform.position.x + j * Ref.Inventory.XSlotsGap, transform.position.y - i*Ref.Inventory.YSlotsGap), transform.rotation, transform).GetComponent<Image>();
                Icons[j + i * XSlots] = Instantiate(Ref.Inventory.Icon, new Vector2(transform.position.x + j * Ref.Inventory.XSlotsGap, transform.position.y - i * Ref.Inventory.YSlotsGap), transform.rotation, transform).GetComponent<Image>();
                ItemNums[j + i * XSlots] = Instantiate(Ref.Inventory.Number, new Vector2(transform.position.x + j * Ref.Inventory.XSlotsGap + Ref.Inventory.NumOffsetX, transform.position.y - i * Ref.Inventory.YSlotsGap + Ref.Inventory.NumOffsetY), transform.rotation, transform).GetComponent<TextMeshProUGUI>();
            }
        }

        for (int i = 0; i < CraftSlots.Length; i++)
        {
            CraftSlots[i].color = new Color(CraftSlots[i].color.r, CraftSlots[i].color.g, CraftSlots[i].color.b, 0);
            CraftIcons[i].color = new Color(255, 255, 255, 0);
            CraftNums[i].text = "";
        }

        for (int i = 0; i < Crafts.Length; i++)
        {
            if (Crafts[i].IsIDTile)
            {
                Crafts[i].CreatedID += Ref.NonTileItems -1;
            }

            for (int j = 0; j < Crafts[i].Costs.Length; j++)
            {
                if (Crafts[i].Costs[j].IsIDTile)
                {
                    Crafts[i].Costs[j].CostID += Ref.NonTileItems -1;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Input.mousePosition;

        ActuallyCraftable = new List<int>();
        foreach (var item in NormalCraftable)
        {
            ActuallyCraftable.Add(item);
        }
        foreach (var item in Craftable)
        {
            ActuallyCraftable.Add(item);
        }

        Craftable.Clear();

        List<int> ToRemove = new List<int>();
        for (int i = 0; i < ActuallyCraftable.Count; i++)
        {
            for (int j = 0; j < Crafts[ActuallyCraftable[i]].Costs.Length; j++)
            {
                int Count = 0;
                for (int k = 0; k < Ref.Inventory.SlotIDs.Length; k++)
                {
                    if (Ref.Inventory.SlotIDs[k] == Crafts[ActuallyCraftable[i]].Costs[j].CostID)
                    {
                        Count += Ref.Inventory.SlotNumbers[k];
                    }
                }

                if(Count < Crafts[ActuallyCraftable[i]].Costs[j].CostAmount)
                {
                    ToRemove.Add(i);
                    j = 99999;
                }
            }
        }
        for(int i = 0; i < ToRemove.Count; i++)
        {
            ActuallyCraftable.Remove(ToRemove[i]);
        }

        if (ActuallyCraftable.Count < 0)
        {
            Selected = -1;
        }

        if(Selected > -1)
        {
            for (int i = 0; i < CraftSlots.Length; i++)
            {
                CraftSlots[i].color = new Color(CraftSlots[i].color.r, CraftSlots[i].color.g, CraftSlots[i].color.b, 0);
                CraftIcons[i].color = new Color(255, 255, 255, 0);
                CraftNums[i].text = "";
            }
            if (Selected >= ActuallyCraftable.Count)
            {
                Selected = ActuallyCraftable.Count - 1;
            }
        }

        if (Page >= ActuallyCraftable.Count / Slots.Length)
        {
            RightButton.color = EndColor;
        }
        else
        {
            RightButton.color = NormalColor;
        }

        if (Page <= 0)
        {
            LeftButton.color = EndColor;
        }
        else
        {
            LeftButton.color = NormalColor;
        }

        for (int i = 0; i < Slots.Length; i++)
        {
            if(i + Slots.Length*Page > ActuallyCraftable.Count -1)
            {
                Slots[i].color = new Color(Slots[i].color.r, Slots[i].color.g, Slots[i].color.b, 0);
                Icons[i].color = new Color(Icons[i].color.r, Icons[i].color.g, Icons[i].color.b, 0);
                ItemNums[i].text = "";
            }
            else
            {
                Slots[i].color = new Color(Slots[i].color.r, Slots[i].color.g, Slots[i].color.b, 255);
                Icons[i].color = new Color(Icons[i].color.r, Icons[i].color.g, Icons[i].color.b, 255);

                Icons[i].sprite = Ref.Items[Crafts[ActuallyCraftable[i + Slots.Length * Page]].CreatedID].icon;

                if(Crafts[ActuallyCraftable[i + Slots.Length * Page]].CreatedAmount > 1)
                    ItemNums[i].text = Crafts[ActuallyCraftable[i + Slots.Length * Page]].CreatedAmount.ToString();
                else
                    ItemNums[i].text = "";
            }

            if (Selected != i + Slots.Length * Page)
            {
                Slots[i].color = new Color(NormalSlotColor.r, NormalSlotColor.g, NormalSlotColor.b, Slots[i].color.a);
            }
            else
            {
                Slots[i].color = new Color(SelectedSlotColor.r, SelectedSlotColor.g, SelectedSlotColor.b, Slots[i].color.a);
            }

            if (Selected == -1)
            {
                Slots[i].color = new Color(NormalSlotColor.r, NormalSlotColor.g, NormalSlotColor.b, Slots[i].color.a);
            }
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            for (int i = 0; i < Slots.Length; i++)
            {
                if (ActuallyCraftable.Count >= i + Slots.Length * Page)
                {
                    if (mousePos.x <= Slots[i].transform.position.x + Ref.Inventory.SlotSize &&
                        mousePos.x >= Slots[i].transform.position.x - Ref.Inventory.SlotSize &&
                        mousePos.y <= Slots[i].transform.position.y + Ref.Inventory.SlotSize &&
                        mousePos.y >= Slots[i].transform.position.y - Ref.Inventory.SlotSize)
                    {
                        if (Slots[i].color.a > 0)
                            Selected = i + Slots.Length * Page;

                        i = 9999;
                    }
                }
            }
        }

        if (Selected > -1)
        {
            Icon.color = new Color(Icon.color.r, Icon.color.g, Icon.color.b, 255);
            Icon.sprite = Ref.Items[Crafts[ActuallyCraftable[Selected]].CreatedID].icon;
            if (Crafts[ActuallyCraftable[Selected]].CreatedAmount > 1)
                ItemNum.text = Crafts[ActuallyCraftable[Selected]].CreatedAmount.ToString();
            else
                ItemNum.text = "";

            for(int i = 0; i < CraftSlots.Length; i++)
            {
                if (i >= Crafts[ActuallyCraftable[Selected]].Costs.Length)
                {
                    CraftSlots[i].color = new Color(CraftSlots[i].color.r, CraftSlots[i].color.g, CraftSlots[i].color.b, 0);
                    CraftIcons[i].color = new Color(255, 255, 255, 0);
                    CraftNums[i].text = "";
                }
                else
                {
                    CraftIcons[i].color = new Color(255, 255, 255, 255);
                    CraftSlots[i].color = new Color(CraftSlots[i].color.r, CraftSlots[i].color.g, CraftSlots[i].color.b, 255);

                    CraftNums[i].text = Crafts[ActuallyCraftable[Selected]].Costs[i].CostAmount.ToString();
                    CraftIcons[i].sprite = Ref.Items[Crafts[ActuallyCraftable[Selected]].Costs[i].CostID].icon;
                }
            }
        }
        else
        {
            Icon.color = new Color(Icon.color.r, Icon.color.g, Icon.color.b, 0);
            ItemNum.text = "";
        }
    }

    public void ScrollLeft()
    {
        if(Page <= 0)
        {

        }
        else
        {
            Page--;
        }
    }

    public void ScrollRight()
    {
        if (Page >= ActuallyCraftable.Count / Slots.Length)
        {

        }
        else
        {
            Page++;
        }
    }

    public void Craft()
    {
        if(Selected > -1)
        {
            if (Crafts[ActuallyCraftable[Selected]].CreatedAmount + Ref.Inventory.CursorNum <= Ref.StackAmount && Ref.Items[Crafts[ActuallyCraftable[Selected]].CreatedID].Stackable)
            {
                if (Ref.Inventory.CursorID == 0)
                {
                    Ref.Inventory.CursorNum = Crafts[ActuallyCraftable[Selected]].CreatedAmount;
                    Ref.Inventory.CursorID = Crafts[ActuallyCraftable[Selected]].CreatedID;
                }
                else if (Ref.Inventory.CursorID == Crafts[ActuallyCraftable[Selected]].CreatedID)
                {
                    Ref.Inventory.CursorNum += Crafts[ActuallyCraftable[Selected]].CreatedAmount;
                    Ref.Inventory.CursorID = Crafts[ActuallyCraftable[Selected]].CreatedID;
                }

                for (int i = 0; i < Crafts[ActuallyCraftable[Selected]].Costs.Length; i++)
                {
                    Ref.RemoveItems(Crafts[ActuallyCraftable[Selected]].Costs[i].CostID, Crafts[ActuallyCraftable[Selected]].Costs[i].CostAmount);
                }

                Ref.Inventory.Cancel = true;
            }
            else
            {
                if (Ref.Inventory.CursorID == 0)
                {
                    Ref.Inventory.CursorNum = Crafts[ActuallyCraftable[Selected]].CreatedAmount;
                    Ref.Inventory.CursorID = Crafts[ActuallyCraftable[Selected]].CreatedID;

                    for (int i = 0; i < Crafts[ActuallyCraftable[Selected]].Costs.Length; i++)
                    {
                        Ref.RemoveItems(Crafts[ActuallyCraftable[Selected]].Costs[i].CostID, Crafts[ActuallyCraftable[Selected]].Costs[i].CostAmount);
                    }
                }
            }
        }
    }
}

[System.Serializable]
public class Craft
{
    public int CreatedID;
    public int CreatedAmount;
    public bool IsIDTile;
    public Cost[] Costs;
}

[System.Serializable]
public class Cost
{
    public int CostID;
    public int CostAmount;
    public bool IsIDTile;
}