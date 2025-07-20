using UnityEngine;

public class QuitGame : MonoBehaviour
{

    public void Quit()
    {
        Debug.Log("Quitting game...");
        Application.Quit();

#if UNITY_EDITOR

        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
