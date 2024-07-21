using UnityEngine;

public class PlayerGroundedState : PlayerBaseState, IRootState
{
    public PlayerGroundedState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base (currentContext, playerStateFactory)
    {
        IsRootState = true;
    }

    public override void EnterState()
    {
        Ctx.CharacterAnimator.SetBool(Ctx.GroundedHash, true);
        InitializeSubState();
    }

    public override void UpdateState()
    {
        Ctx.CharacterAnimator.SetBool(Ctx.GroundedHash, true);
        CheckSwitchStates();
    }

    public override void ExitState()
    {
        Ctx.CharacterAnimator.SetBool(Ctx.GroundedHash, false);
    }

    public override void InitializeSubState()
    {
        if (!Ctx.MovementDetected)
            SetSubState(Factory.Idle());
        else if (Ctx.MovementDetected)
            SetSubState(Factory.Walk());
    }

    public override void CheckSwitchStates()
    {
        if (Ctx.isClimbing)
            SwitchState(Factory.Climb());
        else if (Ctx.isUnderwater)
            SwitchState(Factory.Underwater());
    }
}
