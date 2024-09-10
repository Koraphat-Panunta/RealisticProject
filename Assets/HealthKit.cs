using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthKit : MonoBehaviour
{
    Vector3 orginPos;
    float multiplyPos = 0.2f;
    float timmerCount;
    private void Start()
    {
        orginPos = transform.position;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<CarModel>(out CarModel carModel))
        {
            carModel.SetHP(100);
            Destroy(gameObject);
        }
    }
    private void FixedUpdate()
    {
        timmerCount += Time.deltaTime;
        float sinResult = Mathf.Sin(timmerCount);
        transform.position = orginPos+new Vector3(0,sinResult*multiplyPos,0);
        transform.Rotate(0,30*Time.deltaTime,0);
    }
}
