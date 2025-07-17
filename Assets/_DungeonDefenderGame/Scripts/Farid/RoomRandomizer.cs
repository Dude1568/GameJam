using UnityEngine;

public class RoomRandomizer : MonoBehaviour
{
    [SerializeField] private Transform groundTransform;
    [SerializeField] private Transform groundObstacleTransform;
    [SerializeField] private Transform wallNorthTransform;
    [SerializeField] private Transform wallEastTransform;
    [SerializeField] private Transform wallWestTransform;

    // Start is called before the first frame update
    private void Start()
    {
        if (groundTransform != null)
            groundTransform.GetChild(Random.Range(0, groundTransform.childCount)).gameObject.SetActive(true);
        if (groundObstacleTransform != null)
            groundObstacleTransform.GetChild(Random.Range(0, groundObstacleTransform.childCount)).gameObject.SetActive(true);
        if (wallNorthTransform != null)
            wallNorthTransform.GetChild(Random.Range(0, wallNorthTransform.childCount)).gameObject.SetActive(true);
        if (wallEastTransform != null)
            wallEastTransform.GetChild(Random.Range(0, wallEastTransform.childCount)).gameObject.SetActive(true);
        if (wallWestTransform != null)
            wallWestTransform.GetChild(Random.Range(0, wallWestTransform.childCount)).gameObject.SetActive(true);
    }
}