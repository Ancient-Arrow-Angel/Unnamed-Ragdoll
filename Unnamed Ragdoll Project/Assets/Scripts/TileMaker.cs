using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMaker : MonoBehaviour
{
    AudioSource HitClip;

    public TileType[] TileTypes;

    public TileStat[] FrontTileIDS;
    public TileStat[] BackTileIDS;
    public int[] LightAmount;

    public Tilemap FrontTiles;
    public Tilemap BackTiles;
    public Tilemap WaterTiles;

    public Player player;

    [Header("Light Settings")]


    [Header("World Settings")]
    public int WorldWidth;
    public int WorldHeight;
    public int SurfaceBiomes;
    
    public Biome[] Biomes;
    
    void Start()
    {
        HitClip = GetComponent<AudioSource>();

        int SurfaceHeight;

        FrontTileIDS = new TileStat[WorldWidth * WorldHeight];
        BackTileIDS = new TileStat[FrontTileIDS.Length];

        for (int i = 0; i < (WorldWidth * WorldHeight); i++)
        {
            FrontTileIDS[i] = new TileStat();
        }

        for (int i = 0; i < FrontTileIDS.Length; i++)
        {
            BackTileIDS[i] = new TileStat();
        }

        //for (int j = 0; j < Biomes.Length; j++)
        //{
        //    for (int i = 0; i < FrontTileIDS.Length; i++)
        //    {
        //        if (i - WorldWidth < 0)
        //        {
        //            FrontTileIDS[i].ID = Biomes[j].BedrockID;
        //        }
        //        else
        //        {
        //            if (i < Biomes[j].MaxBedrockHight * WorldWidth)
        //            {
        //                if (FrontTileIDS[i - WorldWidth].ID == 1 && Random.Range(0, 2) == 1)
        //                {
        //                    FrontTileIDS[i].ID = Biomes[j].BedrockID;
        //                }
        //                else
        //                {
        //                    if (Random.Range(0, 101) < Biomes[j].IronRareity)
        //                    {
        //                        FrontTileIDS[i].ID = Biomes[j].IronID;
        //                    }
        //                    else
        //                    {
        //                        FrontTileIDS[i].ID = Biomes[j].StoneID;
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                if (i < Biomes[j].NormalStoneHeight * WorldWidth)
        //                {
        //                    if (Random.Range(0, 101) < Biomes[j].IronRareity)
        //                    {
        //                        FrontTileIDS[i].ID = Biomes[j].IronID;
        //                    }
        //                    else
        //                    {
        //                        FrontTileIDS[i].ID = Biomes[j].StoneID;
        //                    }
        //                }
        //                else
        //                {
        //                    if (i < Biomes[j].MaxStoneHeight * WorldWidth)
        //                    {
        //                        if (FrontTileIDS[i - WorldWidth].ID == 3 && Random.Range(0, 2) == 1)
        //                        {
        //                            if (Random.Range(0, 101) < Biomes[j].IronRareity)
        //                            {
        //                                FrontTileIDS[i].ID = Biomes[j].IronID;
        //                            }
        //                            else
        //                            {
        //                                FrontTileIDS[i].ID = Biomes[j].StoneID;
        //                            }
        //                        }
        //                        else
        //                        {
        //                            FrontTileIDS[i].ID = Biomes[j].DirtID;
        //                        }
        //                    }
        //                    else if (i < Biomes[j].NormalDirtHeight * WorldWidth)
        //                    {
        //                        FrontTileIDS[i].ID = Biomes[j].DirtID;
        //                    }
        //                }
        //            }
        //        }
        //        BackTileIDS[i].ID = FrontTileIDS[i].ID;
        //    }

        //    SurfaceHeight = Biomes[j].NormalDirtHeight + 1;

        //    for (int i = 0; i < WorldWidth; i++)
        //    {
        //        SurfaceHeight += Random.Range(Biomes[j].MinSurfaceInc, Biomes[j].MaxSurfaceInc + 1);
        //        if(SurfaceHeight > Biomes[j].MaxSurfaceHeight)
        //        {
        //            SurfaceHeight = Biomes[j].MaxSurfaceHeight;
        //        }
        //        else if (SurfaceHeight <= Biomes[j].NormalDirtHeight)
        //        {
        //            SurfaceHeight = Biomes[j].NormalDirtHeight + 1;
        //        }

        //        for (int h = 0; h <= SurfaceHeight - Biomes[j].NormalDirtHeight; h++)
        //        {
        //            SetTile(i, h + Biomes[j].NormalDirtHeight, Biomes[j].DirtID, true);
        //            SetTile(i, h + Biomes[j].NormalDirtHeight, Biomes[j].DirtID, false);
        //        }
        //    }
        //}
        for (int i = 0; i < FrontTileIDS.Length; ++i)
        {
            FrontTileIDS[i].ID = Random.Range(0, TileTypes.Length);
        }
        for (int i = 0; i < FrontTileIDS.Length; ++i)
        {
            BackTileIDS[i].ID = Random.Range(0, TileTypes.Length);
        }
        Refresh();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Y))
            Refresh();
    }

    public void Refresh()
    {
        Vector3Int Pos = new Vector3Int(0, 0, 0);
        for (int i = 0; i < FrontTileIDS.Length; i++)
        {
            if (FrontTileIDS[i].ID > 0)
            {
                if (FrontTileIDS[i].HealthSet == false)
                {
                    FrontTileIDS[i].Health = TileTypes[FrontTileIDS[i].ID].MaxHealth;
                    FrontTileIDS[i].HealthSet = true;
                }
                else if (FrontTileIDS[i].Hit == true && FrontTileIDS[i].Health > 0)
                {
                    FrontTileIDS[i].Hit = false;
                    Instantiate(TileTypes[FrontTileIDS[i].ID].BreakParticles, new Vector2(i % WorldWidth, i / WorldWidth), transform.rotation);
                }
                else if (FrontTileIDS[i].Health <= 0)
                {
                    Instantiate(TileTypes[FrontTileIDS[i].ID].BreakParticles, new Vector2(i % WorldWidth, i / WorldWidth), transform.rotation);
                    Instantiate(TileTypes[FrontTileIDS[i].ID].BreakParticles, new Vector2(i % WorldWidth, i / WorldWidth), transform.rotation);
                    Instantiate(TileTypes[FrontTileIDS[i].ID].BreakParticles, new Vector2(i % WorldWidth, i / WorldWidth), transform.rotation);
                    FrontTileIDS[i].ID = 0;
                    FrontTileIDS[i].Hit = false;
                    FrontTileIDS[i].HealthSet = false;
                }
            }
            if (BackTileIDS[i].ID > 0)
            {
                if (BackTileIDS[i].HealthSet == false)
                {
                    BackTileIDS[i].Health = TileTypes[BackTileIDS[i].ID].MaxHealth;
                    BackTileIDS[i].HealthSet = true;
                }
                else if (BackTileIDS[i].Hit == true && BackTileIDS[i].Health > 0)
                {
                    BackTileIDS[i].Hit = false;
                    Instantiate(TileTypes[BackTileIDS[i].ID].BreakParticles, new Vector2(i % WorldWidth, i / WorldWidth), transform.rotation);
                }
                else if (BackTileIDS[i].Health <= 0)
                {
                    Instantiate(TileTypes[BackTileIDS[i].ID].BreakParticles, new Vector2(i % WorldWidth, i / WorldWidth), transform.rotation);
                    Instantiate(TileTypes[BackTileIDS[i].ID].BreakParticles, new Vector2(i % WorldWidth, i / WorldWidth), transform.rotation);
                    Instantiate(TileTypes[BackTileIDS[i].ID].BreakParticles, new Vector2(i % WorldWidth, i / WorldWidth), transform.rotation);
                    BackTileIDS[i].ID = 0;
                    BackTileIDS[i].Hit = false;
                    BackTileIDS[i].HealthSet = false;
                }
            }


            if (i % WorldWidth == 0)
            {
                Pos.x = 0;
                Pos.y++;
            }

            if (!TileTypes[FrontTileIDS[i].ID].IsLiquid)
            {
                WaterTiles.SetTile(Pos, null);
                FrontTiles.SetTile(Pos, TileTypes[FrontTileIDS[i].ID].tile);
            }
            else
            {
                WaterTiles.SetTile(Pos, TileTypes[FrontTileIDS[i].ID].tile);
                FrontTiles.SetTile(Pos, null);
            }
            BackTiles.SetTile(Pos, TileTypes[BackTileIDS[i].ID].tile);

            Pos.x++;
        }
    }

    public void RefreshTile(int x, int y)
    {
        Vector3Int Pos = new Vector3Int(x, y, 0);
            if (FrontTileIDS[x + y * WorldWidth].ID > 0)
            {
                if (FrontTileIDS[x + y * WorldWidth].HealthSet == false)
                {
                    FrontTileIDS[x + y * WorldWidth].Health = TileTypes[FrontTileIDS[x + y * WorldWidth].ID].MaxHealth;
                    FrontTileIDS[x + y * WorldWidth].HealthSet = true;
                }
                else if (FrontTileIDS[x + y * WorldWidth].Hit == true && FrontTileIDS[x + y * WorldWidth].Health > 0)
                {
                    FrontTileIDS[x + y * WorldWidth].Hit = false;
                    Instantiate(TileTypes[FrontTileIDS[x + y * WorldWidth].ID].BreakParticles, new Vector2(x, y), transform.rotation);
                }
                else if (FrontTileIDS[x + y * WorldWidth].Health <= 0)
                {
                    Instantiate(TileTypes[FrontTileIDS[x + y * WorldWidth].ID].BreakParticles, new Vector2(x, y), transform.rotation);
                    Instantiate(TileTypes[FrontTileIDS[x + y * WorldWidth].ID].BreakParticles, new Vector2(x, y), transform.rotation);
                    Instantiate(TileTypes[FrontTileIDS[x + y * WorldWidth].ID].BreakParticles, new Vector2(x, y), transform.rotation);

                    if(TileTypes[FrontTileIDS[x + y * WorldWidth].ID].Drop != null)
                        Instantiate(TileTypes[FrontTileIDS[x + y * WorldWidth].ID].Drop, new Vector2(x, y), transform.rotation);

                    FrontTileIDS[x + y * WorldWidth].ID = 0;
                    FrontTileIDS[x + y * WorldWidth].Hit = false;
                    FrontTileIDS[x + y * WorldWidth].HealthSet = false;
                }
            }
            if (BackTileIDS[x + y * WorldWidth].ID > 0)
            {
                if (BackTileIDS[x + y * WorldWidth].HealthSet == false)
                {
                    BackTileIDS[x + y * WorldWidth].Health = TileTypes[BackTileIDS[x + y * WorldWidth].ID].MaxHealth;
                    BackTileIDS[x + y * WorldWidth].HealthSet = true;
                }
                else if (BackTileIDS[x + y * WorldWidth].Hit == true && BackTileIDS[x + y * WorldWidth].Health > 0)
                {
                    BackTileIDS[x + y * WorldWidth].Hit = false;
                    Instantiate(TileTypes[BackTileIDS[x + y * WorldWidth].ID].BreakParticles, new Vector2(x, y), transform.rotation);
                }
                else if (BackTileIDS[x + y * WorldWidth].Health <= 0)
                {
                    Instantiate(TileTypes[BackTileIDS[x + y * WorldWidth].ID].BreakParticles, new Vector2(x, y), transform.rotation);
                    Instantiate(TileTypes[BackTileIDS[x + y * WorldWidth].ID].BreakParticles, new Vector2(x, y), transform.rotation);
                    Instantiate(TileTypes[BackTileIDS[x + y * WorldWidth].ID].BreakParticles, new Vector2(x, y), transform.rotation);

                    if (TileTypes[BackTileIDS[x + y * WorldWidth].ID].Drop != null)
                        Instantiate(TileTypes[BackTileIDS[x + y * WorldWidth].ID].Drop, new Vector2(x, y), transform.rotation);

                    BackTileIDS[x + y * WorldWidth].ID = 0;
                    BackTileIDS[x + y * WorldWidth].Hit = false;
                    BackTileIDS[x + y * WorldWidth].HealthSet = false;
                }
            }
        if (!TileTypes[FrontTileIDS[x + y * WorldWidth].ID].IsLiquid)
        {
            WaterTiles.SetTile(new Vector3Int(Pos.x, Pos.y + 1), null);
            FrontTiles.SetTile(new Vector3Int(Pos.x, Pos.y + 1), TileTypes[FrontTileIDS[x + y * WorldWidth].ID].tile);
        }
        else
        {
            WaterTiles.SetTile(new Vector3Int(Pos.x, Pos.y + 1), TileTypes[FrontTileIDS[x + y * WorldWidth].ID].tile);
            FrontTiles.SetTile(new Vector3Int(Pos.x, Pos.y + 1), null);
        }
        BackTiles.SetTile(new Vector3Int(Pos.x, Pos.y + 1), TileTypes[BackTileIDS[x + y * WorldWidth].ID].tile);
    }

    public void SetTile(int X, int Y, int ID, bool IsFront)
    {
        if (IsFront)
        {
            FrontTileIDS[X + Y * WorldWidth].ID = ID;
            Instantiate(TileTypes[FrontTileIDS[X + Y * WorldWidth].ID].BreakParticles, new Vector2(X, Y), transform.rotation);
            FrontTileIDS[X + Y * WorldWidth].HealthSet = false;
        }
        else
        {
            BackTileIDS[X + Y * WorldWidth].ID = ID;
            Instantiate(TileTypes[BackTileIDS[X + Y * WorldWidth].ID].BreakParticles, new Vector2(X, Y), transform.rotation);
            BackTileIDS[X + Y * WorldWidth].HealthSet = false;
        }
        RefreshTile(X, Y);
        HitClip.volume = 1;
        HitClip.Play();
    }

    public void DamageTile(int X, int Y, int Damage, bool IsFront, int BreakPower)
    {
        if (IsFront && FrontTileIDS[X + Y * WorldWidth].ID > 0)
        {
            if(BreakPower >= TileTypes[FrontTileIDS[X + Y * WorldWidth].ID].Resistince)
                FrontTileIDS[X + Y * WorldWidth].Health -= Damage;

            if (!TileTypes[FrontTileIDS[X + Y * WorldWidth].ID].IsLiquid)
            {
                FrontTileIDS[X + Y * WorldWidth].Hit = true;
                HitClip.volume = 1;
                HitClip.Play();
                RefreshTile(X, Y);
            }
        }
        else if (!IsFront && BackTileIDS[X + Y * WorldWidth].ID > 0)
        {
            if (BreakPower >= TileTypes[BackTileIDS[X + Y * WorldWidth].ID].Resistince)
                BackTileIDS[X + Y * WorldWidth].Health -= Damage;

            if (!TileTypes[BackTileIDS[X + Y * WorldWidth].ID].IsLiquid)
            {
                BackTileIDS[X + Y * WorldWidth].Hit = true;
                HitClip.volume = 1;
                HitClip.Play();
                RefreshTile(X, Y);
            }
        }
    }

    public int CheckTile(int X, int Y)
    {
        return FrontTileIDS[X + Y * WorldWidth].ID;
    }
    public int CheckBackTile(int X, int Y)
    {
        return BackTileIDS[X + Y * WorldWidth].ID;
    }
}

[System.Serializable]
public class TileType
{
    public Tile tile;
    public bool IsLiquid;
    public int MaxHealth;
    public int Resistince;
    public GameObject BreakParticles;
    public GameObject Drop;
    public bool DontDrop = false;
}

[System.Serializable]
public class TileStat
{
    public int ID;
    public int Health;
    public bool Hit;
    public bool HealthSet = false;
}

[System.Serializable]
public class Biome
{
    [Header("Biome Size")]
    public int MinBiomeWidth;
    public int MaxBiomeWidth;

    [Header("Stats")]
    public int MaxBedrockHight;

    public int NormalStoneHeight;
    public int MaxStoneHeight;

    public int IronRareity;


    public int NormalDirtHeight;

    public int MinSurfaceInc;
    public int MaxSurfaceInc;

    public int MaxSurfaceHeight;

    [Header("Tile Config")]
    public int BedrockID;
    public int DirtID;
    public int GrassID;
    public int StoneID;
    public int IronID;
    public int GoldID;
}