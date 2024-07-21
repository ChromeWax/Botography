using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipevineAnimation : StateMachineBehaviour
{
    private bool _cameFrom;
    private bool _cameTo;
    private PipevinePlant _pipe;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _pipe = animator.gameObject.GetComponent<PipevinePlant>();
		bool from = animator.GetBool("TravelFrom");
		bool to = animator.GetBool("TravelTo");
        _cameFrom = from;
        _cameTo = to;
		if (to)
		{
			animator.SetBool("TravelTo", false);
		}
		if (from)
		{
			animator.SetBool("TravelFrom", false);
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
        if (_cameFrom)
        {
			_pipe.BeginTeleport();
		}
        if (_cameTo)
        {
            _pipe.EndTeleport();
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
