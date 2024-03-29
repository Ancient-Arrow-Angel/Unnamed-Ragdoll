using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightGRADE : MonoBehaviour
{
    public Gradient LightColor;
    public float TimeMultyplyer;
    public bool DestroyOnEnd;

    float time;
    Light2D ThisLight;

    // Start is called before the first frame update
    void Start()
    {
        ThisLight = GetComponent<Light2D>();
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime * TimeMultyplyer;

        if(time > 1)
        {
            if (DestroyOnEnd)
                DestroyImmediate(this.gameObject);

            time = 0;
        }

        ThisLight.color = LightColor.Evaluate(time);
    }
}
