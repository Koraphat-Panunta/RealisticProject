using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarRamper : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody rb;
    public float velocity;
    public float HitDamage;
    private float damageMutiply = 8;

    public Vector3 VelocityNormalized;
    private float deltaUpdate = 0.5f;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        velocity = rb.velocity.magnitude;
        HitDamage = velocity*damageMutiply;
        if(deltaUpdate > 0)
        {
            VelocityNormalized = rb.velocity.normalized;
            deltaUpdate = 5;
        }
        else
        {
            deltaUpdate -= Time.deltaTime;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.TryGetComponent<BodyPart>(out BodyPart bodyPart))
        {
            float dot = Vector3.Dot(VelocityNormalized, (bodyPart.transform.position - transform.position).normalized);
            dot = Mathf.Abs(dot);
            bodyPart.GotHit(HitDamage * dot);
        }
    }
    
}
