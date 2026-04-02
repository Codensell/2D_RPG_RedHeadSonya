using UnityEngine;

public class Player_CounterAttackState : PlayerState
{
    private PlayerCombat combat;
    private bool counteredSomething;
    public Player_CounterAttackState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
        combat = player.GetComponent<PlayerCombat>();
    }

    public override void Enter()
    {
        base.Enter();
        
        stateTimer = combat.GetCounterRecoveryDuration();
        counteredSomething = combat.CounterAttackPerformed();
        
        anim.SetBool("counterAttackPerformed", counteredSomething);
    }

    public override void Update()
    {
        base.Update();
        
        player.SetVelocity(0, rb.linearVelocity.y);
        
        if(triggerCalled)
            stateMachine.ChangeState(player.idleState);
        
        if(stateTimer < 0 && counteredSomething == false)
            stateMachine.ChangeState(player.idleState);
    }
}
