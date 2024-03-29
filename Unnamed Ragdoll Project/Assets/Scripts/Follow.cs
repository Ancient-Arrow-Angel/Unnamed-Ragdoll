using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    [SerializeField]
    bool XFollow = true;
    [SerializeField]
    bool YFollow = true;
    public Vector2 Offset;
    public Transform Target;

    Vector2 Pos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Target != null)
        {
            Pos = transform.position;

            if (XFollow)
            {
                Pos.x = Target.position.x + Offset.x;
            }
            if (YFollow)
            {
                Pos.y = Target.position.y + Offset.y;
            }

            transform.position = Pos;
        }
    }
}