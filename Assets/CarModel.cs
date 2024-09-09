using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarModel : Character
{
    
    // Start is called before the first frame update
    void Start()
    {
        base.HP = 100;
       
    }
    public override void TakeDamage(float Damage)
    {
        base.HP -= Damage*0.1f;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public float GetHP()
    {
        return base.HP;
    }
}
