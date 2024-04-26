using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MomentumBullet : MonoBehaviour
{
    public AnimationCurve XSpeedCurve;
    public AnimationCurve YSpeedCurve;
    public float TimeScale;
    public float SpeedMod;
    Rigidbody2D rb;
    float time;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        time += TimeScale * Time.deltaTime;
        transform.localPosition += new Vector3(XSpeedCurve.Evaluate(time) * Time.deltaTime * SpeedMod, YSpeedCurve.Evaluate(time) * Time.deltaTime * SpeedMod);
    }
}