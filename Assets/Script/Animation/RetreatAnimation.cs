using Botography.Player;
using Botography.Player.StatusEffects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Botography.Player.PlayerConstants;

public class RetreatAnimation : StateMachineBehaviour
{
    private bool _initialEntry;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
		foreach (KeyValuePair<StatusEffectType, string> animString in UNBEARABLE_ANIM_STRINGS)
		{
			if (animator.GetBool(animString.Value))
			{
                _initialEntry = true;
				animator.SetBool(animString.Value, false);
				break;
			}
		}
	}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_initialEntry)
        {
			StatusEffectsHandler.Instance.Respawn();
		}
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
