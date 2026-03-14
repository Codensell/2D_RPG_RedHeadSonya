using UnityEngine;

public class Player_BasicAttackState : EntityState
{
    private float attackVelocityTimer;
    
    private const int FirstComboIndex = 1;
    private int comboIndex = 1;
    private int comboLimit = 3;
    private float lastTimeAttacked;
    private bool isComboAttackQueued;
    private int attackDirection;
    
    public Player_BasicAttackState(Player player, StateMachine stateMachine, string stateName) : base(player, stateMachine, stateName)
    {
        if (comboLimit != player.attackVelocity.Length)
        {
            comboLimit = player.attackVelocity.Length;
        }
    }

    public override void Enter()
    {
        base.Enter();
		
		isComboAttackQueued = false;
        ResetComboIndexIfNeeded();

        attackDirection = player.moveInput.x != 0 ? ((int)player.moveInput.x) : player.facingDirection;
        
        anim.SetInteger("basicAttackIndex", comboIndex);
        ApplyAttackVelocity();
    }
    
    public override void Update()
    {
        base.Update();
        HandleAttackVelocity();

        if (input.Player.Attack.WasPressedThisFrame())
        {
            QueueNextAttack();
        }
        if (triggerCalled)
        {
            HandleStateExit();
        }
    }
    
    public override void Exit()
    {
        base.Exit();
        
        comboIndex++;
        lastTimeAttacked = Time.time;
    }

    private void HandleStateExit()
    {
        if (isComboAttackQueued)
        {
            anim.SetBool(animBoolName, false);
            player.EnterAttackStateWithDelay();
        }
        else
        {
            stateMachine.ChangeState(player.idleState);
        }
    }

	private void QueueNextAttack()
	{
		if(comboIndex < comboLimit)
		{
			isComboAttackQueued = true;
		}
	}

    private void HandleAttackVelocity()
    {
        attackVelocityTimer -= Time.deltaTime;

        if (attackVelocityTimer < 0)
        {
            player.SetVelocity(0, rb.linearVelocity.y);
        }
    }

    private void ApplyAttackVelocity()
    {
        Vector2 attackVelocity = player.attackVelocity[comboIndex - 1];
        
        attackVelocityTimer = player.attackVelocityDuration;
        player.SetVelocity(attackVelocity.x * attackDirection, attackVelocity.y);
    }
    
    private void ResetComboIndexIfNeeded()
    {
        if (comboIndex > comboLimit || Time.time > lastTimeAttacked + player.comboresetTime)
        {
            comboIndex = FirstComboIndex;
        }
    }
}
