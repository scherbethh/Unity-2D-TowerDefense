using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Audio : MonoBehaviour
{
    private static Audio instance = null;
    public AudioClip deathSound;
    public AudioClip clickSound; // Butonlara týklama sesi eklemek için AudioClip deðiþkeni

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Bu GameObject'i sahne deðiþimlerinde yok etme
        }
        else
        {
            Destroy(gameObject); // Eðer bu müzik çalýcýdan zaten bir tane varsa, yeni olaný yok et
        }
    }

    // Sahne yüklendiðinde
    void OnEnable()
    {
        // SceneManager'a sahne yüklendiðinde OnSceneLoaded metoduyla abone ol
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        // SceneManager'dan abonelikten çýk
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Sahne yüklendiðinde çaðrýlýr
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Butonlara týklama ses efekti eklemek için tüm butonlarý bul ve sesi ekleyin
        Button[] buttons = FindObjectsOfType<Button>();
        foreach (Button button in buttons)
        {
            // Butonlara týklama ses efekti eklemek için AddListener metodunu kullanýn
            button.onClick.AddListener(PlayButtonClickSound);
        }
    }

    // Butona týklandýðýnda sesi çalacak metod
    public void PlayButtonClickSound()
    {
        GetComponent<AudioSource>().PlayOneShot(clickSound); // Buton týklandýðýnda clickSound'u çal
    }
    public void PlayDeadSound()
    {
        GetComponent<AudioSource>().PlayOneShot(deathSound); // DeadAnim metodu çaðrýldýðýnda deathSound'u çal
    }
}
