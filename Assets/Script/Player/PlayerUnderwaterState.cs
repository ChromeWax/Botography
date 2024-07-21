using Botography.Player;
using UnityEngine;

public class PlayerUnderwaterState : PlayerBaseState, IRootState
{
    public PlayerUnderwaterState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
	: base(currentContext, playerStateFactory)
	{
		IsRootState = true;
	}
	
	public override void EnterState()
	{
		PlayerManager.Instance.ChangeSpeed(-1.5f);
		Ctx.CharacterAnimator.SetBool(Ctx.GroundedHash, true);
		InitializeSubState();
	}
	
	public override void UpdateState()
	{
        CheckSwitchStates();
	}
	
	public override void ExitState()
	{
		PlayerManager.Instance.ChangeSpeed(1.5f);
		Ctx.CharacterAnimator.SetBool(Ctx.GroundedHash, false);
	}
	
	public override void InitializeSubState()
	{
		if (!Ctx.MovementDetected)
            SetSubState(Factory.UnderwaterIdle());
        else if (Ctx.MovementDetected)
            SetSubState(Factory.UnderwaterWalk());
	}
	
	public override void CheckSwitchStates()
	{
        if (!Ctx.isUnderwater)
            SwitchState(Factory.Grounded());
	}
}
