using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmorHandle : MonoBehaviour
{
    public Sprite[] Default;
    public Image[] Icons;

    public int[] SlotIDS;
    SlotMaker Inventory;
    Player PlayerS;

    item CursorItem;

    // Start is called before the first frame update
    void Start()
    {
        Inventory = GameObject.Find("Slot Maker").GetComponent<SlotMaker>();
        PlayerS = GameObject.Find("Player").GetComponent<Player>();

        SlotIDS = new int[Icons.Length];
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < Icons.Length; i++)
        {
            if (SlotIDS[i] > 0)
            {
                Icons[i].sprite = Inventory.Ref.Items[SlotIDS[i]].icon;
            }
            else
            {
                Icons[i].sprite = Default[i];
            }
        }

        if (Inventory.Ref.Items[Inventory.CursorID].Item != null)
            CursorItem = Inventory.Ref.Items[Inventory.CursorID].Item.transform.GetComponent<item>();

        if(CursorItem != null)
        if (Inventory.CursorID == 0 && Input.GetKeyDown(KeyCode.Mouse0) || CursorItem.ArmorType > 0 && Input.GetKeyDown(KeyCode.Mouse0))
        {
            Vector3 mousePos = Input.mousePosition;
            for (int i = 0; i < Icons.Length; i++)
            {
                if (mousePos.x <= Icons[i].transform.position.x + Inventory.SlotSize &&
                    mousePos.x >= Icons[i].transform.position.x - Inventory.SlotSize &&
                    mousePos.y <= Icons[i].transform.position.y + Inventory.SlotSize &&
                    mousePos.y >= Icons[i].transform.position.y - Inventory.SlotSize)
                {
                    if (Inventory.CursorID == 0)
                    {
                        if (i == 2)
                        {
                            for (int j = 0; j < PlayerS.Legs.Length; j++)
                            {
                                PlayerS.Legs[j].Defence -= CursorItem.Defence;
                                PlayerS.Legs[j].MaxHealth -= CursorItem.Health;
                                PlayerS.Legs[j].GetComponentsInChildren<SpriteRenderer>()[1].sprite = null;
                            }
                        }
                        else if (i == 1)
                        {
                            for (int j = 0; j < PlayerS.Chest.Length; j++)
                            {
                                PlayerS.Chest[j].Defence -= CursorItem.Defence;
                                PlayerS.Chest[j].MaxHealth -= CursorItem.Health;
                                PlayerS.Chest[j].GetComponentsInChildren<SpriteRenderer>()[1].sprite = null;
                            }
                        }
                        else if (i == 0)
                        {
                            PlayerS.Head.Defence -= CursorItem.Defence;
                            PlayerS.Head.MaxHealth -= CursorItem.Health;
                            PlayerS.Head.GetComponentsInChildren<SpriteRenderer>()[1].sprite = null;
                        }

                        Inventory.CursorID = SlotIDS[i];
                        SlotIDS[i] = 0;
                    }
                    else if(CursorItem.ArmorType == 1 && i == 2)
                    {
                        for (int j = 0; j < PlayerS.Legs.Length; j++)
                        {
                            PlayerS.Legs[j].Defence += CursorItem.Defence;
                            PlayerS.Legs[j].MaxHealth += CursorItem.Health;
                            PlayerS.Legs[j].GetComponentsInChildren<SpriteRenderer>()[1].sprite = CursorItem.ArmorSprites[j];
                        }

                        int InterID = SlotIDS[i];
                        SlotIDS[i] = Inventory.CursorID;
                        Inventory.CursorID = InterID;
                    }
                    else if(CursorItem.ArmorType == 2 && i == 1)
                    {
                        for (int j = 0; j < PlayerS.Chest.Length; j++)
                        {
                            PlayerS.Chest[j].Defence += CursorItem.Defence;
                            PlayerS.Chest[j].MaxHealth += CursorItem.Health;
                            PlayerS.Chest[j].GetComponentsInChildren<SpriteRenderer>()[1].sprite = CursorItem.ArmorSprites[j];
                        }

                        int InterID = SlotIDS[i];
                        SlotIDS[i] = Inventory.CursorID;
                        Inventory.CursorID = InterID;
                    }
                    else if (CursorItem.ArmorType == 3 && i == 0)
                    {
                        int InterID = SlotIDS[i];
                        SlotIDS[i] = Inventory.CursorID;
                        Inventory.CursorID = InterID;

                        PlayerS.Head.Defence += CursorItem.Defence;
                        PlayerS.Head.MaxHealth += CursorItem.Health;
                        PlayerS.Head.GetComponentsInChildren<SpriteRenderer>()[1].sprite = CursorItem.ArmorSprites[0];
                    }

                    if(Inventory.CursorID > 0)
                    {
                        Inventory.CursorNum = 1;
                    }
                    else
                    {
                        Inventory.CursorNum = 0;
                    }

                    Inventory.Cancel = true;
                    return;
                }
            }
        }
    }
}