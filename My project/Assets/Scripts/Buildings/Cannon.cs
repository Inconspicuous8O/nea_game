using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    public Transform target;
    public float range = 15f;
    public float blindSpot = 5f;

    public string enemyTag = "Enemy";

    public Transform cannonBase;
    public Transform cannonBarrel;

    public float fireRate = 1f;
    private float fireCountdown = 0f;

    public GameObject bulletPrefab;
    public Transform firePoint;

    void Start()
    {
        InvokeRepeating("UpdateTarget",0f, 0.5f);
    }

    void UpdateTarget()
    {
        /// Creates an array will all the objects with the enemyTag tag 
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);

        /// sets initial shortest distance to infinity and nearest enemy as nothing
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        /// loop to calculate the distance from cannon to enemy for all the enemies
        foreach (GameObject enemy in enemies)
        {
            ///Calculate the distance
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            
            /// Decides if the distance is the shortest distance out of all enemies
            if (distanceToEnemy < shortestDistance && distanceToEnemy >= blindSpot)
            {
                ///Updates shortest distance and enemy object variable
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        ///Decides if enemy exists and is within range
        if (nearestEnemy != null && shortestDistance <= range && shortestDistance >= blindSpot)
        {
            /// Updates target
            target = nearestEnemy.transform;
        }
        else
        {
            target = null;
        }
    }

    void Update()
    {
        /// decides if target exists or not
        if (target == null)
        {
            return;
        }

        /// Code to change the direction of the turret
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation =  Quaternion.LookRotation(dir);
        Vector3 rotation = lookRotation.eulerAngles;

        if (cannonBase != null )
        {
            cannonBase.rotation = Quaternion.Euler(0f,rotation.y,0f);
            
        }
        if (cannonBarrel != null)
        {
            cannonBarrel.rotation = lookRotation;
        }

        /// if the cooldown is 0 then shoot
        if (fireCountdown <= 0)
        {
            Shoot(); /// shoot bullets
            fireCountdown = 1f/fireRate; /// rest countdown
        }

        /// otherwise reduce cooldown
        fireCountdown -= Time.deltaTime;
    }

    /// Function to visualise the range of the turret
    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position,range);
        Gizmos.DrawWireSphere(transform.position,blindSpot);
    }

    void Shoot()
    {
        /// instantiate the bullet a specified position 
        GameObject bulletGo = (GameObject)Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Projectile_motion bullet = bulletGo.GetComponent<Projectile_motion>();

        /// if bullet exists and has the component, then chase target
        if (bullet != null)
        {
            bullet.Seek(target);
        }
    }
}
