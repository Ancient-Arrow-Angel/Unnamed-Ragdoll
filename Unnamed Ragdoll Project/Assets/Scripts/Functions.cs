using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Functions : MonoBehaviour
{
    public void Cheat()
    {
        Player player = GameObject.Find("Player").GetComponent<Player>();

        player.DeathPart.Play();
        player.anim.Play("Idle");
        player.Health = player.MaxHealth;
        LimbHealth[] Limbs = player.GetComponentsInChildren<LimbHealth>();
        for (int i = 0; i < Limbs.Length; i++)
        {
            Limbs[i].Health = Limbs[i].MaxHealth;
            Limbs[i].enabled = true;

            if(Limbs[i].GetComponent<Balance>() != null)
                Limbs[i].GetComponent<Balance>().enabled = true;
            if (Limbs[i].GetComponent<Arms>() != null)
                Limbs[i].GetComponent<Arms>().enabled = true;
            if (Limbs[i].GetComponent<Grab>() != null)
                Limbs[i].GetComponent<Grab>().enabled = true;
            if (Limbs[i].GetComponent<HingeJoint2D>() != null && Limbs[i].GetComponent<Arms>() == null)
                Limbs[i].GetComponent<HingeJoint2D>().useLimits = true;

            player.LeftLeg.GetComponent<HingeJoint2D>().useLimits = false;
            player.RightLeg.GetComponent<HingeJoint2D>().useLimits = false;
            player.LeftFoot.GetComponent<HingeJoint2D>().useLimits = false;
            player.RightFoot.GetComponent<HingeJoint2D>().useLimits = false;
        }
        player.enabled = true;
    }
}