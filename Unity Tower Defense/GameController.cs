using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;  // Oyun sahnesini yeniden y�klemek veya ana men�ye d�nmek i�in gerekli
using TMPro;

public class GameController : MonoBehaviour
{
    private SpawnerBTD spawner;

    [SerializeField] private TMP_Text coinText; 

    public static int coin = 200;
    public static int health = 20;

    public Image winImage;
    public Image loseImage;

    void Start()
    {
        spawner = FindObjectOfType<SpawnerBTD>();
        spawner.CalculateTotalEnemies();  // Toplam d��man say�s�n� hesapla ve g�ncelle
        Debug.Log("Ba�lang��ta kalan d��man say�s�: " + SpawnerBTD.totalEnemies);
    }

    private void FixedUpdate()
    {
        coinText.text = coinText.text = $"${coin}";

        if (EnemyMoveAI.currentHealth <= 0)
            LoseGame();


    }


    public void OnEnemyKilled()
    {
        SpawnerBTD.totalEnemies--;
        Debug.Log(SpawnerBTD.totalEnemies);
        coin += 5;

        if (SpawnerBTD.totalEnemies <= 0)
        {
            WinGame();
        }
    }

    
    public void WinGame()
    {
        Debug.Log("Tebrikler! Oyunu kazand�n�z.game controller");
        Time.timeScale = 0f;
        ResetHealth();
        ResetCoin();
        winImage.gameObject.SetActive(!winImage.gameObject.activeSelf);
        UnlockNewLevel();

    }
    void LoseGame()
    {
        Debug.Log("Kaybettiniz.game controller");
        Time.timeScale = 0f;
        ResetHealth();
        ResetCoin();
        loseImage.gameObject.SetActive(!loseImage.gameObject.activeSelf);

    }
    public void ResetCoin()
    {
        coin = 100;
    }
    public void ResetHealth()
    {
        health = 20;
        EnemyMoveAI.currentHealth = health; // currentHealth'i health'e e�itle
    }

    public void UnlockNewLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex >= PlayerPrefs.GetInt("ReachedIndex"))
        {
            PlayerPrefs.SetInt("ReachedIndex", SceneManager.GetActiveScene().buildIndex +1);
            PlayerPrefs.SetInt("UnlockedLevel", PlayerPrefs.GetInt("UnlockedLevel", 1) + 1);
            PlayerPrefs.Save();
        }
    }

}
