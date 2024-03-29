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
    public GameObject Skin;

    public Rigidbody2D rb;

    public GameObject Icon;
    public bool DontDespawn;

    Spawner Spawner;
    Reference Ref;

    // Start is called before the first frame update
    void Start()
    {
        Health = MaxHealth;
        if(!DontDespawn)
            Spawner = GameObject.Find("Enemy Spawner").GetComponent<Spawner>();
        Ref = GameObject.Find("Global Ref").GetComponent<Reference>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!DontDespawn && !Physics2D.OverlapCircle(rb.position, Spawner.Range, Ref.PlayerMask))
        {
            Spawner.CurrentEnemies--;
            DestroyImmediate(this.gameObject);
        }

        if (HealthPercent != null && GetComponent<HuminoidEnemyAI>() != null && GetComponent<HuminoidEnemyAI>().Pursuing)
            HealthPercent.text = Health.ToString() + "%";
        else if(HealthPercent != null)
            HealthPercent.text = "";

        if (Health <= 0)
        {
            if(GetComponent<HuminoidEnemyAI>() != null)
                GetComponent<HuminoidEnemyAI>().Music.SongOn = 3;

            if (GetComponent<HuminoidEnemyAI>() != null)
                GetComponent<HuminoidEnemyAI>().enabled = false;

            if (GetComponent<SlimeAI>() != null)
                GetComponent<SlimeAI>().enabled = false;

            if (GetComponentInChildren<LimbHealth>() != null)
                GetComponentInChildren<LimbHealth>().Health = 0;

            if(ToDestroy != null)
                Destroy(ToDestroy);

            LimbHealth[] Limbs = GetComponentsInChildren<LimbHealth>();

            for (int i = 1; i < GetComponentsInChildren<Transform>().Length; i++)
            {
                for(int h = 0; h < GetComponentsInChildren<Transform>()[i].GetComponentsInChildren<Transform>().Length; h++)
                {
                    for (int j = 0; j < GetComponentsInChildren<Transform>()[i].GetComponentsInChildren<Transform>()[h].GetComponents<HingeJoint2D>().Length; j++)
                    {
                        Destroy(GetComponentsInChildren<Transform>()[i].GetComponentsInChildren<Transform>()[h].GetComponents<HingeJoint2D>()[j]);
                    }
                    for (int j = 0; j < GetComponentsInChildren<Transform>()[i].GetComponentsInChildren<Transform>()[h].GetComponents<FixedJoint2D>().Length; j++)
                    {
                        Destroy(GetComponentsInChildren<Transform>()[i].GetComponentsInChildren<Transform>()[h].GetComponents<FixedJoint2D>()[j]);
                    }
                    for (int j = 0; j < GetComponentsInChildren<Transform>()[i].GetComponentsInChildren<Transform>()[h].GetComponents<SpringJoint2D>().Length; j++)
                    {
                        Destroy(GetComponentsInChildren<Transform>()[i].GetComponentsInChildren<Transform>()[h].GetComponents<SpringJoint2D>()[j]);
                    }
                    for (int j = 0; j < GetComponentsInChildren<Transform>()[i].GetComponentsInChildren<Transform>()[h].GetComponents<DistanceJoint2D>().Length; j++)
                    {
                        Destroy(GetComponentsInChildren<Transform>()[i].GetComponentsInChildren<Transform>()[h].GetComponents<DistanceJoint2D>()[j]);
                    }
                    for (int j = 0; j < GetComponentsInChildren<Transform>()[i].GetComponentsInChildren<Transform>()[h].GetComponents<DamageInput>().Length; j++)
                    {
                        Destroy(GetComponentsInChildren<Transform>()[i].GetComponentsInChildren<Transform>()[h].GetComponents<DamageInput>()[j]);
                    }
                    for (int j = 0; j < GetComponentsInChildren<Transform>()[i].GetComponentsInChildren<Transform>()[h].GetComponents<Attract>().Length; j++)
                    {
                        Destroy(GetComponentsInChildren<Transform>()[i].GetComponentsInChildren<Transform>()[h].GetComponents<Attract>()[j]);
                    }
                    for (int j = 0; j < GetComponentsInChildren<Transform>()[i].GetComponentsInChildren<Transform>()[h].GetComponents<Transform>().Length; j++)
                    {
                        GetComponentsInChildren<Transform>()[i].GetComponentsInChildren<Transform>()[h].GetComponents<Transform>()[j].gameObject.layer = LayerMask.NameToLayer("Ground");
                        GetComponentsInChildren<Transform>()[i].GetComponentsInChildren<Transform>()[h].GetComponents<Transform>()[j].gameObject.tag = "Ground";
                    }
                }
            }

            if (Skin != null)
                Destroy(Skin.gameObject);
            if(Spawner != null)
                Spawner.CurrentEnemies--;
            Destroy(this.gameObject, 10);
            if (Icon != null)
                Destroy(Icon);
            this.enabled = false;
        }
    }
}