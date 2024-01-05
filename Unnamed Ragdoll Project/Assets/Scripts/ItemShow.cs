using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemShow : MonoBehaviour
{
    public Grab Hand;
    Image ThisImage;
    TextMeshProUGUI ThisNum;
    Reference Ref;

    // Start is called before the first frame update
    void Start()
    {
        Ref = GameObject.Find("Global Ref").GetComponent<Reference>();
        ThisImage = GetComponent<Image>();
        ThisNum = GetComponentInChildren<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Hand.HeldID != 0)
        {
            if(Hand.HeldAmount > 1)
            {
                ThisNum.text = Hand.HeldAmount.ToString();
            }
            else
            {
                ThisNum.text = "";
            }

            ThisImage.color = new Color(255, 255, 255, 255);
            ThisImage.sprite = Ref.Items[Hand.HeldID].icon;
        }
        else
        {
            ThisNum.text = "";
            ThisImage.color = new Color(0, 0, 0, 0);
            ThisImage.sprite = null;
        }
    }
}