using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

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
    public GameObject SpawnPart;
    public GameObject FailSpawnPart;

    public RectTransform[] NonDrops;

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

        CursorSprite.color = new Color(255, 255, 255, 0);
        CursorItemNum.text = "";
        UpdateInventor();
        Menu.SetActive(false);
    }

    private void LateUpdate()
    {
        Vector3 mousePos = Input.mousePosition;

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

        if (Input.GetMouseButtonDown(1) && Cancel == false)
        {
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
                    else if (Ref.Items[SlotIDs[i]].Stackable && CursorID == SlotIDs[i] && SlotNumbers[i] < Ref.StackAmount)
                    {
                        SlotNumbers[i]++;
                        CursorNum--;
                    }
                    else if (SlotIDs[i] == 0 && CursorID > 0)
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
                bool CanDrop = false;
                for (int i = 0; i < NonDrops.Length; i++)
                {
                    if (mousePos.x <= NonDrops[i].transform.position.x + NonDrops[i].sizeDelta.x / 2 &&
                        mousePos.x >= NonDrops[i].transform.position.x - NonDrops[i].sizeDelta.x / 2 &&
                        mousePos.y <= NonDrops[i].transform.position.y + NonDrops[i].sizeDelta.y / 2 &&
                        mousePos.y >= NonDrops[i].transform.position.y - NonDrops[i].sizeDelta.y / 2 &&
                        NonDrops[i].gameObject.activeInHierarchy)
                    {
                        CanDrop = false;
                        i = 999;
                    }
                    else
                    {
                        CanDrop = true;
                    }
                }

                if (CanDrop)
                {
                    if (!Physics2D.OverlapCircle(mousePos = Camera.main.ScreenToWorldPoint(mousePos), SpawnRadius, EverythingBUT))
                    {
                        item Created = Instantiate(Ref.Items[CursorID].Item, mousePos, transform.rotation).GetComponent<item>();
                        Created.ItemNum = 1;
                        CursorNum -= 1;
                        Instantiate(SpawnPart, mousePos, transform.rotation);
                    }
                    else
                    {
                        Instantiate(FailSpawnPart, mousePos, transform.rotation);
                    }
                }
            }
        }

        if (Input.GetMouseButtonDown(0) && Cancel == false)
        {
            for (int i = 1; i < XSlots * YSlots + 1; i++)
            {
                if (mousePos.x <= Slots[i].transform.position.x + SlotSize &&
                    mousePos.x >= Slots[i].transform.position.x - SlotSize &&
                    mousePos.y <= Slots[i].transform.position.y + SlotSize &&
                    mousePos.y >= Slots[i].transform.position.y - SlotSize)
                {
                    if (SlotIDs[i] == CursorID && Ref.Items[CursorID].Stackable && SlotNumbers[i] < Ref.StackAmount && CursorNum < Ref.StackAmount)
                    {
                        if (SlotNumbers[i] + CursorNum >= Ref.StackAmount)
                        {
                            CursorNum -= Ref.StackAmount - SlotNumbers[i];
                            SlotNumbers[i] = Ref.StackAmount;
                        }
                        else if (SlotNumbers[i] == Ref.StackAmount)
                        {

                        }
                        else
                        {
                            SlotIDs[i] = CursorID;
                            CursorID = 0;

                            SlotNumbers[i] += CursorNum;
                            CursorNum = 0;
                        }
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
                bool CanDrop = false;
                for (int i = 0; i < NonDrops.Length; i++)
                {
                    if (mousePos.x <= NonDrops[i].transform.position.x + NonDrops[i].sizeDelta.x / 2 &&
                        mousePos.x >= NonDrops[i].transform.position.x - NonDrops[i].sizeDelta.x / 2 &&
                        mousePos.y <= NonDrops[i].transform.position.y + NonDrops[i].sizeDelta.y / 2 &&
                        mousePos.y >= NonDrops[i].transform.position.y - NonDrops[i].sizeDelta.y / 2 &&
                        NonDrops[i].gameObject.activeInHierarchy)
                    {
                        CanDrop = false;
                        i = 999;
                    }
                    else
                    {
                        CanDrop = true;
                    }
                }

                if(CanDrop)
                {
                    if (!Physics2D.OverlapCircle(mousePos = Camera.main.ScreenToWorldPoint(mousePos), SpawnRadius, EverythingBUT))
                    {
                        item Created = Instantiate(Ref.Items[CursorID].Item, mousePos, transform.rotation).GetComponent<item>();
                        Created.ItemNum = CursorNum;
                        CursorID = 0;
                        Instantiate(SpawnPart, mousePos, transform.rotation);
                    }
                    else
                    {
                        Instantiate(FailSpawnPart, mousePos, transform.rotation);
                    }
                }
            }
        }
        Cancel = false;

        for (int i = 1; i < XSlots * YSlots + 1; i++)
        {
            if (mousePos.x <= Slots[i].transform.position.x + SlotSize &&
                mousePos.x >= Slots[i].transform.position.x - SlotSize &&
                mousePos.y <= Slots[i].transform.position.y + SlotSize &&
                mousePos.y >= Slots[i].transform.position.y - SlotSize)
            {
                if(Ref.Items[SlotIDs[i]].Item != null && CursorID == 0)
                    ItemText.text = Ref.Items[SlotIDs[i]].Item.name + Ref.Items[SlotIDs[i]].Description;
                else
                    ItemText.text = "";
                i = 999999;
            }
            else
            {
                ItemText.text = "";
            }
        }
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