using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTrap : Trap
{
    [SerializeField] List<ParticleSystem> burnParticleSystems;
    [SerializeField] Transform afterburnParticlePrefab;
    public int damage = 10;
    public int AfterburnDuration;
    public int TimeToTakeTheDamageFromAfterburn;
    public int AfterburnDamage;
    List<GameObject> currentTargets = new List<GameObject>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Trigger(other.gameObject);
        }
    }

    public override void Trigger(GameObject target)
    {
        if (!gameObject.activeSelf)
            return;
        if (!burnParticleSystems[0].isPlaying)
                foreach (ParticleSystem particleSystem in burnParticleSystems)
                {
                    particleSystem.Play();
                }
        var health = target.GetComponent<EnemyHealth>();
        if (health != null)
        {
            health.TakeDamage(damage);
            if (health != null)
            {
                StartCoroutine(StartAfterburnForTarget(target));
            }
        }

        Use(); // reduce usage count
    }

    public IEnumerator StartAfterburnForTarget(GameObject target)
    {
        if (currentTargets.Contains(target))
            yield return null;
        else
        {
            currentTargets.Add(target);
            Transform particle = Instantiate(afterburnParticlePrefab, target.transform);
            float timer = 0;
            while (timer < AfterburnDuration)
            {
                yield return new WaitForSeconds(TimeToTakeTheDamageFromAfterburn);
                timer += TimeToTakeTheDamageFromAfterburn;
                var health = target.GetComponent<EnemyHealth>();
                health.TakeDamage(AfterburnDamage);
            }
            currentTargets.Remove(target);
            Destroy(particle.gameObject);
        }
        
    }
}
