using UnityEngine;
using UnityEngine.U2D.IK;

public class Collide : MonoBehaviour
{
    public GameObject TurnOn;
    public LayerMask CanCollide;
    public float Size;
    public float DamageMultiplyer;
    public bool DealsDamage;

    private void Update()
    {
        if (Physics2D.OverlapCircle(transform.position, Size, CanCollide) && Physics2D.OverlapCircle(transform.position, Size, CanCollide).GetComponent<item>() == null ||
            Physics2D.OverlapCircle(transform.position, Size, CanCollide) && Physics2D.OverlapCircle(transform.position, Size, CanCollide).GetComponent<item>() != null && Physics2D.OverlapCircle(transform.position, Size, CanCollide).GetComponent<item>().Held == false)
        {
            if(DealsDamage)
            {
                if (Physics2D.OverlapCircle(transform.position, Size, CanCollide).GetComponent<LimbHealth>() != null)
                {
                    LimbHealth Limb = Physics2D.OverlapCircle(transform.position, Size, CanCollide).GetComponent<LimbHealth>();
                    Limb.FlashAmount.b = 0;
                    Limb.FlashAmount.g = 0;

                    if(DamageMultiplyer * transform.parent.GetComponent<GetWeapon>().Creator.HeavyDamage * transform.parent.GetComponent<GetWeapon>().Creator.HeavyDamageMod - Limb.Defence <= 1)
                    {
                        Limb.Health--;
                    }
                    else
                    {
                        Limb.Health -= DamageMultiplyer * transform.parent.GetComponent<GetWeapon>().Creator.HeavyDamage * transform.parent.GetComponent<GetWeapon>().Creator.HeavyDamageMod - Limb.Defence;
                    }

                    if (DamageMultiplyer * transform.parent.GetComponent<GetWeapon>().Creator.LightDamage * transform.parent.GetComponent<GetWeapon>().Creator.LightDamageMod - Limb.Defence <= 1)
                    {
                        Limb.Health--;
                    }
                    else
                    {
                        Limb.Health -= DamageMultiplyer * transform.parent.GetComponent<GetWeapon>().Creator.LightDamage * transform.parent.GetComponent<GetWeapon>().Creator.LightDamageMod - Limb.Defence;
                    }

                    if (DamageMultiplyer * transform.parent.GetComponent<GetWeapon>().Creator.MagicDamage * transform.parent.GetComponent<GetWeapon>().Creator.MagicDamageMod - Limb.Defence <= 1)
                    {
                        Limb.Health--;
                    }
                    else
                    {
                        Limb.Health -= DamageMultiplyer * transform.parent.GetComponent<GetWeapon>().Creator.MagicDamage * transform.parent.GetComponent<GetWeapon>().Creator.MagicDamageMod - Limb.Defence;
                    }
                }
                else if (Physics2D.OverlapCircle(transform.position, Size, CanCollide).GetComponent<DamageInput>() != null)
                {
                    EnemyStats Enemy = Physics2D.OverlapCircle(transform.position, Size, CanCollide).GetComponent<DamageInput>().Attached;

                    if (DamageMultiplyer * transform.parent.GetComponent<GetWeapon>().Creator.HeavyDamage * transform.parent.GetComponent<GetWeapon>().Creator.HeavyDamageMod - Physics2D.OverlapCircle(transform.position, Size, CanCollide).GetComponent<DamageInput>().Defence <= 1)
                    {
                        Enemy.Health--;
                    }
                    else
                    {
                        Enemy.Health -= DamageMultiplyer * transform.parent.GetComponent<GetWeapon>().Creator.HeavyDamage * transform.parent.GetComponent<GetWeapon>().Creator.HeavyDamageMod - Physics2D.OverlapCircle(transform.position, Size, CanCollide).GetComponent<DamageInput>().Defence;
                    }

                    if (DamageMultiplyer * transform.parent.GetComponent<GetWeapon>().Creator.LightDamage * transform.parent.GetComponent<GetWeapon>().Creator.LightDamageMod - Physics2D.OverlapCircle(transform.position, Size, CanCollide).GetComponent<DamageInput>().Defence <= 1)
                    {
                        Enemy.Health--;
                    }
                    else
                    {
                        Enemy.Health -= DamageMultiplyer * transform.parent.GetComponent<GetWeapon>().Creator.LightDamage * transform.parent.GetComponent<GetWeapon>().Creator.LightDamageMod - Physics2D.OverlapCircle(transform.position, Size, CanCollide).GetComponent<DamageInput>().Defence;
                    }

                    if (DamageMultiplyer * transform.parent.GetComponent<GetWeapon>().Creator.MagicDamage * transform.parent.GetComponent<GetWeapon>().Creator.MagicDamageMod - Physics2D.OverlapCircle(transform.position, Size, CanCollide).GetComponent<DamageInput>().Defence <= 1)
                    {
                        Enemy.Health--;
                    }
                    else
                    {
                        Enemy.Health -= DamageMultiplyer * transform.parent.GetComponent<GetWeapon>().Creator.MagicDamage * transform.parent.GetComponent<GetWeapon>().Creator.MagicDamageMod - Physics2D.OverlapCircle(transform.position, Size, CanCollide).GetComponent<DamageInput>().Defence;
                    }
                }
            }

            TurnOn.SetActive(true);
            TurnOn.transform.position = transform.position;
            Destroy(gameObject);
        }
    }
}