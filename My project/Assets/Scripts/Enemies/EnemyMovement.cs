using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Movement")]
    public Rigidbody rb;
    public Transform target;
    public float speed;
    
    public GameObject currentlyAttacking = null;

    [Header("Damage")]
    public int dmgAgainstTH;
    public int dmgAgainstResc;
    public int dmgAgainstDef;
    public int dmgAgainstWall;
    public int dmgAgainstTroops;

    [Header("When defeated")]
    public int goldWhenDefeated;
    public int elixirWhenDefeated;
    public int pointsWhenDefeated;

    private Dictionary<string, int> damages = new Dictionary<string, int>();

    [Header("Target")]
    public string targetTag;

    [Header("Boolean")]
    public bool invoked = false;
    public bool attacking = false;
    public bool chasingTroop = false;

    private float countdown = 0f;

    void Start()
    {
        rb.freezeRotation = true; /// stops the object from falling over
        InvokeRepeating("UpdateBuildingTarget",0f, 0.5f);
        invoked = true;

        DefineDic();
    }

    void DefineDic()
    {
        /// Inputting the values into the dictionary
        damages["Town Hall"] = dmgAgainstTH;
        damages["Resource Buildings"] = dmgAgainstResc;
        damages["Defence Buildings"] = dmgAgainstDef;
        damages["Wall"] = dmgAgainstWall;
        damages["Troop"] = dmgAgainstTroops;
    }

    public int GetDamageForBuilding(string building)
    {
        /// Grabs value from dictionary
        if (damages.TryGetValue(building, out int damage))
        {
            return damage; /// if key-value pair exists, return the value
        }
        else
        {
            return -1; /// else return -1
        }
    }

    void UpdateBuildingTarget()
    {
        if (target == null)
        {
            /// create an array with all objects with specified tag
            GameObject[] targets = GameObject.FindGameObjectsWithTag(targetTag);

            /// if array is empty
            if(GameObject.FindGameObjectsWithTag(targetTag).Length == 0)
            {
                targetTag = "Town Hall";
            }

            /// initial values are set
            float shortestDistance = Mathf.Infinity;
            GameObject nearestTarget = null;

            /// for loop to iterate through every target
            foreach (GameObject building in targets)
            {
                /// calculate the distance to object
                float distanceToTarget = Vector3.Distance(transform.position, building.transform.position);
                if (distanceToTarget < shortestDistance) /// if statement to decide if current object is the nearest 
                {
                    /// if it is, set shortest distance to one calculated and building to the corresponding one
                    shortestDistance = distanceToTarget;
                    nearestTarget = building;
                }
            }

            /// if statement to ensure the object was not deleted during entire process
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
        /// if no target exists
        if (target == null){
            /// change variables
            attacking = false;
            countdown = 0f;
            if(invoked == false)
            {
                /// restart the regular intervening calling of function
                InvokeRepeating("UpdateBuildingTarget",0f, 0.5f);
                invoked = true;
            }
            return;
        }

        if (!attacking)
        {
            /// if enemy is not stationary and attacking, chase target
            ChaseTarget();
        }
        else
        {
            /// if target is a troop and distance is less than 1 then chase
            if(Vector3.Distance(transform.position, target.position) >= 1f && target.tag == "Troop")
            {
                ChaseTarget();
            }
            else
            {  
                /// do damage if cooldown is less than or equals to 0 and reset cooldown
                if (countdown <= 0f)
                {
                    DoDamage();
                    countdown = 2f;
                }
            }

            /// reduce countdown
            countdown -= Time.deltaTime;

        }
        
    }

    void ChaseTarget()
    {
        /// orientates the object toward target
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation =  Quaternion.LookRotation(dir);
        transform.rotation = lookRotation;

        /// calculate distance to travel in this frame
        float distanceThisFrame = speed * Time.deltaTime;

        /// move object
        transform.Translate(dir.normalized * distanceThisFrame, Space.World);

    }

    void DoDamage()
    {
        HealthScript health = null; /// define health as null initially

        if(currentlyAttacking.tag == "Troop")
        {
            health = currentlyAttacking.GetComponentInChildren<HealthScript>(); /// grab health component in child if it is a troop
        }
        else
        {
            health = currentlyAttacking.GetComponent<HealthScript>(); /// grab health component
        }

        health.TakeDamage(damages[currentlyAttacking.tag]); /// call function to deal damage
        rb.velocity = Vector3.zero; /// set speed to 0
        
    }

    void OnCollisionEnter(Collision collision)
    {
        /// if dictionary has valid value for the type of object
        if (GetDamageForBuilding(collision.gameObject.tag) != -1)
        {
            /// change variables accordingly
            attacking = true;
            currentlyAttacking = collision.gameObject;
            target = currentlyAttacking.transform;

            CancelInvoke("UpdateBuildingTarget"); /// cancel invoking function
            invoked = false; /// change variables accordingly
        }
    }
}
