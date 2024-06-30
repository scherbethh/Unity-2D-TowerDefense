using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpawnerBTD : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private float countdown;
    [SerializeField] public Wave[] waves;
    [SerializeField] private Button startButton;
    [SerializeField] private TMP_Text waveText;

    private int currentWaveIndex = 0;
    private bool isSpawning = false;
    public static int totalEnemies = 0;

    private GameController gameController;
   

    void Start()
    {
        gameController = FindObjectOfType<GameController>();

        if (startButton != null)
        {
            startButton.onClick.AddListener(StartSpawning);
        }

        CalculateTotalEnemies();
        UpdateWaveText();
    }

    void Update()
    {
        if (!isSpawning) return;

        countdown -= Time.deltaTime;

        if (countdown < 0 && currentWaveIndex < waves.Length)
        {
            SpawnWave();
            countdown = waves[currentWaveIndex].waveInterval;
        }

        
       

    }

    public void StartSpawning()
    {
        isSpawning = true;
        countdown = 0;
        startButton.gameObject.SetActive(false);
    }

    private void SpawnWave()
    {
        Wave currentWave = waves[currentWaveIndex];
        StartCoroutine(SpawnEnemies(currentWave));
        currentWaveIndex++;
        UpdateWaveText();
    }

    private IEnumerator SpawnEnemies(Wave wave)
    {
        for (int i = 0; i < wave.enemyPrefabs.Length; i++)
        {
            for (int j = 0; j < wave.enemyCounts[i]; j++)
            {
                Instantiate(wave.enemyPrefabs[i], WaveManage.main.startPoint.position, Quaternion.identity);
                yield return new WaitForSeconds(1f / wave.spawnRate);
            }
        }
    }

    private void UpdateWaveText()
    {
        if (waveText != null)
        {
            waveText.text = $" {currentWaveIndex}/{waves.Length}";
        }
    }

    

    public void CalculateTotalEnemies()
    {
        totalEnemies = 0;
        foreach (Wave wave in waves)
        {
            foreach (int count in wave.enemyCounts)
            {
                totalEnemies += count;
            }
        }
    }

    [System.Serializable]
    public class Wave
    {
        public GameObject[] enemyPrefabs;
        public int[] enemyCounts;
        public float spawnRate;
        public float waveInterval;
    }
}
