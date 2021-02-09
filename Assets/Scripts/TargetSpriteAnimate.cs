using UnityEngine;

public class TargetSpriteAnimate : MonoBehaviour
{
    public Animator animator;
    public float crossFadeTime = 0.1f;
    public string lastAnimatorState = "";

    private void OnValidate()
    {
        if (!animator) animator = GetComponent<Animator>();
    }

    public void SetNextUp() => PlayAnimation("Up");
    public void SetNextDown() => PlayAnimation("Down");
    public void SetNextLeft() => PlayAnimation("Left");
    public void SetNextRight() => PlayAnimation("Right");
    public void SetNextNone() => PlayAnimation("None");

    public void PlayAnimation(string stateName)
    {
        if (lastAnimatorState != stateName)
        {
            animator.CrossFade(stateName, crossFadeTime);
            lastAnimatorState = stateName;
        }
    }
}
