using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Collide : MonoBehaviour
{
    public GameObject TurnOn;
    public string[] CanCollide;
    public float DamageMultiplyer;
    public bool DealsDamage;
    public bool CanHeal;
    public int Pierces;
    public float InvinceTime;
    public float Range;

    public List<HitObject> HitObjects = new List<HitObject>();

    private void Update()
    {
        Range -= Time.deltaTime;

        if (Range <= 0)
        {
            if (TurnOn != null)
            {
                TurnOn.SetActive(true);
                Instantiate(TurnOn, transform.position, TurnOn.transform.rotation);
                TurnOn.SetActive(false);
            }

            Destroy(gameObject);
        }

        for (int i = 0; i < HitObjects.Count; i++)
        {
            HitObjects[i].Time -= Time.deltaTime;
        }

        for (int i = 0; i < HitObjects.Count; i++) 
        {
            if (HitObjects[i].Time <= 0)
            {
                HitObjects.RemoveAt(i);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (DealsDamage && collision.gameObject.GetComponent<item>() == null ||
            DealsDamage && collision.gameObject.GetComponent<item>() != null && !collision.gameObject.GetComponent<item>().Held)
        {
            bool IsHit = false;
            for (int i = 0; i < CanCollide.Length; ++i)
            {
                if (CanCollide[i] == collision.tag)
                {
                    IsHit = true;
                    i = 9999;
                }
            }

            if(IsHit)
            {
                bool CanHit = true;

                for (int i = 0; i < HitObjects.Count; i++)
                {
                    if (HitObjects[i].Hit == collision.gameObject)
                    {
                        CanHit = false;
                        i = 99999;
                    }
                }

                if (CanHit)
                {
                    HitObject GotHit = new HitObject();
                    GotHit.Hit = collision.gameObject;
                    GotHit.Time = InvinceTime;
                    HitObjects.Add(GotHit);

                    if (collision.gameObject.GetComponent<LimbHealth>() != null)
                    {
                        LimbHealth Limb = collision.gameObject.GetComponent<LimbHealth>();
                        float PreLimbHealth = Limb.Health;

                        if (DamageMultiplyer * transform.parent.GetComponent<GetWeapon>().Creator.HeavyDamage * transform.parent.GetComponent<GetWeapon>().Creator.HeavyDamageMod - Limb.Defence <= 1 && !CanHeal)
                        {
                            Limb.Health--;
                        }
                        else
                        {
                            Limb.Health -= DamageMultiplyer * transform.parent.GetComponent<GetWeapon>().Creator.HeavyDamage * transform.parent.GetComponent<GetWeapon>().Creator.HeavyDamageMod - Limb.Defence;
                        }

                        if (DamageMultiplyer * transform.parent.GetComponent<GetWeapon>().Creator.LightDamage * transform.parent.GetComponent<GetWeapon>().Creator.LightDamageMod - Limb.Defence <= 1 && !CanHeal)
                        {
                            Limb.Health--;
                        }
                        else
                        {
                            Limb.Health -= DamageMultiplyer * transform.parent.GetComponent<GetWeapon>().Creator.LightDamage * transform.parent.GetComponent<GetWeapon>().Creator.LightDamageMod - Limb.Defence;
                        }

                        if (DamageMultiplyer * transform.parent.GetComponent<GetWeapon>().Creator.MagicDamage * transform.parent.GetComponent<GetWeapon>().Creator.MagicDamageMod - Limb.Defence <= 1 && !CanHeal)
                        {
                            Limb.Health--;
                        }
                        else
                        {
                            Limb.Health -= DamageMultiplyer * transform.parent.GetComponent<GetWeapon>().Creator.MagicDamage * transform.parent.GetComponent<GetWeapon>().Creator.MagicDamageMod - Limb.Defence;
                        }

                        if (PreLimbHealth > Limb.Health)
                        {
                            Limb.FlashAmount.b = 0;
                            Limb.FlashAmount.g = 0;
                        }
                        else
                        {
                            Limb.FlashAmount.b = 0;
                            Limb.FlashAmount.r = 0;
                        }
                    }
                    else if (collision.gameObject.GetComponent<DamageInput>() != null)
                    {
                        EnemyStats Enemy = collision.gameObject.GetComponent<DamageInput>().Attached;

                        if (DamageMultiplyer * transform.parent.GetComponent<GetWeapon>().Creator.HeavyDamage * transform.parent.GetComponent<GetWeapon>().Creator.HeavyDamageMod - collision.gameObject.GetComponent<DamageInput>().Defence <= 1 && !CanHeal)
                        {
                            Enemy.Health--;
                        }
                        else
                        {
                            Enemy.Health -= DamageMultiplyer * transform.parent.GetComponent<GetWeapon>().Creator.HeavyDamage * transform.parent.GetComponent<GetWeapon>().Creator.HeavyDamageMod - collision.gameObject.GetComponent<DamageInput>().Defence;
                        }

                        if (DamageMultiplyer * transform.parent.GetComponent<GetWeapon>().Creator.LightDamage * transform.parent.GetComponent<GetWeapon>().Creator.LightDamageMod - collision.gameObject.GetComponent<DamageInput>().Defence <= 1 && !CanHeal)
                        {
                            Enemy.Health--;
                        }
                        else
                        {
                            Enemy.Health -= DamageMultiplyer * transform.parent.GetComponent<GetWeapon>().Creator.LightDamage * transform.parent.GetComponent<GetWeapon>().Creator.LightDamageMod - collision.gameObject.GetComponent<DamageInput>().Defence;
                        }

                        if (DamageMultiplyer * transform.parent.GetComponent<GetWeapon>().Creator.MagicDamage * transform.parent.GetComponent<GetWeapon>().Creator.MagicDamageMod - collision.gameObject.GetComponent<DamageInput>().Defence <= 1 && !CanHeal)
                        {
                            Enemy.Health--;
                        }
                        else
                        {
                            Enemy.Health -= DamageMultiplyer * transform.parent.GetComponent<GetWeapon>().Creator.MagicDamage * transform.parent.GetComponent<GetWeapon>().Creator.MagicDamageMod - collision.gameObject.GetComponent<DamageInput>().Defence;
                        }
                    }
                    else
                    {
                        Pierces = 0;
                    }

                    Pierces--;

                    if (Pierces <= 0)
                    {
                        if (TurnOn != null)
                        {
                            TurnOn.SetActive(true);
                            Instantiate(TurnOn, transform.position, TurnOn.transform.rotation);
                            TurnOn.SetActive(false);
                        }

                        Destroy(gameObject);
                    }
                }
            }
        }
    }
}

[System.Serializable]
public class HitObject
{
    public GameObject Hit;
    public float Time;
}