using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class settings : MonoBehaviour
{
    [Header("Items")]
    public Item[] Items;
}

[System.Serializable]
public class Item
{
    public string Name;
    public float Damage;
    public byte WeaponType;
    public byte Element;
}

//WT 0: Melee

//EL 0: Null