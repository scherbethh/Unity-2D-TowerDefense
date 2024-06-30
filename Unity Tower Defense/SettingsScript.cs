using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SettingsScript : MonoBehaviour
{
    public GameObject settingsImage;
    private AudioManager audioManager;

    public Button[] buttons;

   



    void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    public void ToggleSettings()
    {
        audioManager.PlaySFX(audioManager.click);

        Debug.Log("Settings clicked");
        
        settingsImage.gameObject.SetActive(!settingsImage.gameObject.activeSelf);
    }


    public void Restart()
    {
        audioManager.PlaySFX(audioManager.click);
        // GameController'� bul
        GameController gameController = FindObjectOfType<GameController>();
        if (gameController != null)
        {
            // Coin ve sa�l�k de�erlerini s�f�rla
            gameController.ResetCoin();
            gameController.ResetHealth();
        }

        // Oyun zaman �l�e�ini ayarla ve sahneyi yeniden y�kle
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    public void Exit()
    {
        Time.timeScale = 1f;
        audioManager.PlaySFX(audioManager.click);
        SceneManager.LoadScene("MainMenu");
        
    }

    public void LevelMenu()
    {
        Time.timeScale = 1f;
        audioManager.PlaySFX(audioManager.click);
        SceneManager.LoadScene("LevelMenu");
       
    }

    public void Level1()
    {
        Time.timeScale = 1f;
        audioManager.PlaySFX(audioManager.click);
        SceneManager.LoadScene("Level1");
    }

    public void Level2()
    {
        Time.timeScale = 1f;
        audioManager.PlaySFX(audioManager.click);
        SceneManager.LoadScene("Level2");
    }

    

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    

    
    

    
}
