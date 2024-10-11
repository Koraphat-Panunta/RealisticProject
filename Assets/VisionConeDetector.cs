using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionConeDetector : MonoBehaviour
{
    public float visionRadius = 400;         // Radius of the vision cone
    public float visionAngle = 45f;          // Half-angle of the vision cone (in degrees)
    public LayerMask targetMask;             // LayerMask to filter specific layers (like enemies)
    public Transform playerEyes;             // Transform representing the player's eyes or direction

    public List<GameObject> FindTargetsInCone()
    {
        List<GameObject> targetsInCone = new List<GameObject>();

        // Step 1: Get all objects within the radius
        Collider[] targetsInRadius = Physics.OverlapSphere(transform.position, visionRadius, targetMask);

        // Step 2: Loop through all objects and filter by the angle of the vision cone
        foreach (Collider target in targetsInRadius)
        {
            Vector3 directionToTarget = (target.transform.position - transform.position).normalized;

            // Step 3: Check if the target is within the angle of the vision cone
            float angleBetween = Vector3.Angle(playerEyes.forward, directionToTarget);

            if (angleBetween < visionAngle)  // If within the vision cone
            {
                // Step 4: Perform raycast to ensure there's no obstacle blocking the view
                RaycastHit hit;
                if (Physics.Raycast(transform.position, directionToTarget, out hit, visionRadius, targetMask))
                {
                    if (hit.collider.gameObject == target.gameObject)  // Target is visible
                    {
                        targetsInCone.Add(target.gameObject);  // Add the target to the list
                    }
                }
            }
        }

        return targetsInCone;
    }
}
