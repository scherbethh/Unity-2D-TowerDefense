using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    public GameObject[] objectsToSpawn; // Spawn edilecek objelerin dizisi
    public float spawnInterval = 2f; // Spawn interval s�resi
    public float spawnRangeX = 10f; // X ekseninde spawn aral���
    public float spawnRangeY = 5f; // Y ekseninde spawn aral���

    private void Start()
    {
        // Belirli aral�klarla spawn i�lemini ba�lat�r
        InvokeRepeating("SpawnObject", 0f, spawnInterval);
    }

    void SpawnObject()
    {
        // Rastgele bir pozisyon belirler
        float randomX = Random.Range(-spawnRangeX / 2, spawnRangeX / 2);
        float randomY = Random.Range(-spawnRangeY / 2, spawnRangeY / 2);
        Vector3 randomPosition = new Vector3(randomX, randomY, 0);

        // Rastgele bir obje se�er
        int randomIndex = Random.Range(0, objectsToSpawn.Length);
        GameObject randomObject = objectsToSpawn[randomIndex];

        // Objeyi belirtilen pozisyonda spawn eder
        Instantiate(randomObject, randomPosition, Quaternion.identity);
    }
}
