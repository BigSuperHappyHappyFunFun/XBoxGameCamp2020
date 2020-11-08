using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationState : MonoBehaviour
{
    public Animator animator;
    public float crossFadeTime = 0.5f;

    public bool triggerSuccess = false;
    public bool triggerFail = false;

    private void OnValidate()
    {
        if (!animator) animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (triggerSuccess)
        {
            TriggerSuccess();
            triggerSuccess = false;
        }
        if (triggerFail)
        {
            TriggerFail();
            triggerFail = false;
        }
    }

    public void TriggerSuccess()
    {
        animator.CrossFade("Success", crossFadeTime, 0, 0);
    }

    public void TriggerFail()
    {
        animator.CrossFade("Fail", crossFadeTime, 0, 0);
    }
}
