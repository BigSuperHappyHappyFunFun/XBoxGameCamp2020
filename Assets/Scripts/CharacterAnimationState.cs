using UnityEngine;
using UnityEngine.Events;

public class CharacterAnimationState : MonoBehaviour
{
    public Animator animator;
    public float crossFadeTime = 0.5f;
    public bool isComboSuccessThisFrame;

    private void OnValidate()
    {
        if (!animator) animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        GameEvents.PressedCorrect.Add(PlayNoteSuccess);
        GameEvents.FinishedCombo.Add(PlayComboSuccess);
        GameEvents.PressedWrong.Add(PlayPressedWrong);
        GameEvents.Missed.Add(PlayMissed);
    }

    private void OnDisable()
    {
        GameEvents.PressedCorrect?.Remove(PlayNoteSuccess);
        GameEvents.FinishedCombo?.Remove(PlayComboSuccess);
        GameEvents.PressedWrong?.Remove(PlayPressedWrong);
        GameEvents.Missed?.Remove(PlayMissed);
    }

    private void Update()
    {
        isComboSuccessThisFrame = false;
    }

    public void PlayNoteSuccess()
    {
        if (isComboSuccessThisFrame) return;
        AnimatorCrossFade("NoteSuccess");
    }

    public void PlayComboSuccess(int _ignore)
    {
        isComboSuccessThisFrame = true;
        AnimatorCrossFade("ComboSuccess");
    }

    public void PlayPressedWrong()
    {
        AnimatorCrossFade("PressedWrong");
    }

    public void PlayMissed()
    {
        AnimatorCrossFade("Missed");
    }

    private void AnimatorCrossFade(string animationName)
    {
        animator.CrossFade(animationName, crossFadeTime, 0, 0);
    }
}
