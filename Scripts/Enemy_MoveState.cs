using UnityEngine;

public class Enemy_MoveState : EnemyState
{
    public Enemy_MoveState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        
        if (enemy.isGround == false || enemy.isWall)
        {
            stateMachine.ChangeState(enemy.idleState);
            enemy.Flip();
        }
    }

    public override void Update()
    {
        base.Update();
        
        enemy.SetVelocity(enemy.moveSpeed * enemy.facingDirection, rb.linearVelocity.y);

        if (enemy.isGround == false || enemy.isWall)
        {
            stateMachine.ChangeState(enemy.idleState);
        }
    }
}
