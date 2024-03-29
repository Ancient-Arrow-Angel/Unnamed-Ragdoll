using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TIME : MonoBehaviour
{
    public Gradient SkyGradiant;
    public Gradient SunGradiant;
    public float time;
    public int DayLength;
    public Light2D Sun;
    public float TimeMultiplier;
    public SpriteRenderer[] Effected;
    public SpriteRenderer[] SkyEffected;

    Camera Cam;

    // Start is called before the first frame update
    void Start()
    {
        Cam = Camera.main;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Time.timeScale = TimeMultiplier;

        if(time >= 1)
        {
            time = 0;
        }
        time += 0.02f / DayLength;

        Cam.backgroundColor = SkyGradiant.Evaluate(time);
        Sun.color = SunGradiant.Evaluate(time);
        for(int i = 0; i < Effected.Length; i++)
        {
            Effected[i].color = SunGradiant.Evaluate(time);
        }
        for (int i = 0; i < SkyEffected.Length; i++)
        {
            SkyEffected[i].color = new Color(SkyGradiant.Evaluate(time).r, SkyGradiant.Evaluate(time).g, SkyGradiant.Evaluate(time).b, SkyEffected[i].color.a);
        }

        transform.Rotate(0, 0, 0.02f / DayLength * -360);
        //transform.rotation = new Quaternion(0, 0, -time * 360 + 180, transform.rotation.w);

        if (Input.GetKey(KeyCode.O))
        {
            TimeMultiplier = 24;
        }
        else
        {
            TimeMultiplier = 1;
        }
    }
}