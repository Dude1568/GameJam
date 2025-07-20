using UnityEngine;
using UnityEngine.AI;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject gameplayCanvasObject;
    [SerializeField] private GameObject pauseMenuObject;

    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }
    public static bool IsGamePaused { get; private set; }

    private void TogglePause()
    {
        isPaused = !isPaused;
        IsGamePaused = isPaused;

        Time.timeScale = isPaused ? 0f : 1f;

        gameplayCanvasObject.SetActive(!isPaused);
        pauseMenuObject.SetActive(isPaused);

        TogglePhysics(isPaused);
        ToggleAnimators(isPaused);
    }

    private void ToggleAnimators(bool pause)
    {
        foreach (Animator animator in FindObjectsOfType<Animator>())
        {
            animator.enabled = !pause;
        }
    }


    private void TogglePhysics(bool pause)
    {
        foreach (Rigidbody2D rb in FindObjectsOfType<Rigidbody2D>())
        {
            rb.simulated = !pause;
        }

        foreach (NavMeshAgent agent in FindObjectsOfType<NavMeshAgent>())
        {
            agent.isStopped = pause;
        }
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        gameplayCanvasObject.SetActive(true);
        pauseMenuObject.SetActive(false);

        TogglePhysics(false);
    }
}
