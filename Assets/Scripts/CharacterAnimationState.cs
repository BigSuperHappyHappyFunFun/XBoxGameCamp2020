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

    private void OnValidate()
    {
        if (!animator) animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        GameEvents.PressedCorrect.Add(PlaySuccessWrap);
        GameEvents.Failed.Add(PlayFailureWrap);
    }

    private void OnDisable()
    {
        GameEvents.PressedCorrect?.Remove(PlaySuccessWrap);
        GameEvents.Failed?.Remove(PlayFailureWrap);
    }

    public void PlaySuccess()
    {
        AnimatorCrossFade("Success");
    }

    public void PlayFailure()
    {
        AnimatorCrossFade("Failure");
    }

    public void PlaySuccessWrap()
    {
        var animationName = $"Success {successIndex}";
        AnimatorCrossFade(animationName);
        successIndex = (successIndex + 1) % numberOfSuccesses;
    }

    public void PlayFailureWrap()
    {
        var animationName = $"Failure {failureIndex}";
        AnimatorCrossFade(animationName);
        failureIndex = (failureIndex + 1) % numberOfFailures;
    }

    private void AnimatorCrossFade(string animationName)
    {
        animator.CrossFade(animationName, crossFadeTime, 0, 0);
    }
}
