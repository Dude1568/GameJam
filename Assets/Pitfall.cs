using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.XR;
using UnityEngine;
using UnityEngine.AI;

public class Pitfall : Trap
{
    [Header("Pittfall feedback related:")]
    [SerializeField] float fallDuration = 2f;
    [SerializeField] float fallRotation = 15f;
    [SerializeField,Range(0f,1f)] float finalScale = 0.2f;
    [SerializeField] Sprite triggeredSprite;
    [SerializeField] Gradient fallGradient;
    [SerializeField] AnimationCurve fallPositionCurve;
    [SerializeField] AnimationCurve fallScaleCurve;
    [SerializeField] ParticleSystem feedbackPS;
    [SerializeField] Vector2 targetOffset = new Vector2(0f, -0.1f);
    SpriteRenderer spriteRenderer;
    bool gotTriggered;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    //private void OnTriggerStay2D(Collider2D other)
    //{
    //    Debug.Log("Fall Famnit");
    //    if (other.CompareTag("Enemy"))
    //    {
    //        Vector2 enemyCenter = other.bounds.center;
    //        BoxCollider2D pitCollider = GetComponent<BoxCollider2D>();
    //        Debug.Log("Enemy center: " + enemyCenter);
    //        if (pitCollider.OverlapPoint(enemyCenter))
    //        {
    //            Debug.Log("Center is inside pit!");
    //            Trigger(other.gameObject);
    //        }
    //        else
    //        {
    //            Debug.Log("Center is NOT inside pit.");
    //        }
    //    }
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!gotTriggered && collision.CompareTag("Enemy"))
        {
            Trigger(collision.gameObject);
            gotTriggered = true;
        }
    }

    public override void Trigger(GameObject target)
    {
        var enemy = target;
        if (enemy != null)
        {
            NavMeshAgent agent= enemy.GetComponent<NavMeshAgent>();
            agent.enabled=false;
             ShrinkEnemy(enemy);
        }

        // Play trap effects.
        spriteRenderer.sprite = triggeredSprite;
        //ToDO: Trigger sound.
        if(feedbackPS != null) { Instantiate(feedbackPS, transform.position, Quaternion.identity); }
    }
    public void ShrinkEnemy(GameObject enemy) => StartCoroutine(ShrinkOverTime(enemy.transform));

    IEnumerator ShrinkOverTime(Transform target)    // think we may need to check if the target got null or disable the enemy script.
    {
        Vector3 startScale = target.localScale;
        Vector3 endScale = startScale * finalScale; // 🔥 relative shrink
        Vector2 targetStartPosition = target.position;
        Vector2 finalPosition = (Vector2)transform.position + targetOffset;
        SpriteRenderer targetRenderer = target.GetComponent<SpriteRenderer>();
        float time = 0f;

        while (time < fallDuration)
        {
            time += Time.deltaTime;
            float t = time / fallDuration;
            target.localScale = Vector3.Lerp(startScale, endScale, fallScaleCurve.Evaluate(t));

            target.position = Vector2.Lerp(targetStartPosition, finalPosition, fallPositionCurve.Evaluate(t));
            targetRenderer.color = fallGradient.Evaluate(t);
            target.Rotate(0f,0f,fallRotation * Time.deltaTime * t);

            yield return null;
        }

        Destroy(target.gameObject);
        gotTriggered = false; // ToDO: removelater.
    }

}
