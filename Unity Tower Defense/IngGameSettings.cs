using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngGameSettings : MonoBehaviour
{
    public GameObject inGameSettingsObject;

    public void ToggleInGameSettings()
    {
        inGameSettingsObject.SetActive(!inGameSettingsObject.activeSelf);
    }
}
