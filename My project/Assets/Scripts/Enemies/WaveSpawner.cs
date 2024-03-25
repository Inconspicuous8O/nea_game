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
            /// if countdown is less than or equals to 0 the spawn wave
            SpawnWave();
            countdown = timeBetweenWaves; /// reset countdown time
        }

        countdown -= Time.deltaTime; /// reduce countdown time
    }

    public void SpawnWave()
    {   
        countdown = timeBetweenWaves;
        for(int i = 0; i < waveNumber; i++)
        {
            /// iterate up to number of waves and spawn enemy
            SpawnEnemy();
        }

        waveNumber++; ///increment wave number
    }

    void SpawnEnemy()
    {
        float randInt = Random.value; /// create random value

        if (randInt > chanceOfNothing) /// if random number is large than chance of nothing value
        {
            Transform[] enemies = new Transform[] { normal, wallBreaker, goblin, giant }; /// create array of all the enemies
            int randomIndex = Random.Range(0, enemies.Length); /// generate another random number 

            Transform randomEnemy = enemies[randomIndex]; /// select chosen enemy from array

            Instantiate(randomEnemy, transform.position, transform.rotation); /// spawn enemy
        }
        
    }

}
