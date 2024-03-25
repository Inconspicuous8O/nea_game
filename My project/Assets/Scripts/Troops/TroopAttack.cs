using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TroopAttack : MonoBehaviour
{
    public float range;
    public int damage;

    public float fireRate = 1f;
    private float fireCountdown = 0f;

    public GameObject bulletPrefab;
    public Transform firePoint;


    public void Update()
    {
        if (Input.GetMouseButtonDown(0)) /// if left click was inputted
        {
            if (fireCountdown <= 0) /// if ready to attack
            {
                TroopAttackFunction(); /// call attack function
                fireCountdown = 1f/fireRate; /// reset countdown
            }
        }
        fireCountdown -= Time.deltaTime; /// reduce countdown
    }

    public void TroopAttackFunction()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); /// shoot a raycast from camera
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit)) /// if raycast hit something
        {
            if (hit.collider.gameObject.tag == "Enemy") /// if that something is an enemy
            {
                /// calculate distance to troop from enemy
                float distanceToEnemy = Vector3.Distance(transform.position, hit.transform.position);

                if (distanceToEnemy <= range) /// if enemy is within range
                {
                    if(bulletPrefab == null && firePoint == null) /// if there is no prefabs
                    {
                        BarbAttack(hit.collider.transform.parent.gameObject); /// barbarian attack
                    }
                    else
                    {
                        ShootArrow(hit.collider.transform.parent.gameObject); /// archer attack
                    }
                }
                else
                {
                    Debug.Log(distanceToEnemy);
                }
            }
        }
    }

    void BarbAttack(GameObject obj)
    {
        HealthScript health = obj.GetComponent<HealthScript>(); /// grab enemy health component
        health.TakeDamage(damage); /// deal damage
    }


    void ShootArrow(GameObject obj)
    {
        /// create object
        GameObject arrowShot = (GameObject)Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Projectile_motion arrow = arrowShot.GetComponent<Projectile_motion>(); /// gets component

        if (arrow != null) /// ensures that arrow exists
        {
            arrow.Seek(obj.transform); /// continue tracking target 
        }
    } 
}


