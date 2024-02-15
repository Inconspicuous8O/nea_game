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
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);

        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDistance <= range && shortestDistance >= blindSpot)
        {
            target = nearestEnemy.transform;
        }
        else
        {
            target = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null){
            return;
        }

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



        if (fireCountdown <= 0)
        {
            Shoot();
            fireCountdown = 1f/fireRate;
        }

        fireCountdown -= Time.deltaTime;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position,range);
        Gizmos.DrawWireSphere(transform.position,blindSpot);
    }

    void Shoot()
    {
        GameObject bulletGo = (GameObject)Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Projectile_motion bullet = bulletGo.GetComponent<Projectile_motion>();

        if (bullet != null)
        {
            bullet.Seek(target);
        }
    }
}
