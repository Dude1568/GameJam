using UnityEngine;
using UnityEngine.Events;

public enum EnemyState { IDLE, WALKING, ATTACKING, DEAD }

public class EnemyStateController : MonoBehaviour
{
    [SerializeField] private EnemyState currentState = EnemyState.IDLE;
    public UnityEvent<EnemyState> OnStateChanged;

    public EnemyState CurrentState => currentState;

    public void SetState(EnemyState newState)
    {
        if (currentState != newState)
        {
            currentState = newState;
            OnStateChanged?.Invoke(currentState);
            Debug.Log($"State changed to {newState}");
        }
    }

    public bool IsDead => currentState == EnemyState.DEAD;
    public bool IsAttacking => currentState == EnemyState.ATTACKING;
    public bool IsWalking => currentState == EnemyState.WALKING;
}