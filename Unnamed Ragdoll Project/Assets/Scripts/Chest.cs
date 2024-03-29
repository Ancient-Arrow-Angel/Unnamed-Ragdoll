using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Chest : MonoBehaviour
{
    public int XSlots;
    public int YSlots;

    public GameObject Icon;
    public GameObject Slot;
    public GameObject Number;

    public int[] SlotIDs;
    public int[] SlotNumbers;
    public Transform[] Slots;
    public TextMeshProUGUI[] ItemNums;

    public SlotMaker Invent;

    Vector2 Pos;

    // Start is called before the first frame update
    void Start()
    {
        SlotIDs = new int[XSlots * YSlots];
        SlotNumbers = new int[XSlots * YSlots];
        Slots = new Transform[XSlots * YSlots];
        ItemNums = new TextMeshProUGUI[XSlots * YSlots];

        for (int i = 0; i < XSlots * YSlots; i++)
        {
            SlotIDs[i] = new int();
        }

        for (int i = 0; i < XSlots * YSlots; i++)
        {
            Vector2 PrePos = Pos;
            Pos = new Vector2(-i % XSlots * Invent.XSlotsGap, -i / XSlots * Invent.YSlotsGap);

            Instantiate(Slot, Pos + (Vector2)transform.position, transform.rotation, transform);
            Slots[i] = Instantiate(Icon, Pos + (Vector2)transform.position, transform.rotation, transform).transform;
            ItemNums[i] = Instantiate(Number, Pos + new Vector2(Invent.NumOffsetX, Invent.NumOffsetY) + (Vector2)transform.position, transform.rotation, transform).GetComponent<TextMeshProUGUI>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateInventor();

        if (Input.GetMouseButtonDown(1))
        {
            Vector3 mousePos = Input.mousePosition;
            for (int i = 0; i < XSlots * YSlots; i++)
            {
                if (mousePos.x <= Slots[i].transform.position.x + Invent.SlotSize &&
                    mousePos.x >= Slots[i].transform.position.x - Invent.SlotSize &&
                    mousePos.y <= Slots[i].transform.position.y + Invent.SlotSize &&
                    mousePos.y >= Slots[i].transform.position.y - Invent.SlotSize)
                {
                    if (Invent.CursorID == 0)
                    {
                        SlotNumbers[i]--;
                        Invent.CursorNum++;
                        Invent.CursorID = SlotIDs[i];
                    }
                    else if (Invent.Ref.Items[SlotIDs[i]].Stackable && Invent.CursorID == SlotIDs[i] && SlotNumbers[i] < Invent.Ref.StackAmount)
                    {
                        SlotNumbers[i]++;
                        Invent.CursorNum--;
                    }
                    else if (SlotIDs[i] == 0 && Invent.CursorID > 0)
                    {
                        SlotNumbers[i]++;
                        Invent.CursorNum--;
                        SlotIDs[i] = Invent.CursorID;
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
        }

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            for (int i = 0; i < XSlots * YSlots; i++)
            {
                if (mousePos.x <= Slots[i].transform.position.x + Invent.SlotSize &&
                    mousePos.x >= Slots[i].transform.position.x - Invent.SlotSize &&
                    mousePos.y <= Slots[i].transform.position.y + Invent.SlotSize &&
                    mousePos.y >= Slots[i].transform.position.y - Invent.SlotSize)
                {
                    if (SlotIDs[i] == Invent.CursorID && Invent.Ref.Items[Invent.CursorID].Stackable && SlotNumbers[i] < Invent.Ref.StackAmount && Invent.CursorNum < Invent.Ref.StackAmount)
                    {
                        if (SlotNumbers[i] + Invent.CursorNum >= Invent.Ref.StackAmount)
                        {
                            Invent.CursorNum -= Invent.Ref.StackAmount - SlotNumbers[i];
                            SlotNumbers[i] = Invent.Ref.StackAmount;
                        }
                        else if (SlotNumbers[i] == Invent.Ref.StackAmount)
                        {

                        }
                        else
                        {
                            SlotIDs[i] = Invent.CursorID;
                            Invent.CursorID = 0;

                            SlotNumbers[i] += Invent.CursorNum;
                            Invent.CursorNum = 0;
                        }
                    }
                    else
                    {
                        Invent.InterID = Invent.CursorID;
                        Invent.CursorID = SlotIDs[i];
                        SlotIDs[i] = Invent.InterID;

                        Invent.InterID = SlotNumbers[i];
                        SlotNumbers[i] = Invent.CursorNum;
                        Invent.CursorNum = Invent.InterID;
                    }
                    return;
                }
                else if (SlotNumbers[i] < 0)
                {
                    SlotNumbers[i] = 0;
                }
            }
        }
    }

    public void UpdateInventor()
    {
        for (int i = 0; i < Slots.Length; i++)
        {
            Slots[i].GetComponent<Image>().sprite = Invent.Ref.Items[SlotIDs[i]].icon;

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

            if (SlotNumbers[i] <= 1)
            {
                ItemNums[i].text = "";
            }
            else
            {
                ItemNums[i].text = SlotNumbers[i].ToString();
            }
        }
    }
}