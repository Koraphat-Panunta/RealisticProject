using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHpbar : MonoBehaviour
{
    // Start is called before the first frame update
    public RectTransform HPbar;
    public CarModel CarModel;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Car HP :" + CarModel.GetHP());
        HPbar.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, CarModel.GetHP());
    }
}
