using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AdventurerItems : MonoBehaviour
{
    [SerializeField] List<Item> possibleItems;
    [SerializeField] int adventurerValue;
    [SerializeField] int maxItemDistance;

    public void SpawnItemsByCost()
    {
        List<Item> items = new List<Item>();
        while (items.Sum(i => i.Value) < adventurerValue)
        {
            List<Item> validItems = possibleItems.Where(i => i.Value <= adventurerValue).ToList();
            Item randomItem = validItems[Random.Range(0, validItems.Count)];
            items.Add(randomItem);
        }
        foreach (Item item in items)
        {
            Vector2 finalPos;
            float currentAngle = Random.Range(0, 2 * Mathf.PI);
            Vector2 direction = new Vector2(Mathf.Cos(currentAngle), Mathf.Sin(currentAngle));
            RaycastHit2D raycastHit;
            if (raycastHit = Physics2D.Raycast(transform.position, direction, maxItemDistance, 1 << 6))
            {
                finalPos = raycastHit.point;
            }
            else
            {
                finalPos = (Vector2)transform.position + direction * maxItemDistance;
            }
            Item newItem = Instantiate(item, finalPos, Quaternion.identity);
        }
    }
}
