using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using TMPro.Examples;

public class SpawnEnemiesScript : MonoBehaviour
{
    public TextMeshProUGUI waveText; 

    public void Start()
    {
        /// start invoking functions
        InvokeRepeating("InBattleFunction",0f, 0.5f);
        InvokeRepeating("ChangeWave", 0f, 0.5f);
    }

    public void InBattleFunction()
    {
        /// retrieves building system instance
        BuildingSystem buildingSystemInstance = FindObjectOfType<BuildingSystem>();
        /// creates an array with all the game object with the tag
        GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemyObject in enemyObjects)
        {
            /// checks if there is an active enemy objects
            if (enemyObject.activeSelf)
            {
                buildingSystemInstance.inBattle = true; /// set inBattle to true
                return;
            }
        }
        
        buildingSystemInstance.inBattle = false; /// set inBattle to false

    }

    public void ActivateSpawners()
    {
        /// create an array containing all the spawn points
        WaveSpawner[] spawners = FindObjectsOfType<WaveSpawner>();

        /// iterate through the array
        foreach (WaveSpawner obj in spawners)
        {
            obj.SpawnWave(); /// activate function
        }
    }

    public void ChangeWave()
    {
        WaveSpawner waveSpawnerInstance = FindObjectOfType<WaveSpawner>(); /// grab instance
        waveText.text = "Wave " + waveSpawnerInstance.waveNumber.ToString(); /// change ui value
    }
    
}
