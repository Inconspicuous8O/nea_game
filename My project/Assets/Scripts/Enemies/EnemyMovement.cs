using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Rigidbody rb;
    public Transform target;

    public GameObject currentlyAttacking = null;

    public float speed;

    public int dmgAgainstTH;
    public int dmgAgainstResc;
    public int dmgAgainstDef;
    public int dmgAgainstWall;
    public int dmgAgainstTroops;

    public int goldWhenDefeated;
    public int elixirWhenDefeated;

    private Dictionary<string, int> damages = new Dictionary<string, int>();

    public string targetTag = "Enemy";

    public bool invoked = false;
    public bool attacking = false;
    public bool chasingTroop = false;

    private float countdown = 0f;

    void Start()
    {
        rb.freezeRotation = true;
        InvokeRepeating("UpdateBuildingTarget",0f, 0.5f);
        invoked = true;

        DefineDic();

    }

    void DefineDic()
    {
        damages["Town Hall"] = dmgAgainstTH;
        damages["Resource Buildings"] = dmgAgainstResc;
        damages["Defence Buildings"] = dmgAgainstDef;
        damages["Wall"] = dmgAgainstWall;
        damages["Troop"] = dmgAgainstTroops;
    }

    public int GetDamageForBuilding(string building)
    {
        if (damages.TryGetValue(building, out int damage))
        {
            return damage;
        }
        else
        {
            return -1;
        }
    }

    void UpdateBuildingTarget()
    {
        if (target == null)
        {
            GameObject[] targets = GameObject.FindGameObjectsWithTag(targetTag);

            if(GameObject.FindGameObjectsWithTag(targetTag).Length == 0)
            {
                targetTag = "Town Hall";
            }

            float shortestDistance = Mathf.Infinity;
            GameObject nearestTarget = null;

            foreach (GameObject building in targets)
            {
                float distanceToTarget = Vector3.Distance(transform.position, building.transform.position);
                if (distanceToTarget < shortestDistance)
                {
                    shortestDistance = distanceToTarget;
                    nearestTarget = building;
                }
            }

            if (nearestTarget != null)
            {
                target = nearestTarget.transform;
            }
            else
            {
                target = null;
            }
        }
    }
    
    void Update()
    {
        if (target == null){
            attacking = false;
            countdown = 0f;
            if(invoked == false)
            {
                InvokeRepeating("UpdateBuildingTarget",0f, 0.5f);
                invoked = true;
            }
            return;
        }

        if (!attacking)
        {
            ChaseTarget();
        }
        else
        {
            if(Vector3.Distance(transform.position, target.position) > 1f && target.tag == "Troop")
            {
                ChaseTarget();
            }
            else
            {  
                if (countdown <= 0f)
                {
                    DoDamage();
                    countdown = 2f;
                }
            }

            countdown -= Time.deltaTime;
        }
        
    }

    void ChaseTarget()
    {
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation =  Quaternion.LookRotation(dir);
        transform.rotation = lookRotation;

        float distanceThisFrame = speed * Time.deltaTime;

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);

    }

    void DoDamage()
    {
        if(currentlyAttacking.tag == "Troop")
        {
            HealthScript health = currentlyAttacking.GetComponentInChildren<HealthScript>();
            health.TakeDamage(damages[currentlyAttacking.tag]);
            rb.velocity = Vector3.zero;
        }
        else
        {
            HealthScript health = currentlyAttacking.GetComponent<HealthScript>();
            health.TakeDamage(damages[currentlyAttacking.tag]);
            rb.velocity = Vector3.zero;
        }
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (GetDamageForBuilding(collision.gameObject.tag) != -1)
        {
            attacking = true;
            currentlyAttacking = collision.gameObject;
            target = currentlyAttacking.transform;
            CancelInvoke("UpdateBuildingTarget");
            invoked = false;
        }
    }
}
