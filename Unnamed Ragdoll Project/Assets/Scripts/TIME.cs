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

        transform.Rotate(0, 0, 0.02f / DayLength * -360);

        if(Input.GetKey(KeyCode.O))
        {
            TimeMultiplier = 20;
        }
        else
        {
            TimeMultiplier = 1;
        }
    }
}