using UnityEngine;
public class Enemy_DeadState : EnemyState
{
    private Collider2D colliderEnemy;
    public Enemy_DeadState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
        colliderEnemy = enemy.GetComponent<Collider2D>();
    }

    public override void Enter()
    {
        anim.enabled = false;
        colliderEnemy.enabled = false;
        
        rb.gravityScale = 12;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 15);
        
        Object.Destroy(enemy.gameObject, 3f);
        
        stateMachine.SwitchStateMachine();

    }
}
