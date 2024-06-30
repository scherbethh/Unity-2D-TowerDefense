using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RGB : MonoBehaviour
{
    private TextMeshProUGUI textMeshPro;
    private float hue;

    void Start()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
        hue = 0f;
    }

    void Update()
    {
        // HSV renk modelinde rengi de�i�tir
        hue += Time.deltaTime * 0.5f; // Renk ge�i� h�z� (iste�e g�re ayarlanabilir)
        if (hue > 1f) hue -= 1f;

        Color color = Color.HSVToRGB(hue, 1f, 1f);
        textMeshPro.color = color;
    }
}

