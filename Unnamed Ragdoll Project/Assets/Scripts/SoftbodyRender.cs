using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class SoftbodyRender : MonoBehaviour
{
    #region Constraints
    const float splineOffset = 0.4f;
    #endregion

    #region Fields
    [SerializeField]
    SpriteShapeController SpriteShape;
    [SerializeField]
    Transform[] points;
    #endregion

    #region MonoBehaviour Callbacks
    void Awake()
    {
        UpdateVerticies();
    }

    private void Update()
    {
        UpdateVerticies();
    }
    #endregion

    #region privateMethods
    void UpdateVerticies()
    {
        for (int i  = 0; i < points.Length -1; i++)
        {
            Vector2 _vertex = points[i].localPosition;

            Vector2 _towardsCenter = (Vector2.zero - _vertex).normalized;

            float _ColliderRadius = points[i].gameObject.GetComponent<CircleCollider2D>().radius;
            try
            {
                SpriteShape.spline.SetPosition(i, (_vertex - _towardsCenter * (_ColliderRadius + splineOffset)));
            }
            catch
            {
                Debug.Log("Spline points are too close to each other.. recalculate");
                SpriteShape.spline.SetPosition(i, (_vertex - _towardsCenter * (_ColliderRadius - splineOffset)));
            }
        }
    }
    #endregion
}