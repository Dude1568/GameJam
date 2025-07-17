using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    private Animator anim;
    private EnemyStateController stateController;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        stateController = GetComponent<EnemyStateController>();
        stateController.OnStateChanged.AddListener(UpdateAnimation);
    }

    private void UpdateAnimation(EnemyState state)
    {
        anim.SetBool("IsWalking", state == EnemyState.WALKING);
        anim.SetBool("IsAttacking", state == EnemyState.ATTACKING);

        if (state == EnemyState.DEAD)
            anim.SetTrigger("Die");
    }
}