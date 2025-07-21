using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultyMultiplierSlider : MonoBehaviour
{
    [SerializeField] Slider slider;
    public void OnValueChanged()
    {
        PlayerPrefs.SetFloat("Difficulty", slider.value);
    }
}
