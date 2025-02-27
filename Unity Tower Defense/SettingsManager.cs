using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsManager : MonoBehaviour
{
    private bool isSettingsMenuOpen = false;
    public GameObject settingsMenu;

    public void ToggleSettingsMenu()
    {
        isSettingsMenuOpen = !isSettingsMenuOpen;
        settingsMenu.SetActive(isSettingsMenuOpen);
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.activeSceneChanged += OnSceneChanged;
    }

    private void OnSceneChanged(Scene currentScene, Scene nextScene)
    {
        // Yeni sahneye geçildiğinde, ayar menüsünün durumunu kontrol edin
        settingsMenu.SetActive(isSettingsMenuOpen);
    }
}
