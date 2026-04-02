using UnityEngine;

public class Player_FallState : Player_AiredState
{
    public Player_FallState(Player player, StateMachine stateMachine, string stateName) : base(player, stateMachine, stateName)
    {
        
    }
    
    public override void Update()
    {
        base.Update();

        if (player.isGround)
        {
            stateMachine.ChangeState(player.idleState);
        }
        if(player.isWall)
        {
            stateMachine.ChangeState(player.wallSlideState);
        }
    }
}
