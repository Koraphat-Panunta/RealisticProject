using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableKinamatic : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<BodyPart>(out BodyPart bodyPart))
        {
            bodyPart.GetComponent<Rigidbody>().isKinematic = false;
        }
    }
}
