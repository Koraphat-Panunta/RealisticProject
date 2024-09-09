using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Missile : MonoBehaviour
{
    // Start is called before the first frame update
    bool IsRotatetoTarget = false;
    public GameObject target;
    float forceLunch = 15 ;
    private Vector3 randomPos;
    void Start()
    {
        gameObject.transform.forward = Vector3.up;
        GetComponent<Rigidbody>().AddForce(transform.forward*forceLunch, ForceMode.Impulse);
        StartCoroutine(LunchMissile());
        randomPos = new Vector3(Random.Range(-400, 400) + transform.position.x, Random.Range(-400, 400) + transform.position.y, Random.Range(-400, 400) + transform.position.z);
    }

    // Update is called once per fram
    private void FixedUpdate()
    {

        if (IsRotatetoTarget == true)
        {
            if (target != null)
            {
                Vector3 dir = target.transform.position - gameObject.transform.position;
                RotateTowards(dir,20);
                GetComponent<Rigidbody>().AddForce(dir.normalized * 4000 * Time.deltaTime, ForceMode.Force);
                Debug.Log("Cruise Missile in coming");
            }
            else
            {
                Vector3 dir = randomPos - transform.position;
                RotateTowards(dir, 20);
                GetComponent<Rigidbody>().AddForce(dir.normalized * 4000 * Time.deltaTime, ForceMode.Force);
                Debug.Log("Cruise Missile in coming randomly");
            }
        }
        else
        {
            if (target != null)
            {
                Vector3 dir = target.transform.position - gameObject.transform.position;
                RotateTowards(dir,1);
            }
            else
            {
                Vector3 dir = randomPos - transform.position;
                RotateTowards(dir,1);              
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Collider[] Hitcoller = Physics.OverlapSphere(transform.position, 7);
        foreach (Collider col in Hitcoller)
        {
            if (col.TryGetComponent<BodyPart>(out BodyPart body))
            {
                Enemy enemy = body.GetEnemy();
                enemy.GetComponent<RagdollAnimation>().enableRagdoll = true;
                col.GetComponent<Rigidbody>().AddExplosionForce(10, transform.position, 7,3,ForceMode.Impulse);
                enemy.TakeDamage(1000);
            }
        }
        Debug.Log("Missile Hit");
        Destroy(gameObject);
    }
    
    IEnumerator LunchMissile()
    {
        yield return new WaitForSeconds(1f);
        IsRotatetoTarget = true;    
    }
    protected void RotateTowards(Vector3 direction, float rotationSpeed )
    {
        // Ensure the direction is normalized
        direction.Normalize();

        // Flatten the direction vector to the XZ plane to only rotate around the Y axis
        direction.y = 0;

        // Check if the direction is not zero to avoid setting a NaN rotation
        if (direction != Vector3.zero)
        {
            // Calculate the target rotation based on the direction
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // Smoothly rotate towards the target rotation
            gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
