using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public float Health;
    public float MaxHealth;
    public TextMeshProUGUI HealthPercent;
    public GameObject ToDestroy;

    public GameObject Icon;

    Spawner Spawn;

    // Start is called before the first frame update
    void Start()
    {
        Health = MaxHealth;
        Spawn = GameObject.Find("Enemy Spawner").GetComponent<Spawner>();
    }

    // Update is called once per frame
    void Update()
    {
        if(HealthPercent != null && GetComponent<HuminoidEnemyAI>() != null && GetComponent<HuminoidEnemyAI>().Pursuing)
            HealthPercent.text = Health.ToString() + "%";
        else if(HealthPercent != null)
            HealthPercent.text = "";

        if (Health <= 0)
        {
            Spawn.CurrentEnemies--;
            GetComponent<HuminoidEnemyAI>().Music.SongOn = 3;

            if (GetComponent<HuminoidEnemyAI>() != null)
            {
                GetComponent<HuminoidEnemyAI>().enabled = false;
            }
            GetComponentInChildren<LimbHealth>().Health = 0;

            if(ToDestroy != null)
                Destroy(ToDestroy);

            LimbHealth[] Limbs = GetComponentsInChildren<LimbHealth>();
            for (int i = 0; i < Limbs.Length; i++)
            {
                if (Limbs[i].GetComponent<HingeJoint2D>() != null)
                    Destroy(Limbs[i].GetComponent<HingeJoint2D>());
                if (Limbs[i].GetComponent<FixedJoint2D>() != null)
                    Destroy(Limbs[i].GetComponent<FixedJoint2D>());
            }
            Destroy(this.gameObject, 10);
            Destroy(Icon);
            this.enabled = false;
        }
    }
}