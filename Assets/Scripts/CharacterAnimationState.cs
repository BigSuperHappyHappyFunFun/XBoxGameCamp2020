using UnityEngine;
using UnityEngine.Events;

public class CharacterAnimationState : MonoBehaviour
{
    public Animator animator;
    public float crossFadeTime = 0.5f;

    public int numberOfSuccesses = 3;
    public int numberOfFailures = 2;

    public int successIndex = 0;
    public int failureIndex = 0;

    public UnityEvent succeeded;
    public UnityEvent failed;

    private void OnValidate()
    {
        if (!animator) animator = GetComponent<Animator>();
    }

    public void PlaySuccess()
    {
        AnimatorCrossFade("Success");
        succeeded.Invoke();
    }

    public void PlayFailure()
    {
        AnimatorCrossFade("Failure");
        failed.Invoke();
    }

    public void PlaySuccessWrap()
    {
        var animationName = $"Success {successIndex}";
        AnimatorCrossFade(animationName);
        successIndex = (successIndex + 1) % numberOfSuccesses;
        succeeded.Invoke();
    }

    public void PlayFailureWrap()
    {
        var animationName = $"Failure {failureIndex}";
        AnimatorCrossFade(animationName);
        failureIndex = (failureIndex + 1) % numberOfFailures;
        failed.Invoke();
    }

    private void AnimatorCrossFade(string animationName)
    {
        animator.CrossFade(animationName, crossFadeTime, 0, 0);
    }
}
