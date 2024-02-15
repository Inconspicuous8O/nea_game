using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthScript : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public HealthBar healthbar;



    void Start()
    {
        currentHealth = maxHealth;
        healthbar.SetMaxHealth(maxHealth);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthbar.SetHealth(currentHealth);

        if (currentHealth <= 0 && gameObject.tag != "Enemy" && gameObject.tag != "Troop")
        {
            DestroyBuilding();
        }
        else if(gameObject.tag == "Enemy" && currentHealth <=0)
        {
            DestroyEnemy();
        }
        else if(gameObject.tag == "Troop" && currentHealth <=0)
        {
            DestroyTroop();
        }

    }

    public void DestroyTroop()
    {
        Debug.Log("Destroying troop");
        ResourcesScript.currentGold -= 50;
        ResourcesScript.goldPerSec += 5;
        Destroy(transform.parent.gameObject);

        TroopsPossessing troopsPossessingScript = FindObjectOfType<TroopsPossessing>();
        troopsPossessingScript.ExitPossession();
    }

    public void DestroyEnemy()
    {
        EnemyMovement movementScript = gameObject.GetComponent<EnemyMovement>();

        ResourcesScript.currentGold += movementScript.goldWhenDefeated;
        ResourcesScript.currentElixir += movementScript.elixirWhenDefeated;

        Destroy(gameObject);

    }

    public void DestroyBuilding()
    {
        BuildingSystem buildingSystemInstance = FindObjectOfType<BuildingSystem>();     
        buildingSystemInstance.RemoveObject(gameObject);
    }



    public void Update()
    {

    }
}
