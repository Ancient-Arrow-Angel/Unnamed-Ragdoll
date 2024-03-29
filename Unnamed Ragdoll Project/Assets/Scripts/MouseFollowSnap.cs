using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFollowSnap : MonoBehaviour
{
    Vector2 MousePos;
    Vector2 Offset;

    // Start is called before the first frame update
    void Start()
    {
        Offset = transform.localPosition;
        transform.SetParent(null);
    }

    // Update is called once per frame
    void Update()
    {
        MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector2(Mathf.Round(MousePos.x) + Offset.x, Mathf.Round(MousePos.y) + Offset.y);
    }
}