using UnityEngine;

public class SpikeTrap : Trap
{
    public int damage = 10;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Trigger(other.gameObject);
        }
    }

    public override void Trigger(GameObject target)
    {
        if (!IsActive)
            return;
        var health = target.GetComponent<EnemyHealth>();
        if (health != null)
        {
            health.TakeDamage(damage);
        }

        Use(); // reduce usage count
    }
}