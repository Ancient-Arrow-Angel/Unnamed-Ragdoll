using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnParent : MonoBehaviour
{
    void Start()
    {
        transform.parent = null;
    }
}