using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Spawner : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private GameObject[] enemyPrefabs;
     
    [Header("Attributes")]
    [SerializeField] private int baseEnemies = 15;
    [SerializeField] private float enemiesPerSec = 0.5f;
    [SerializeField] private float timeBetweenWaves = 5f;
    [SerializeField] private float diffucltyScalingFactor = 0.75f;


    private int currentWave = 1;
    private float timeSinceLastSpawn;
    private int enemiesAlive;
    private int enemiesLeftToSpawn;
    private bool isSpawning = false;

    [Header("Events")]
    public static UnityEvent onEnemyDestroy = new UnityEvent();
    private void Awake()
    {
        onEnemyDestroy.AddListener(EnemyDestroyed);
    }
    void Start() 
    {
        StartCoroutine(StartWave());
    }
    void Update()
    {
        if (!isSpawning) 
        return; // eðer spawn olan yoksa girme 
      
        timeSinceLastSpawn += Time.deltaTime;

        if(timeSinceLastSpawn >= (1f / enemiesPerSec) && enemiesLeftToSpawn > 0)
        {
            SpawnEnemy();
            enemiesLeftToSpawn--;
            enemiesAlive++;
            
            timeSinceLastSpawn = 0f; 
        }

        if(enemiesAlive == 0 && enemiesLeftToSpawn == 0) 
        {
            EndWave();
        }



    }

    private IEnumerator StartWave()
    {
        yield return new WaitForSeconds(timeBetweenWaves);
        isSpawning = true;
        enemiesLeftToSpawn = EnemiesPerWave();

    }
    private void EndWave()
    {
        isSpawning = false;
        timeSinceLastSpawn = 0f;
        currentWave++;
        StartCoroutine(StartWave());
    }
    private void EnemyDestroyed()
    {
        enemiesAlive--;
    }
    
    private int EnemiesPerWave()
    {
        return Mathf.RoundToInt(baseEnemies * MathF.Pow(currentWave, diffucltyScalingFactor));
    }

    private void SpawnEnemy()
    {
        GameObject prefabToSpawn = enemyPrefabs[4];
        Instantiate(prefabToSpawn, WaveManage.main.startPoint.position, Quaternion.identity);
        Debug.Log("spawn enem");

    }
    

    

    
   
}
