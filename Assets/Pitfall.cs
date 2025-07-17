using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Pitfall : Trap
{
    private void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("Fall Famnit");
        if (other.CompareTag("Enemy"))
        {
            Vector2 enemyCenter = other.bounds.center;
            BoxCollider2D pitCollider = GetComponent<BoxCollider2D>();
            Debug.Log("Enemy center: " + enemyCenter);
            if (pitCollider.OverlapPoint(enemyCenter))
            {
                Debug.Log("Center is inside pit!");
                Trigger(other.gameObject);
            }
            else
            {
                Debug.Log("Center is NOT inside pit.");
            }
        }
    }

    public override void Trigger(GameObject target)
    {


        var enemy = target;
        if (enemy != null)
        {
            NavMeshAgent agent= enemy.GetComponent<NavMeshAgent>();
            agent.enabled=false;
             ShrinkEnemy(enemy, 2f);
            Destroy(enemy,2f);
        }
       

        
    }
        public void ShrinkEnemy(GameObject enemy, float duration)
    {
        StartCoroutine(ShrinkOverTime(enemy.transform, duration));
    }

    IEnumerator ShrinkOverTime(Transform target, float duration)
    {
        Vector3 startScale = target.localScale;
        Vector3 endScale = startScale * 0.2f; // 🔥 relative shrink
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            target.localScale = Vector3.Lerp(startScale, endScale, t);
            yield return null;
        }

        target.localScale = endScale;
    }

}
