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
        InvokeRepeating("InBattleFunction",0f, 0.5f);
        InvokeRepeating("ChangeWave", 0f, 0.5f);
    }

    public void InBattleFunction()
    {
        BuildingSystem buildingSystemInstance = FindObjectOfType<BuildingSystem>();
        GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemyObject in enemyObjects)
        {
            if (enemyObject.activeSelf)
            {
                buildingSystemInstance.inBattle = true;
                return;
            }
        }
        
        buildingSystemInstance.inBattle = false;

    }

    public void ActivateSpawners()
    {
        WaveSpawner[] spawners = FindObjectsOfType<WaveSpawner>();

        foreach (WaveSpawner obj in spawners)
        {
            obj.SpawnWave();
        }
    }

    public void ChangeWave()
    {
        WaveSpawner waveSpawnerInstance = FindObjectOfType<WaveSpawner>();
        waveText.text = "Wave " + waveSpawnerInstance.waveNumber.ToString();
    }
    
}
