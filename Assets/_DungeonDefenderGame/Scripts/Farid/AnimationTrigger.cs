using UnityEngine;
using UnityEngine.Events;

public class AnimationTrigger : MonoBehaviour
{
    [SerializeField] private UnityEvent animationTrigger;

    public void PlayTriggerMethods() => animationTrigger?.Invoke();
}