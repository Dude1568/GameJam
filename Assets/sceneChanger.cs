using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{

    
    public void GoToNextSceneOrReload()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int totalScenes = SceneManager.sceneCountInBuildSettings;

        if (currentIndex < totalScenes - 1)
        {
            // Load next scene
            SceneManager.LoadScene(currentIndex + 1);
        }
        else
        {
            // Reload current (last) scene
            SceneManager.LoadScene(currentIndex);
        }
    }
}
