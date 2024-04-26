using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseSprite : MonoBehaviour
{
    public SpriteRenderer Renderer;
    SpriteRenderer ThisRenderer;

    void Start()
    {
        ThisRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        ThisRenderer.sprite = Renderer.sprite;
    }
}