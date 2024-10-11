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
        CarModel = FindObjectOfType<CarModel>();
    }

    // Update is called once per frame
    void Update()
    {
        HPbar.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, CarModel.GetHP());
    }
}
