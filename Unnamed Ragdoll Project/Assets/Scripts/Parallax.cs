using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float length, startpos;
    private float length2, startpos2;
    public GameObject cam;
    public float parallexEffect;

    public float YparallexEffect;


    void Start()
    {
        startpos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;

        startpos2 = transform.position.y;
        length2 = GetComponent<SpriteRenderer>().bounds.size.y;
    }
    void Update()
    {
        float temp = (cam.transform.position.x * (1 - parallexEffect));
        float dist = (cam.transform.position.x * parallexEffect);

        float temp2 = (cam.transform.position.y * (1 - parallexEffect));
        float dist2 = (cam.transform.position.y * parallexEffect);

        transform.position = new Vector3(startpos + dist, transform.position.y, transform.position.z);
        if (temp > startpos + length) startpos += length;
        else if (temp < startpos - length) startpos -= length;
    }
}