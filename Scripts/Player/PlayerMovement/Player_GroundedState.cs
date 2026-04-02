using UnityEngine;

public class Player_GroundedState : PlayerState
{
    public Player_GroundedState(Player player, StateMachine stateMachine, string stateName) : base(player, stateMachine, stateName)
    {
        
    }
    public override void Update()
    {
        base.Update();

        if (rb.linearVelocity.y < 0 && player.isGround == false)
        {
            stateMachine.ChangeState(player.fallState);
        }

        if (input.Player.Jump.WasPressedThisFrame())
        {
            stateMachine.ChangeState(player.jumpState);
        }

        if (input.Player.Attack.WasPerformedThisFrame())
        {
            stateMachine.ChangeState(player.basicAttackState);
        }

        if (input.Player.CounterAttack.WasPerformedThisFrame())
        {
            stateMachine.ChangeState(player.counterAttackState);
        }
    }
}
