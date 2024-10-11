using System.Collections.Generic;
using UnityEngine;

public class HeavyMachineGun : MonoBehaviour
{
    private FieldOfView _weaponView;
    [SerializeField] private List<GameObject> targetLocked;
    [SerializeField] private GameObject currentLockTargetSingle;
    [SerializeField] LayerMask lockAbleMask;
    public Transform bulletSpawner;
    public Transform missileSpawner;
    public int targetIndex = 0;

    public GameObject Bullet;
    public GameObject Missile;

    public float firingRecovery;
    public float lunchingMissileRecovery;

    public RectTransform ui;
    private void Start()
    {
        _weaponView = new FieldOfView(400, 137, gameObject.transform);
    
        ui.gameObject.SetActive(false);
    }
    private void Update()
    {
        PerformedInput();
        targetLocked = FindTarget();
       
       
        if (targetLocked.Count > 0)
        {
            foreach (GameObject t in targetLocked)
            {
                if (currentLockTargetSingle == null)
                {
                    if (t.GetComponent<ChestBodyPart>().GetEnemy().GetHP() > 0)
                    {
                        currentLockTargetSingle = t;
                        break;
                    }

                }
                else if (t == currentLockTargetSingle)
                {
                    if (t.GetComponent<ChestBodyPart>().GetEnemy().GetHP() <= 0)
                    {
                        currentLockTargetSingle = null;
                    }
                    break;
                }
                else
                {
                    if (t == targetLocked[targetLocked.Count - 1])
                    {
                        currentLockTargetSingle = null;
                    }
                }
            }
        }
        else
        {
            currentLockTargetSingle = null;

        }
        if (currentLockTargetSingle != null)
        {
           
            ui.gameObject.SetActive(true);
            ui.position = Camera.main.WorldToScreenPoint(currentLockTargetSingle.transform.position);
        }
        else
        {

            ui.gameObject.SetActive(false);
        }

    }
    private List<GameObject> FindTarget()
    {
        float visionRadius = 45f;         // Radius of the vision cone
        float visionAngle = 45f;          // Half-angle of the vision cone (in degrees)
        LayerMask targetMask = lockAbleMask;        // LayerMask to filter specific layers (like enemies)
        Transform playerEyes = gameObject.transform;
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
    private void SwitchTargetLocked()
    {
        if (targetLocked.Count > 0)
        {
            if (targetIndex + 1 > targetLocked.Count - 1)
            {
                targetIndex = 0;
                currentLockTargetSingle = targetLocked[targetIndex];
            }
            else
            {
                targetIndex += 1;
                currentLockTargetSingle = targetLocked[targetIndex];
            }
        }
        else
        {
            currentLockTargetSingle = null;
        }
    }
    private void FiringBullet()
    {
        var bullet = GameObject.Instantiate(Bullet, bulletSpawner.transform);
        if (currentLockTargetSingle != null)
        {
            bullet.GetComponent<Bullet>().ShootDirection(GetTargetLockedDirection());
        }
        else
        {
            bullet.GetComponent<Bullet>().ShootDirection(bulletSpawner.transform.forward);
        }
    }
    private void PerformedInput()
    {
        if (firingRecovery > 0)
        {
            firingRecovery -= Time.deltaTime;
        }
        else
        {
            if (Input.GetKey(KeyCode.F))
            {
                FiringBullet();
                firingRecovery = 0.2f;
            }
        }
        if (lunchingMissileRecovery > 0)
        {
            lunchingMissileRecovery -= Time.deltaTime;
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                Debug.Log("Lunching Missile");
                FiringMissile();
            }
        }
    }
    private void FiringMissile()
    {
        var Missile = GameObject.Instantiate(this.Missile, missileSpawner);
        if (currentLockTargetSingle != null)
        {
            Missile.GetComponent<Missile>().target = currentLockTargetSingle;
        }
        else
        {
            Missile.GetComponent<Missile>().target = null;
        }
    }
    private Vector3 GetTargetLockedDirection()
    {
        Vector3 Dir = (currentLockTargetSingle.transform.position - bulletSpawner.transform.position).normalized;
        return Dir;
    }

}
