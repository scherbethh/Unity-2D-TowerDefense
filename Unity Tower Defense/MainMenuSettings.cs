using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MainMenuSettings : MonoBehaviour
{
    public GameObject mainMenuSettingsObject;

   
    public void ToggleMainMenuSettings()
    {
        Debug.Log("Settings clicked");
        mainMenuSettingsObject.SetActive(!mainMenuSettingsObject.activeSelf);
    }
}
