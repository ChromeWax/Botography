using UnityEngine;

public class PlayerUnderwaterWalkState : PlayerBaseState
{
    public PlayerUnderwaterWalkState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base (currentContext, playerStateFactory){}

    public override void EnterState(){}

    public override void UpdateState()
    {
        Vector2 walkMovement = Ctx.MovementInput.normalized * Ctx.WalkSpeed;

        Ctx.CharacterAnimator.SetFloat(Ctx.HorizontalHash, walkMovement.x);
        Ctx.CharacterAnimator.SetFloat(Ctx.VerticalHash, walkMovement.y);
        Ctx.CharacterAnimator.SetFloat(Ctx.SpeedHash, walkMovement.magnitude);

        if (walkMovement.magnitude > 0)
        {
            Ctx.CharacterAnimator.SetFloat(Ctx.IdleHorizontalHash, walkMovement.x);
            Ctx.CharacterAnimator.SetFloat(Ctx.IdleVerticalHash, walkMovement.y);
        }

        Ctx.Movement = new Vector2(walkMovement.x, walkMovement.y);
        SoundManager.Instance.PlayFootsteps();

        CheckSwitchStates();
    }

    public override void ExitState(){}

    public override void InitializeSubState(){}

    public override void CheckSwitchStates()
    {
        if (!Ctx.MovementDetected)
            SwitchState(Factory.UnderwaterIdle());
    }
}

