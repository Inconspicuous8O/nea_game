using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public Transform normal;
    public Transform wallBreaker;
    public Transform goblin;
    public Transform giant;

    public float chanceOfNothing = 0.2f;

    public float timeBetweenWaves = 180f;
    private float countdown = 180f;

    public int waveNumber = 1;

    void Update()
    {
        if (countdown <= 0f)
        {
            SpawnWave();
            countdown = timeBetweenWaves;
        }

        countdown -= Time.deltaTime;
    }

    public void SpawnWave()
    {   
        countdown = timeBetweenWaves;
        for(int i = 0; i < waveNumber; i++)
        {
            SpawnEnemy();
        }

        waveNumber++;
    }

    void SpawnEnemy()
    {
        float randInt = Random.value;

        if (randInt > chanceOfNothing)
        {
            Transform[] enemies = new Transform[] { normal, wallBreaker, goblin, giant };
            int randomIndex = Random.Range(0, enemies.Length);

            Transform randomEnemy = enemies[randomIndex];

            Instantiate(randomEnemy, transform.position, transform.rotation);
        }
        
    }

}
