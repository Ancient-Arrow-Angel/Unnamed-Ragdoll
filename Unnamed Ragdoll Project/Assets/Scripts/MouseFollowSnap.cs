using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFollowSnap : MonoBehaviour
{
    Vector2 MousePos;

    // Start is called before the first frame update
    void Start()
    {
        transform.SetParent(null);
    }

    // Update is called once per frame
    void Update()
    {
        MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector2(Mathf.Round(MousePos.x) + 0.5f, Mathf.Round(MousePos.y) + 0.5f);
    }
}