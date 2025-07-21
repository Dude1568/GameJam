using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{


    public void GoToNextSceneOrReload()
    {
        EnemyBehaviorController.KEYFOUND = false;
        EnemyBehaviorController.KEYHOLDER = null;
        GridManager.treasuryPlaced = false;
        EnemyBehaviorController.GameOver = false;
        WaveSpawner.gameState = WaveSpawner.WaveSpawnerState.BUILDING;

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
