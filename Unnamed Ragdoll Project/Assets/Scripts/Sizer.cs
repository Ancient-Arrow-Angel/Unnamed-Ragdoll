using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sizer : MonoBehaviour
{
    public AnimationCurve XSpeedCurve;
    public AnimationCurve YSpeedCurve;
    public float TimeScale;
    public float SpeedMod;
    float time;

    void Update()
    {
        time += Time.deltaTime * TimeScale;
        transform.localScale += new Vector3(XSpeedCurve.Evaluate(time) * Time.deltaTime * SpeedMod, YSpeedCurve.Evaluate(time) * Time.deltaTime * SpeedMod);
    }
}
