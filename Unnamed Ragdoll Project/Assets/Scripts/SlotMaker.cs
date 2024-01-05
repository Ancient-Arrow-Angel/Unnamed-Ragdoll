using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using TMPro;

public class SlotMaker : MonoBehaviour
{
    public GameObject Icon;
    public GameObject Slot;
    public GameObject Number;

    public Transform SlotKeep;
    public Transform NumKeep;

    public Image CursorSprite;
    public int CursorID;
    public int CursorNum;
    public TextMeshProUGUI CursorItemNum;

    public int InterID;

    public int[] SlotIDs;
    public int[] SlotNumbers;
    public Transform[] Slots;
    public TextMeshProUGUI[] ItemNums;

    public int XSlots;
    public int YSlots;

    public int XSlotsGap;
    public int YSlotsGap;

    public float SlotSize;

    public float NumOffsetX;
    public float NumOffsetY;

    public TextMeshProUGUI ItemText;
    public GameObject SpawnPoint;
    public LayerMask EverythingBUT;
    public float SpawnRadius;
    public GameObject Menu;

    public Reference Ref;
    public bool Cancel;

    Vector2 Pos;

    void Start()
    {
        SlotIDs = new int[XSlots * YSlots + 1];
        SlotNumbers = new int[XSlots * YSlots + 1];
        Slots = new Transform[XSlots * YSlots + 2];
        ItemNums = new TextMeshProUGUI[XSlots * YSlots + 2];

        for (int i = 0; i < XSlots * YSlots; i++)
        {
            SlotIDs[i] = new int();
        }

        for (int i = 0; i < XSlots * YSlots; i++)
        {
            Vector2 PrePos = Pos;
            Pos = new Vector2(-i % XSlots * XSlotsGap, -i / XSlots * YSlotsGap);

            Instantiate(Icon, Pos + (Vector2)transform.position, transform.rotation, transform);
            Instantiate(Slot, Pos + (Vector2)transform.position, transform.rotation, SlotKeep);
            Instantiate(Number, PrePos + new Vector2(NumOffsetX, NumOffsetY) + (Vector2)transform.position, transform.rotation, NumKeep);
        }
        Instantiate(Number, Pos + new Vector2(NumOffsetX, NumOffsetY) + (Vector2)transform.position, transform.rotation, NumKeep);

        Slots = GetComponentsInChildren<Transform>();
        ItemNums = NumKeep.GetComponentsInChildren<TextMeshProUGUI>();


        Ref.AddItem(3, 1);
        Ref.AddItem(1, 1);
        Ref.AddItem(2, 500);
        Ref.AddItem(4, 500);

        CursorSprite.color = new Color(255, 255, 255, 0);
        CursorItemNum.text = "";
        UpdateInventor();
        Menu.SetActive(false);
    }

    private void Update()
    {
        if(CursorID != 0)
            ItemText.text = Ref.Items[CursorID].Item.name;
        else
            ItemText.text = "";

        CursorSprite.sprite = Ref.Items[CursorID].icon;
        if (CursorID == 0)
        {
            CursorSprite.color = new Color(255, 255, 255, 0);
            CursorNum = 0;
        }
        else
        {
            CursorSprite.color = new Color(255, 255, 255, 255);
        }
        if (CursorNum <= 1)
        {
            CursorItemNum.text = "";
            if(CursorNum <= 0)
            {
                CursorID = 0;
            }
        }
        else
        {
            CursorItemNum.text = CursorNum.ToString();
        }

        UpdateInventor();

        if (Input.GetMouseButtonDown(1) && !Cancel)
        {
            Vector3 mousePos = Input.mousePosition;
            for (int i = 1; i < XSlots * YSlots + 1; i++)
            {
                if (mousePos.x <= Slots[i].transform.position.x + SlotSize &&
                    mousePos.x >= Slots[i].transform.position.x - SlotSize &&
                    mousePos.y <= Slots[i].transform.position.y + SlotSize &&
                    mousePos.y >= Slots[i].transform.position.y - SlotSize)
                {
                    if(CursorID == 0)
                    {
                        SlotNumbers[i]--;
                        CursorNum++;
                        CursorID = SlotIDs[i];
                    }
                    else if (Ref.Items[SlotIDs[i]].Stackable && CursorID == SlotIDs[i])
                    {
                        SlotNumbers[i]++;
                        CursorNum--;
                        SlotIDs[i] = CursorID;
                    }
                    else if (SlotIDs[i] == 0)
                    {
                        SlotNumbers[i]++;
                        CursorNum--;
                        SlotIDs[i] = CursorID;
                    }

                    if (SlotNumbers[i] == 0)
                    {
                        SlotIDs[i] = 0;
                    }
                    return;
                }
                else if (SlotNumbers[i] <= 0)
                {
                    SlotNumbers[i] = 0;
                }
            }
            if (CursorID > 0)
            {
                if (!Physics2D.OverlapCircle(mousePos = Camera.main.ScreenToWorldPoint(mousePos), SpawnRadius, EverythingBUT))
                {
                    item Created = Instantiate(Ref.Items[CursorID].Item, mousePos, transform.rotation).GetComponent<item>();
                    Created.Num = 1;
                    CursorNum -= 1;
                }
            }
        }

        if (Input.GetMouseButtonDown(0) && !Cancel)
        {
            Vector3 mousePos = Input.mousePosition;
            for (int i = 1; i < XSlots * YSlots + 1; i++)
            {
                if (mousePos.x <= Slots[i].transform.position.x + SlotSize &&
                    mousePos.x >= Slots[i].transform.position.x - SlotSize &&
                    mousePos.y <= Slots[i].transform.position.y + SlotSize &&
                    mousePos.y >= Slots[i].transform.position.y - SlotSize)
                {
                    if (SlotIDs[i] == CursorID && Ref.Items[CursorID].Stackable)
                    {
                        SlotIDs[i] = CursorID;
                        CursorID = 0;

                        SlotNumbers[i] += CursorNum;
                        CursorNum = 0;
                    }
                    else
                    {
                        InterID = CursorID;
                        CursorID = SlotIDs[i];
                        SlotIDs[i] = InterID;

                        InterID = SlotNumbers[i];
                        SlotNumbers[i] = CursorNum;
                        CursorNum = InterID;
                    }
                    return;
                }
                else if (SlotNumbers[i] < 0)
                {
                    SlotNumbers[i] = 0;
                }
            }
            if(CursorID > 0)
            {
                if (!Physics2D.OverlapCircle(mousePos = Camera.main.ScreenToWorldPoint(mousePos), SpawnRadius, EverythingBUT))
                {
                    item Created = Instantiate(Ref.Items[CursorID].Item, mousePos, transform.rotation).GetComponent<item>();
                    Created.Num = CursorNum;
                    CursorID = 0;
                }
            }
        }
        Cancel = false;
    }

    public void UpdateInventor()
    {
        for (int i = 1; i < Slots.Length; i++)
        {
            Slots[i].GetComponent<Image>().sprite = Ref.Items[SlotIDs[i]].icon;

            if (SlotNumbers[i] <= 0)
            {
                SlotIDs[i] = 0;
            }

            if (SlotIDs[i] == 0)
            {
                Slots[i].GetComponent<Image>().color = new Color(255, 255, 255, 0);
            }
            else
            {
                Slots[i].GetComponent<Image>().color = new Color(255, 255, 255, 255);
            }

            if (SlotNumbers[i - 1] <= 1)
            {
                ItemNums[i - 1].text = "";
            }
            else
            {
                ItemNums[i - 1].text = SlotNumbers[i - 1].ToString();
            }
        }
        if (SlotNumbers[Slots.Length - 1] <= 1)
        {
            ItemNums[Slots.Length - 1].text = "";
        }
        else
        {
            ItemNums[Slots.Length - 1].text = SlotNumbers[Slots.Length - 1].ToString();
        }
    }
}