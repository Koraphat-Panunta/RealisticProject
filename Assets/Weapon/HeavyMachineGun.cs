using System.Collections;
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
    private void Start()
    {
        _weaponView = new FieldOfView(400, 137, gameObject.transform);
    }
    private void Update()
    {
        PerformedInput();
        targetLocked = FindTarget();
       
        if (targetLocked.Count > 0)
        {
            if(currentLockTargetSingle == null)
            {
                currentLockTargetSingle = targetLocked[0];
            }
            CheckTargetDestroy();
        }
        else
        {
            currentLockTargetSingle = null;
        }
        
    }
    private List<GameObject> FindTarget()
    {
        List<GameObject> target = new List<GameObject>();
        target = _weaponView.FindMutipleObjectInView(lockAbleMask);
        foreach (GameObject obj in target)
        {
            if(obj.TryGetComponent<BodyPart>(out BodyPart bodyPart))
            {
                Debug.Log("Get Enemy");
               if(bodyPart.GetEnemy().enemyStateManager._currentState == bodyPart.GetEnemy().enemyStateManager.enemyDead)
               {
                    target.Remove(obj);
               }
            }
        }
        return target;
    }
    private void SwitchTargetLocked()
    {
        if (targetLocked.Count > 0)
        {
            if (targetIndex+1 > targetLocked.Count - 1)
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
    private void CheckTargetDestroy()
    {
        if(currentLockTargetSingle.TryGetComponent<BodyPart>(out BodyPart bodyPart))
        {
            if (bodyPart.GetEnemy().GetHP()<=0)
            {
                SwitchTargetLocked();
            }
        }
    }
    private void FiringBullet()
    {
        var bullet = GameObject.Instantiate(Bullet,bulletSpawner.transform);
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
        if(lunchingMissileRecovery > 0)
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
        Vector3 Dir = (currentLockTargetSingle.transform.position-bulletSpawner.transform.position).normalized;
        return Dir;
    }
    
}
