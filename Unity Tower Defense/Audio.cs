using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Audio : MonoBehaviour
{
    private static Audio instance = null;
    public AudioClip deathSound;
    public AudioClip clickSound; // Butonlara t�klama sesi eklemek i�in AudioClip de�i�keni

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Bu GameObject'i sahne de�i�imlerinde yok etme
        }
        else
        {
            Destroy(gameObject); // E�er bu m�zik �al�c�dan zaten bir tane varsa, yeni olan� yok et
        }
    }

    // Sahne y�klendi�inde
    void OnEnable()
    {
        // SceneManager'a sahne y�klendi�inde OnSceneLoaded metoduyla abone ol
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        // SceneManager'dan abonelikten ��k
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Sahne y�klendi�inde �a�r�l�r
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Butonlara t�klama ses efekti eklemek i�in t�m butonlar� bul ve sesi ekleyin
        Button[] buttons = FindObjectsOfType<Button>();
        foreach (Button button in buttons)
        {
            // Butonlara t�klama ses efekti eklemek i�in AddListener metodunu kullan�n
            button.onClick.AddListener(PlayButtonClickSound);
        }
    }

    // Butona t�kland���nda sesi �alacak metod
    public void PlayButtonClickSound()
    {
        GetComponent<AudioSource>().PlayOneShot(clickSound); // Buton t�kland���nda clickSound'u �al
    }
    public void PlayDeadSound()
    {
        GetComponent<AudioSource>().PlayOneShot(deathSound); // DeadAnim metodu �a�r�ld���nda deathSound'u �al
    }
}
