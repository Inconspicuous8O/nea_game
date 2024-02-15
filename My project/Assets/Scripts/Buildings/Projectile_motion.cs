using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_motion : MonoBehaviour
{
    private Transform target;

    public float speed = 70f;
    public int damage = 1;

    public void Seek(Transform _target)
    {
        target = _target;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            Debug.Log("no target");
            Destroy(gameObject);
            return;
        }

        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);

    }

    void HitTarget()
    {
        Destroy(gameObject);
        
        HealthScript health = target.GetComponent<HealthScript>();
        health.TakeDamage(damage);

    }
}
