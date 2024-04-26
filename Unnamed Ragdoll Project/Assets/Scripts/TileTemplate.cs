using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileTemplate : MonoBehaviour
{
    public int XOffset;
    public int YOffset;
    public int Width;
    public int Height;
    public Tilemap Tiles;
    public Tilemap BackTiles;
    public Tilemap Remove;
    public Tilemap RemoveBack;

    public int[] TileIDS;
    public int[] BackTileIDS;
    public bool[] FrontRemove;
    public bool[] BackRemove;

    TileMaker tileMaker;

    void Start()
    {
        tileMaker = GameObject.Find("Grid").GetComponent<TileMaker>();

        TileIDS = new int[Width*Height];
        BackTileIDS = new int[Width * Height];
        FrontRemove = new bool[Width * Height];
        BackRemove = new bool[Width * Height];

        for (int i = 0; i < Width * Height; i++)
        {
            int TileID = 0;

            for (int j = 1; j < tileMaker.TileTypes.Length; j++)
            {
                if (tileMaker.TileTypes[j].tile == Tiles.GetTile(new Vector3Int(i % Width + XOffset, i / Width + YOffset)))
                {
                    TileID = j;
                    j = 99999;
                }
            }

            TileIDS[i] = TileID;
            TileID = 0;

            for (int j = 1; j < tileMaker.TileTypes.Length; j++)
            {
                if (tileMaker.TileTypes[j].tile == BackTiles.GetTile(new Vector3Int(i % Width + XOffset, i / Width + YOffset)))
                {
                    TileID = j;
                    j = 99999;
                }
            }

            BackTileIDS[i] = TileID;

            if (Remove.HasTile(new Vector3Int(i % Width + XOffset, i / Width + YOffset)) == true)
            {
                FrontRemove[i] = true;
            }

            if (RemoveBack.HasTile(new Vector3Int(i % Width + XOffset, i / Width + YOffset)) == true)
            {
                BackRemove[i] = true;
            }
        }

        Destroy(GetComponent<Grid>());
        DestroyImmediate(GetComponentInChildren<Tilemap>().gameObject);
        DestroyImmediate(GetComponentInChildren<Tilemap>().gameObject);
        DestroyImmediate(GetComponentInChildren<Tilemap>().gameObject);
        DestroyImmediate(GetComponentInChildren<Tilemap>().gameObject);
        gameObject.SetActive(false);
    }
}