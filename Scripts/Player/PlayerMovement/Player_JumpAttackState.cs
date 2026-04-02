using UnityEngine;

public class Player_JumpAttackState : PlayerState
{
    private bool isTouchedGround;
    public Player_JumpAttackState(Player player, StateMachine stateMachine, string stateName) : base(player, stateMachine, stateName)
    {
        
    }

    public override void Enter()
    {
        base.Enter();
        isTouchedGround = false;
        
        player.SetVelocity(player.jumpAttackVelocity.x * player.facingDirection, player.jumpAttackVelocity.y);
    }

    public override void Update()
    {
        base.Update();

        if (player.isGround && isTouchedGround == false)
        {
            isTouchedGround = true;
            anim.SetTrigger("jumpAttackTrigger");
            player.SetVelocity(0,rb.linearVelocity.y);
        }

        if (triggerCalled && player.isGround)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
