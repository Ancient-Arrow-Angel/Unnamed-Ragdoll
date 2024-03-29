using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorFlash : MonoBehaviour
{
    LimbHealth FlashRef;
    SpriteRenderer Self;

    // Start is called before the first frame update
    void Start()
    {
        FlashRef = transform.parent.GetComponent<LimbHealth>();
        Self = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Self.color = FlashRef.FlashAmount;
    }
}