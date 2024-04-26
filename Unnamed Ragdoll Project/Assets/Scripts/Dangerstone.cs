using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dangerstone : MonoBehaviour
{
    public GameObject Explosion;

    Furniture Furn;

    // Start is called before the first frame update
    void Start()
    {
        Furn = GetComponent<Furniture>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Instantiate(Explosion, new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y)), Quaternion.Euler(0, 0, 0)).GetComponent<GetWeapon>().Creator = this.gameObject.GetComponent<item>();
        Furn.Ref.grid.FurnitureIDS[(int)Mathf.Round(transform.position.x) + (int)Mathf.Round(transform.position.y) * Furn.Ref.grid.WorldWidth] = 0;
        Destroy(this.gameObject);
    }
}