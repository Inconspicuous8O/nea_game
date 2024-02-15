using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarbarianAttack : MonoBehaviour
{
    public float range = 5f;
    public int damage = 10;
    public float cooldownTime = 1f;

    private float cooldownTimer = 0f;

    void Update()
    {
        cooldownTimer -= Time.deltaTime;

        if (cooldownTimer <= 0f)
        {
            ExecuteAoEAttack();

            cooldownTimer = cooldownTime;
        }
    }

    void ExecuteAoEAttack()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, range);

        foreach (Collider collider in hitColliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                HealthScript healthScript = collider.transform.parent.GetComponent<HealthScript>();
                if (healthScript != null)
                {
                    healthScript.TakeDamage(damage);
                }
            }
        }
    }
}
