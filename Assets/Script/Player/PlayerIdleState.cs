using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base (currentContext, playerStateFactory){}

    public override void EnterState()
    {
        Ctx.CharacterAnimator.SetFloat(Ctx.HorizontalHash, 0f);
        Ctx.CharacterAnimator.SetFloat(Ctx.VerticalHash, 0f);
        Ctx.Movement = Vector2.zero;
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    public override void ExitState(){}

    public override void InitializeSubState(){}

    public override void CheckSwitchStates()
    {
        if (Ctx.MovementDetected)
            SwitchState(Factory.Walk());
    }
}
