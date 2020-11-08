using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCharacterAnimation : MonoBehaviour
{
    public CharacterAnimationState characterAnimationState;
    public KeyCode success = KeyCode.Q;
    public KeyCode fail = KeyCode.E;

    private void OnValidate()
    {
        if (!characterAnimationState) characterAnimationState = GetComponent<CharacterAnimationState>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(success))
            characterAnimationState.triggerSuccess = true;
        if (Input.GetKeyDown(fail))
            characterAnimationState.triggerFail = true;
    }
}
