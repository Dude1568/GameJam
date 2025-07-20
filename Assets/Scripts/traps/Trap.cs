using UnityEngine;

public abstract class Trap : MonoBehaviour
{
    [Header("Trap Settings")]
    public int maxUses = 1;
    protected int currentUses;

    protected virtual void Start()
    {
        currentUses = maxUses;
    }

    public abstract void Trigger(GameObject target); // Called when an enemy steps into or activates the trap

    protected void Use()
    {
        currentUses--;
        if (currentUses <= 0)
        {
            OnEmpty();
        }
    }

    protected virtual void OnEmpty()
    {
        // Default behavior: destroy the trap
        Destroy(gameObject);
    }
}