using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] DifficultyMultiplierSlider difficultyMultiplierSlider;

    public void LoadScene(int levelIndex)
    {
        difficultyMultiplierSlider.OnValueChanged();
        SceneManager.LoadScene(levelIndex);
    }
}
