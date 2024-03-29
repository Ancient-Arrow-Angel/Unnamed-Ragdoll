using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foliage : MonoBehaviour
{
    Player player;
    Reference Ref;
    TileMaker Tile;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        Ref = GameObject.Find("Global Ref").GetComponent<Reference>();
        Tile = GameObject.Find("Grid").GetComponent<TileMaker>();
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.x > player.rb.transform.position.x + Ref.CameraSize ||
           transform.position.x < player.rb.transform.position.x - Ref.CameraSize ||
           transform.position.y > player.rb.transform.position.y + Ref.CameraSize ||
           transform.position.y < player.rb.transform.position.y - Ref.CameraSize)
        {
            Destroy(gameObject);
        }

        if (Tile.CheckTile((int)Mathf.Round(transform.position.x + 0.5f), (int)Mathf.Round(transform.position.y + 1.5f) -1) == 0)
        {
            Destroy(gameObject);
        }
        if (Tile.CheckTile((int)Mathf.Round(transform.position.x + 0.5f), (int)Mathf.Round(transform.position.y + 1.5f)) > 0)
        {
            Destroy(gameObject);
        }
    }
}