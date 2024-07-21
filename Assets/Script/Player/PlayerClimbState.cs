using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClimbState : PlayerBaseState, IRootState
{
    public PlayerClimbState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base (currentContext, playerStateFactory)
    {
        IsRootState = true;
    }

    public override void EnterState()
    {
        Ctx.CharacterAnimator.SetBool(Ctx.GroundedHash, false);
        InitializeSubState();
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    public override void ExitState()
    {
        Ctx.CharacterAnimator.SetBool(Ctx.GroundedHash, true);
    }

    public override void InitializeSubState(){}

    public override void CheckSwitchStates()
    {
        if (!Ctx.isClimbing)
            SwitchState(Factory.Grounded());
    }
}