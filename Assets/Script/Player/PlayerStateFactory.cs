using System.Collections.Generic;

public class PlayerStateFactory
{
    private PlayerStateMachine _context;
    Dictionary<PlayerStates, PlayerBaseState> _states = new Dictionary<PlayerStates, PlayerBaseState>();

    public PlayerStateFactory(PlayerStateMachine currentContext)
    {
        _context = currentContext;
        _states[PlayerStates.idle] = new PlayerIdleState(_context, this);
        _states[PlayerStates.walk] = new PlayerWalkState(_context, this);
        _states[PlayerStates.grounded] = new PlayerGroundedState(_context, this);
        _states[PlayerStates.climb] = new PlayerClimbState(_context, this);
		_states[PlayerStates.underwater] = new PlayerUnderwaterState(_context, this);
		_states[PlayerStates.underwaterIdle] = new PlayerUnderwaterIdleState(_context, this);
		_states[PlayerStates.underwaterWalk] = new PlayerUnderwaterWalkState(_context, this);
    }

    public PlayerBaseState Idle()
    {
        return _states[PlayerStates.idle];
    }

    public PlayerBaseState Walk()
    {
        return _states[PlayerStates.walk];
    }

    public PlayerBaseState Grounded()
    {
        return _states[PlayerStates.grounded];
    }

    public PlayerBaseState Climb()
    {
        return _states[PlayerStates.climb];
    }
	
	public PlayerBaseState Underwater()
	{
		return _states[PlayerStates.underwater];
	}

	public PlayerBaseState UnderwaterWalk()
	{
		return _states[PlayerStates.underwaterWalk];
	}

	public PlayerBaseState UnderwaterIdle()
	{
		return _states[PlayerStates.underwaterIdle];
	}
}
