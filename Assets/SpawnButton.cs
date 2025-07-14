using UnityEngine;

public class SpawnButton : MonoBehaviour
{
    public GameObject prefabToSpawn;

    public void OnClickSpawn()
    {
        // You can integrate your placement system here
        GameObject spawned = Instantiate(prefabToSpawn);
        Debug.Log("Spawned " + spawned.name);
        // e.g., set mode to placement mode
    }
}