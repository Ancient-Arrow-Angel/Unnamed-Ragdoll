using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMaker : MonoBehaviour
{
    AudioSource HitClip;

    public TileType[] TileTypes;

    public TileStat[] FrontTileIDS;
    public TileStat[] BackTileIDS;
    //public int[] LightAmount;

    public Tilemap FrontTiles;
    public Tilemap BackTiles;
    public Tilemap WaterTiles;

    public Player player;

    [Header("Initial")]
    public int WorldWidth;
    public int WorldHeight;
    public int MinStoneHeight;
    public int MaxStoneHeight;
    public int DirtHeight;
    public int MaxDirtHeight;
    [Range(0, 101)]
    public float CopperDirtChance;

    [Header("Fairy Woods")]
    public int MinStart_FW;
    public int MaxStart_FW;

    public int MinEnd_FW;
    public int MaxEnd_FW;

    public TileTemplate StartingArea_FW;

    [Range(0, 101)]
    public float BackTreeChance_FW;
    [Range(0, 101)]
    public float FrontTreeChance_FW;

    public TileTemplate[] FairyWoodsTrees_FW;
    public TileTemplate[] FairyWoodsBackTrees_FW;

    [Header("Lost Desert")]
    public int MinEnd_LD;
    public int MaxEnd_LD;

    [Range(0, 101)]
    public float RubyChance_LD;

    [Range(0, 101)]
    public float BackCactusChance_LD;
    [Range(0, 101)]
    public float FrontCactusChance_LD;
    public int MinCactusHeight_LD;
    public int MaxCactusHeight_LD;

    public int MinPiramidWidth_LD;
    public int MaxPiramidWidth_LD;

    public int MinPiramidStartHeight_LD;
    public int MaxPiramidStartHeight_LD;

    public TileTemplate PiramidEntrance_Right_LD;
    public TileTemplate PiramidEntrance_Left_LD;

    public TileTemplate[] Level1PiramidRooms_LD;

    [Header("Waterfall Mountain")]
    public int MinEnd_WM;
    public int MaxEnd_WM;

    public int MinIncrease_WM;
    public int MaxIncrease_WM;

    public int SnowHeight_WM;

    [Range(0, 101)]
    public float SapphireChance_WM;

    [Header("Graveyard")]
    public int MinEnd_G;
    public int MaxEnd_G;

    [Header("Furniture")]
    public item[] FurnitureTypes;
    public int[] FurnitureIDS;

    Reference Ref;
    Vector2 LastPos;

    void Start()
    {
        Ref = GameObject.Find("Global Ref").GetComponent<Reference>();
        HitClip = GetComponent<AudioSource>();

        FrontTileIDS = new TileStat[WorldWidth * WorldHeight];
        BackTileIDS = new TileStat[FrontTileIDS.Length];

        LastPos.x = Mathf.Round(player.rb.transform.position.x);
        LastPos.y = Mathf.Round(player.rb.transform.position.y);

        for (int i = 0; i < (WorldWidth * WorldHeight); i++)
        {
            FrontTileIDS[i] = new TileStat();
        }

        for (int i = 0; i < FrontTileIDS.Length; i++)
        {
            BackTileIDS[i] = new TileStat();
        }

        FurnitureIDS = new int[FrontTileIDS.Length];

        for (int i = 0; i < FurnitureIDS.Length; i++)
        {
            FurnitureIDS[i] = new int();
        }

        GenerateWorld();

        LastPos = player.rb.transform.position;
        RefreshRange();
    }

    public void GenerateWorld()
    {
        int CurrentHeight = Random.Range(DirtHeight, MaxDirtHeight);

        for(int i = 0; i < WorldWidth; i++)
        {
            int StoneHeight = Random.Range(MinStoneHeight, MaxStoneHeight);

            for (int j = 0; j < StoneHeight; j++)
            {
                SetTileA(i, j, 16, true);
            }
            for(int j = 0; j < MaxStoneHeight-StoneHeight; j++)
            {
                SetTileA(i, j + StoneHeight, 3, true);
            }

            for (int j = 0; j < DirtHeight; j++)
            {
                if(Random.Range(0f, 101f) <= CopperDirtChance)
                {
                    SetTileA(i, j + MaxStoneHeight, 17, true);
                }
                else
                {
                    SetTileA(i, j + MaxStoneHeight, 3, true);
                }
            }

            CurrentHeight += Random.Range(-1, 2);
            if(CurrentHeight < DirtHeight)
            {
                CurrentHeight = DirtHeight;
            }
            if (CurrentHeight > MaxDirtHeight)
            {
                CurrentHeight = MaxDirtHeight;
            }
            for (int j = 0; j < CurrentHeight; j++)
            {
                SetTileA(i, j + DirtHeight+MaxStoneHeight, 3, true);
            }
            SetTileA(i, CurrentHeight + DirtHeight + MaxStoneHeight, 2, true);
        }

        int Start = Random.Range(MinStart_FW, MaxStart_FW);
        int End = Random.Range(MinEnd_FW, MaxEnd_FW);
        for (int i = Start; i < End; i++)
        {
            for (int j = 0; j < WorldHeight; j++)
            {
                if (CheckTile(i, j) == 3)
                {
                    SetTileA(i, j, 32, true);
                }
                else if (CheckTile(i, j) == 2)
                {
                    SetTileA(i, j, 31, true);
                }
                else if (CheckTile(i, j) == 17)
                {
                    SetTileA(i, j, 33, true);
                }
            }

            if (i - 20 > WorldWidth / 2 || i + 20 < WorldWidth / 2)
            {
                for (int j = 0; j < WorldHeight; j++)
                {
                    if (CheckTile(i, j) == 31)
                    {
                        if (Random.Range(0f, 101f) <= BackTreeChance_FW)
                        {
                            UseTemplate(FairyWoodsBackTrees_FW[Random.Range(0, FairyWoodsBackTrees_FW.Length)], i, j + 1);
                        }
                        if (Random.Range(0f, 101f) <= FrontTreeChance_FW)
                        {
                            UseTemplate(FairyWoodsTrees_FW[Random.Range(0, FairyWoodsTrees_FW.Length)], i, j + 1);
                        }
                    }
                }
            }
        }
        for (int j = 0; j < WorldHeight; j++)
        {
            if (CheckTile(WorldWidth / 2, j) == 31)
            {
                UseTemplate(StartingArea_FW, WorldWidth / 2, j +10);
            }
        }

        int OtherStart;
        int OtherEnd;

        if(Random.Range(0,2) == 0)
        {
            OtherStart = Start - Random.Range(MinEnd_WM, MaxEnd_WM);
            OtherEnd = Start;
            Start = End;
            End += Random.Range(MinEnd_LD, MaxEnd_LD);
        }
        else
        {
            OtherStart = End;
            OtherEnd = End + Random.Range(MinEnd_WM, MaxEnd_WM);
            End = Start;
            Start -= Random.Range(MinEnd_LD, MaxEnd_LD);
        }

        int PiramidWidth = Random.Range(MinPiramidWidth_LD, MaxPiramidWidth_LD);
        int PiramidStartHeight = Random.Range(MinPiramidStartHeight_LD, MaxPiramidStartHeight_LD);

        for (int i = Start; i < End; i++)
        {
            for (int j = 0; j < WorldHeight; j++)
            {
                if(CheckTile(i, j) != 37)
                {
                    if (CheckTile(i, j) == 3)
                    {
                        if (Random.Range(0f, 101f) <= RubyChance_LD)
                        {
                            SetTileA(i, j, 39, true);
                        }
                        else
                        {
                            SetTileA(i, j, 15, true);
                        }
                    }
                    else if (CheckTile(i, j) == 2)
                    {
                        if (Random.Range(0f, 101f) <= RubyChance_LD)
                        {
                            SetTileA(i, j, 39, true);
                        }
                        else
                        {
                            SetTileA(i, j, 15, true);
                        }

                        if (Random.Range(0f, 101f) <= BackCactusChance_LD)
                        {
                            for (int h = 0; h < Random.Range(MinCactusHeight_LD, MaxCactusHeight_LD); h++)
                            {
                                SetTileA(i, j + 1 + h, 34, false);
                            }
                        }
                        if (Random.Range(0f, 101f) <= FrontCactusChance_LD)
                        {
                            for (int h = 0; h < Random.Range(MinCactusHeight_LD, MaxCactusHeight_LD); h++)
                            {
                                SetTileA(i, j + 1 + h, 34, true);
                            }
                        }
                    }
                    else if (CheckTile(i, j) == 17)
                    {
                        SetTileA(i, j, 15, true);
                    }
                }
            }
            
            if (i == (End - Start) / 2 + Start)
            {
                for(int j = 0; j < PiramidWidth; j++)
                    SetTileA(i, PiramidStartHeight + j, 40, true);
                
                for (int j = 1; j < PiramidWidth; j++)
                    for (int h = 0; h < PiramidWidth; h++)
                        if(PiramidStartHeight + h - j >= PiramidStartHeight)
                            SetTileA(i-j, PiramidStartHeight + h - j, 40, true);

                for (int j = 1; j < PiramidWidth; j++)
                    for (int h = 0; h < PiramidWidth; h++)
                        if (PiramidStartHeight + h - j >= PiramidStartHeight)
                            SetTileA(i + j, PiramidStartHeight + h - j, 40, true);


                for (int j = 0; j < PiramidWidth; j++)
                    SetTileA(i, PiramidStartHeight + j, 40, false);

                for (int j = 1; j < PiramidWidth; j++)
                    for (int h = 0; h < PiramidWidth; h++)
                        if (PiramidStartHeight + h - j >= PiramidStartHeight)
                            SetTileA(i - j, PiramidStartHeight + h - j, 40, false);

                for (int j = 1; j < PiramidWidth; j++)
                    for (int h = 0; h < PiramidWidth; h++)
                        if (PiramidStartHeight + h - j >= PiramidStartHeight)
                            SetTileA(i + j, PiramidStartHeight + h - j, 40, false);

                if(Random.Range(0, 2) == 0)
                {
                    UseTemplate(PiramidEntrance_Right_LD, i - 2, PiramidStartHeight + PiramidWidth - 17);
                }
                else
                {
                    UseTemplate(PiramidEntrance_Left_LD, i - 9, PiramidStartHeight + PiramidWidth - 17);
                }

                UseTemplate(Level1PiramidRooms_LD[Random.Range(0, Level1PiramidRooms_LD.Length)], i, PiramidStartHeight + PiramidWidth - 27);
            }
        }


        for (int i = OtherStart; i < (OtherEnd - OtherStart) / 2 + OtherStart; i++)
        {
            if(i != OtherStart)
            {
                CurrentHeight += Random.Range(MinIncrease_WM, MaxIncrease_WM + 1);
            }

            for (int j = 0; j < WorldHeight; j++)
            {
                if(CheckTile(i, j) == 2)
                {
                    SetTileA(i, j, 3, true);
                    if(i == OtherStart)
                    {
                        CurrentHeight = j;
                        j = WorldHeight;
                    }
                }
                if(j < CurrentHeight && CheckTile(i, j) == 0)
                {
                    if(j > SnowHeight_WM)
                    {
                        if (Random.Range(0f, 101f) <= SapphireChance_WM)
                        {
                            SetTileA(i, j, 38, true);
                        }
                        else
                        {
                            SetTileA(i, j, 35, true);
                        }
                    }
                    else
                    {
                        SetTileA(i, j, 3, true);
                    }
                }
            }
            for (int j = 0; j < WorldHeight; j++)
            {
                if (CheckTile(i, j) == 3 && CheckTile(i, j+1) == 0)
                {
                    SetTileA(i, j, 2, true);
                }
            }
        }


        //if(Random.Range(0, 2) == 0)
        //{

        //}
        //else
        //{

        //}

        //for (int i = Start; i < End; i++)
        //{
        //    for (int j = 0; j < WorldHeight; j++)
        //    {
        //        if (CheckTile(i, j) == 3)
        //        {
        //            SetTileA(i, j, 5, true);
        //        }
        //        else if (CheckTile(i, j) == 2)
        //        {
        //            SetTileA(i, j, 4, true);
        //        }
        //    }
        //}

        //for (int i = 0; i < WorldWidth; i++)
        //{
        //    for (int j = 0; j < WorldHeight; j++)
        //    {
        //        SetTileA(i, j, 22, false);
        //    }
        //}
    }

    void FixedUpdate()
    {
        Ref.TilesChanged = true;

        if (player.rb.transform.position.x > LastPos.x + Ref.ChunkSize ||
            player.rb.transform.position.x < LastPos.x - Ref.ChunkSize ||
            player.rb.transform.position.y > LastPos.y + Ref.ChunkSize ||
            player.rb.transform.position.y < LastPos.y - Ref.ChunkSize)
        {
            RefreshRange();
            LastPos = player.rb.transform.position;
        }
        
        //RwfreshFoliage();
    }

    public void UseTemplate(TileTemplate Temp, int x, int y)
    {
        for (int i = 0; i < Temp.TileIDS.Length; i++)
        {
            if (Temp.FrontRemove[i] == true)
                SetTileA(i % Temp.Width + x + Temp.XOffset, i / Temp.Width + y + Temp.YOffset, 0, true);

            if (Temp.BackRemove[i] == true)
                SetTileA(i % Temp.Width + x + Temp.XOffset, i / Temp.Width + y + Temp.YOffset, 0, false);


            if (Temp.TileIDS[i] > 0)
                SetTileA(i % Temp.Width + x + Temp.XOffset, i / Temp.Width + y + Temp.YOffset, Temp.TileIDS[i], true);

            if (Temp.BackTileIDS[i] > 0)
                SetTileA(i % Temp.Width + x + Temp.XOffset, i / Temp.Width + y + Temp.YOffset, Temp.BackTileIDS[i], false);
        }
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
                    Ref.TilesChanged = true;

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
            }

            Pos.x++;
    }

    public void SetTileA(int X, int Y, int ID, bool IsFront)
    {
        Ref.TilesChanged = true;

        if (IsFront)
        {
            FrontTileIDS[X + Y * WorldWidth].ID = ID;
            FrontTileIDS[X + Y * WorldWidth].HealthSet = false;
        }
        else
        {
            BackTileIDS[X + Y * WorldWidth].ID = ID;
            BackTileIDS[X + Y * WorldWidth].HealthSet = false;
        }
    }

    public void RefreshRange()
    {
        Ref.TilesChanged = true;

        for (int i = (int)Mathf.Round(player.rb.transform.position.y) - Ref.TileRange / 2; i < (int)Mathf.Round(player.rb.transform.position.y) + Ref.TileRange / 2; i++)
        {
            for (int j = (int)Mathf.Round(player.rb.transform.position.x) - Ref.TileRange / 2; j < (int)Mathf.Round(player.rb.transform.position.x) + Ref.TileRange / 2; j++)
            {
                if (FurnitureIDS[j + i * WorldWidth] > 0)
                {
                    bool SpaceEmpty = true;

                    for (int k = 0; k < FurnitureTypes[FurnitureIDS[j + i * WorldWidth]].Height; k++)
                    {
                        for (int y = 0; y < FurnitureTypes[FurnitureIDS[j + i * WorldWidth]].Width; y++)
                        {
                            if (Physics2D.OverlapCircle(new Vector2(j + y, i + k), 0.45f, Ref.FurnitureMask))
                            {
                                SpaceEmpty = false;
                            }
                        }
                    }

                    if (SpaceEmpty)
                    {
                        if (FurnitureTypes[FurnitureIDS[j + i * WorldWidth]] != null)
                        {
                            if (FurnitureTypes[FurnitureIDS[j + i * WorldWidth]].GroundPlaced != null)
                            {
                                FurnitureTypes[FurnitureIDS[j + i * WorldWidth]].GroundPlaced.SetActive(true);
                                Instantiate(FurnitureTypes[FurnitureIDS[j + i * WorldWidth]].GroundPlaced, new Vector2(j, i), transform.rotation);
                                FurnitureTypes[FurnitureIDS[j + i * WorldWidth]].GroundPlaced.SetActive(false);
                            }
                            else if (FurnitureTypes[FurnitureIDS[j + i * WorldWidth]].LeftWallMount != null)
                            {
                                FurnitureTypes[FurnitureIDS[j + i * WorldWidth]].LeftWallMount.SetActive(true);
                                Instantiate(FurnitureTypes[FurnitureIDS[j + i * WorldWidth]].LeftWallMount, new Vector2(j, i), transform.rotation);
                                FurnitureTypes[FurnitureIDS[j + i * WorldWidth]].LeftWallMount.SetActive(false);
                            }
                            else if (FurnitureTypes[FurnitureIDS[j + i * WorldWidth]].RightWallMount != null)
                            {
                                FurnitureTypes[FurnitureIDS[j + i * WorldWidth]].RightWallMount.SetActive(true);
                                Instantiate(FurnitureTypes[FurnitureIDS[j + i * WorldWidth]].RightWallMount, new Vector2(j, i), transform.rotation);
                                FurnitureTypes[FurnitureIDS[j + i * WorldWidth]].RightWallMount.SetActive(false);
                            }
                            else if (FurnitureTypes[FurnitureIDS[j + i * WorldWidth]].CeilingMount != null)
                            {
                                FurnitureTypes[FurnitureIDS[j + i * WorldWidth]].CeilingMount.SetActive(true);
                                Instantiate(FurnitureTypes[FurnitureIDS[j + i * WorldWidth]].CeilingMount, new Vector2(j, i), transform.rotation);
                                FurnitureTypes[FurnitureIDS[j + i * WorldWidth]].CeilingMount.SetActive(false);
                            }
                            else if (FurnitureTypes[FurnitureIDS[j + i * WorldWidth]].BackgroundMount != null)
                            {
                                FurnitureTypes[FurnitureIDS[j + i * WorldWidth]].BackgroundMount.SetActive(true);
                                Instantiate(FurnitureTypes[FurnitureIDS[j + i * WorldWidth]].BackgroundMount, new Vector2(j, i), transform.rotation);
                                FurnitureTypes[FurnitureIDS[j + i * WorldWidth]].BackgroundMount.SetActive(false);
                            }
                        }
                    }
                }
            }
        }

        Vector3Int Pos2 = new Vector3Int((int)Mathf.Round(LastPos.x) - Ref.TileRange / 2, (int)Mathf.Round(LastPos.y) - Ref.TileRange / 2, 0);
        for (int i = 0; i < Ref.TileRange * Ref.TileRange * 1; i++)
        {
            WaterTiles.SetTile(Pos2, null);
            FrontTiles.SetTile(Pos2, null);
            BackTiles.SetTile(Pos2, null);

            Pos2.x++;

            if (i % Ref.TileRange * 1 == 0)
            {
                Pos2.x = (int)Mathf.Round(LastPos.x) - Ref.TileRange / 2;
                Pos2.y++;
            }
        }

        Vector3Int Pos = new Vector3Int((int)Mathf.Round(player.rb.transform.position.x) - Ref.TileRange / 2, (int)Mathf.Round(player.rb.transform.position.y) - Ref.TileRange / 2, 0);
        for (int i = 0; i < Ref.TileRange * Ref.TileRange *1; i++)
        {
            if (!TileTypes[CheckTile(Pos.x, Pos.y - 1)].IsLiquid)
            {
                WaterTiles.SetTile(Pos, null);
                FrontTiles.SetTile(Pos, TileTypes[CheckTile(Pos.x, Pos.y - 1)].tile);
            }
            else
            {
                WaterTiles.SetTile(Pos, TileTypes[CheckTile(Pos.x, Pos.y - 1)].tile);
                FrontTiles.SetTile(Pos, null);
            }
            BackTiles.SetTile(Pos, TileTypes[CheckBackTile(Pos.x, Pos.y - 1)].tile);

            Pos.x++;

            if (i % Ref.TileRange*1 == 0)
            {
                Pos.x = (int)Mathf.Round(player.rb.transform.position.x) - Ref.TileRange / 2;
                Pos.y++;
            }
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

                Ref.TilesChanged = true;

                if(TileTypes[FrontTileIDS[x + y * WorldWidth].ID].Drop != null)
                {
                    if (Ref.CanAdd(TileTypes[FrontTileIDS[x + y * WorldWidth].ID].Drop.GetComponent<item>().ItemID, 1) > 0)
                    {
                        Ref.AddItem(TileTypes[FrontTileIDS[x + y * WorldWidth].ID].Drop.GetComponent<item>().ItemID);
                    }
                    else
                    {
                        Instantiate(TileTypes[FrontTileIDS[x + y * WorldWidth].ID].Drop, new Vector2(x, y), transform.rotation);
                    }
                }

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
                Ref.TilesChanged = true;

                Instantiate(TileTypes[BackTileIDS[x + y * WorldWidth].ID].BreakParticles, new Vector2(x, y), transform.rotation);
                Instantiate(TileTypes[BackTileIDS[x + y * WorldWidth].ID].BreakParticles, new Vector2(x, y), transform.rotation);
                Instantiate(TileTypes[BackTileIDS[x + y * WorldWidth].ID].BreakParticles, new Vector2(x, y), transform.rotation);

                if (TileTypes[BackTileIDS[x + y * WorldWidth].ID].Drop != null)
                {
                    if (Ref.CanAdd(TileTypes[BackTileIDS[x + y * WorldWidth].ID].Drop.GetComponent<item>().ItemID, 1) > 0)
                    {
                        Ref.AddItem(TileTypes[BackTileIDS[x + y * WorldWidth].ID].Drop.GetComponent<item>().ItemID);
                    }
                    else
                    {
                        Instantiate(TileTypes[BackTileIDS[x + y * WorldWidth].ID].Drop, new Vector2(x, y), transform.rotation);
                    }
                }

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
        Ref.TilesChanged = true;

        if (IsFront)
        {
            FrontTileIDS[X + Y * WorldWidth].ID = (byte)ID;
            Instantiate(TileTypes[FrontTileIDS[X + Y * WorldWidth].ID].BreakParticles, new Vector2(X, Y), transform.rotation);
            FrontTileIDS[X + Y * WorldWidth].HealthSet = false;
        }
        else
        {
            BackTileIDS[X + Y * WorldWidth].ID = (byte)ID;
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

            if (!TileTypes[FrontTileIDS[X + Y * WorldWidth].ID].IsLiquid && BreakPower > -1)
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

            if (!TileTypes[BackTileIDS[X + Y * WorldWidth].ID].IsLiquid && BreakPower > -1)
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
        if (X >= WorldWidth || X < 0 || Y >= WorldHeight || Y < 0)
            return 0;
        else
            return FrontTileIDS[X + Y * WorldWidth].ID;
    }
    public int CheckBackTile(int X, int Y)
    {
        if (X >= WorldWidth || X < 0 || Y >= WorldHeight || Y < 0)
            return 0;
        else
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