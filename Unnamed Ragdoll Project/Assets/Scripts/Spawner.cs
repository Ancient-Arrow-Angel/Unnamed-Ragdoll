using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Spawn[] Spawns;
     
    public float MinSpawnTimer;
    public float MaxSpawnTimer;

    public float MaxEnemies;
    public float CurrentEnemies;

    float Timer;

    public float Range;

    TileMaker tileMaker;
    Player player;

    // Start is called before the first frame update
    void Start()
    {
        Timer = Random.Range(MinSpawnTimer, MaxSpawnTimer);
        tileMaker = GameObject.Find("Grid").GetComponent<TileMaker>();
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if(CurrentEnemies < MaxEnemies)
        {
            Timer -= Time.deltaTime;

            if (Timer <= 0)
            {
                bool Worked = false;
                while (!Worked)
                {
                    int ToSpawn = Random.Range(0, Spawns.Length);
                    if (Random.Range(0, 101) >= Spawns[ToSpawn].SpawnRarity)
                    {
                        Worked = true;
                        Instantiate(Spawns[ToSpawn].ToSpawn, new Vector2(Random.Range(player.rb.transform.position.x - Range, player.rb.transform.position.x + Range), tileMaker.WorldHeight + 10), transform.rotation);
                    }
                }
                CurrentEnemies++;
                Timer = Random.Range(MinSpawnTimer, MaxSpawnTimer);
            }
        }
    }
}

[System.Serializable]
public class Spawn
{
    public GameObject ToSpawn;
    public float SpawnRarity;
}