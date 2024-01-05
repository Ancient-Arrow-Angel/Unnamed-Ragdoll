using UnityEngine;

public class MouseFollow : MonoBehaviour
{
    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        transform.position = mousePos;
    }
}