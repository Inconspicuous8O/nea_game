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
        if (Input.GetMouseButtonDown(0))
        {
            if (fireCountdown <= 0)
            {
                TroopAttackFunction();
                fireCountdown = 1f/fireRate;
            }
        }
        fireCountdown -= Time.deltaTime;
    }

    public void TroopAttackFunction()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.tag == "Enemy")
            {
                float distanceToEnemy = Vector3.Distance(transform.position, hit.transform.position);

                if (distanceToEnemy <= range)
                {
                    if(bulletPrefab == null && firePoint == null)
                    {
                        BarbAttack(hit.collider.transform.parent.gameObject);
                    }
                    else
                    {
                        ShootArrow(hit.collider.transform.parent.gameObject);
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
        HealthScript health = obj.GetComponent<HealthScript>();
        health.TakeDamage(damage);
    }


    void ShootArrow(GameObject obj)
    {
        GameObject arrowShot = (GameObject)Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Projectile_motion arrow = arrowShot.GetComponent<Projectile_motion>();

        if (arrow != null)
        {
            arrow.Seek(obj.transform);
        }
    } 
}


