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
        currentHealth = maxHealth; /// sets current health to the max health possible
        healthbar.SetMaxHealth(maxHealth); /// changes the slider
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage; /// Change health
        healthbar.SetHealth(currentHealth); /// Change slider value

        /// calls correct function for when an object health is 0
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
        ResourcesScript.currentGold -= 50; /// consequence of losing the troop
        ResourcesScript.goldPerSec += 5; /// makes so that troop income is not still happening
        Destroy(transform.parent.gameObject); /// destroy object

        /// grabs script
        TroopsPossessing troopsPossessingScript = FindObjectOfType<TroopsPossessing>();
        troopsPossessingScript.ExitPossession(); /// exit troop pov
    }

    public void DestroyEnemy()
    {
        /// getting relevant script
        EnemyMovement movementScript = gameObject.GetComponent<EnemyMovement>();

        /// add reward for defeating enemies
        ResourcesScript.currentGold += movementScript.goldWhenDefeated;
        ResourcesScript.currentElixir += movementScript.elixirWhenDefeated;

        Points.add_points(movementScript.pointsWhenDefeated); /// adding points
        Destroy(gameObject); /// destroy object

    }

    public void DestroyBuilding()
    {
        BuildingSystem buildingSystemInstance = FindObjectOfType<BuildingSystem>(); /// gets instance
        buildingSystemInstance.RemoveObject(gameObject); /// calls function to remove building

        if (gameObject.tag == "Town Hall")
        {
            GameOver.ChangeScene(); /// change scene if town hall has been destroyed
        }
    }
}
