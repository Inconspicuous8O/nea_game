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
        cooldownTimer -= Time.deltaTime; /// reduce timer

        if (cooldownTimer <= 0f)
        {
            ExecuteAoEAttack(); /// activate AOE

            cooldownTimer = cooldownTime;/// reset timer
        }
    }

    void ExecuteAoEAttack()
    {
        /// gets all objects that are in the invisible sphere created
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, range);

        foreach (Collider collider in hitColliders) /// iterate through each object
        {
            if (collider.CompareTag("Enemy")) /// checks if it has the enemy tag
            {
                HealthScript healthScript = collider.transform.parent.GetComponent<HealthScript>(); /// grabs health component from object
                if (healthScript != null) /// makes sure the object is attackable
                {
                    healthScript.TakeDamage(damage); /// deal damage
                }
            }
        }
    }
}
