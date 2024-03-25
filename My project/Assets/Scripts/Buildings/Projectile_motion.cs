using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_motion : MonoBehaviour
{
    private Transform target;

    public float speed = 70f;
    public int damage = 1;

    /// Sets the target for bullet
    public void Seek(Transform _target)
    {
        target = _target;
    }

    void Update()
    {
        if (target == null)
        {
            /// If target doesn't exist, delete the bullet 
            Debug.Log("no target");
            Destroy(gameObject);
            return;
        }

        ///Figures out the directions/distance that needs to be moved in a frame
        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (dir.magnitude <= distanceThisFrame)
        {
            /// Calls bullet hit when distance is great enough
            HitTarget();
            return;
        }

        /// moves the bullet
        transform.Translate(dir.normalized * distanceThisFrame, Space.World);

    }

    void HitTarget()
    {
        Destroy(gameObject); /// destroy bullet
        
        HealthScript health = target.GetComponent<HealthScript>(); ///grab health script of object
        health.TakeDamage(damage); /// deal damage by calling function 

    }
}
