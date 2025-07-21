using System.Collections;
using UnityEngine;

public class SpikeTrap : Trap
{
    public int damage = 10;

    [SerializeField] Sprite normalSprite;
    [SerializeField] Sprite triggeredSprite;
    [SerializeField] AudioClip triggeredClip;
    [SerializeField] float clipVolume = 100f;
    [SerializeField] float retriggerDuration = 1f;
    [SerializeField] ParticleSystem triggeredPS;
    SpriteRenderer spriteRenderer;
    bool gotTriggered;
    bool isFinished;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = normalSprite;
    }


    //private void OnTriggerEnter2D(Collider2D other)
    //{
    //    if (!gotTriggered && other.CompareTag("Enemy"))
    //    {
    //        Trigger(other.gameObject);
    //    }
    //}

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!gotTriggered && collision.CompareTag("Enemy"))
        {
            if (isFinished) { return; }
            Trigger(collision.gameObject);
        }
    }

    public override void Trigger(GameObject target)
    {   
        if(target.TryGetComponent(out EnemyHealth health))
        {
            health.TakeDamage(damage);
        }

        StartCoroutine(TriggerRoutine(target));
        Use(); // reduce usage count
    }

    IEnumerator TriggerRoutine(GameObject target)
    {
        gotTriggered = true;
        spriteRenderer.sprite = triggeredSprite;
        if(Soundmanager.instance != null)
            Soundmanager.instance.PlaySoundEffect(triggeredClip, transform, clipVolume);
        if(triggeredPS != null)
            Instantiate(triggeredPS,target.transform.position, Quaternion.identity);   

        yield return new WaitForSeconds(retriggerDuration);
        spriteRenderer.sprite = normalSprite;
        gotTriggered = false;
    }

    protected override void OnEmpty()
    {
        StartCoroutine(DestroyRoutine());
    }

    IEnumerator DestroyRoutine()
    {
        isFinished = true;
        yield return new WaitForSeconds(retriggerDuration + 1f);
        Destroy(gameObject);
    }
}