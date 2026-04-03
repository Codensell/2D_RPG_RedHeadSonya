using UnityEngine;
using System.Collections;
using System;

public class Entity : MonoBehaviour
{
    public event Action OnFlipped;
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public EntityStats stats { get; private set; }
    protected StateMachine stateMachine;

    private bool facingRight = true;
    public int facingDirection { get; private set; } = 1;

    [Header("Collision Detection")] [SerializeField]
    private float groundCheckDistance;

    [SerializeField] private float wallCheckDistance;
    [SerializeField] protected LayerMask whatIsGround;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform primaryWallCheck;
    [SerializeField] private Transform secondaryWallCheck;
    public bool isGround { get; private set; }
    public bool isWall { get; private set; }

    private bool _isKnocked;
    private Coroutine _knockbackCo;
    private Coroutine _slowDownCo;

    protected virtual void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        stats = GetComponent<EntityStats>();

        stateMachine = new StateMachine();
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        HandleCollisionDetection();
        stateMachine.UpdateActiveState();
    }

    public void CurrentStateAnimationTrigger()
    {
        stateMachine.currentState.AnimationTrigger();
    }

    public virtual void EntityDeath()
    {
        
    }

    public virtual void SlowDownEntityBy(float duration, float slowMultiplier)
    {
        if(_slowDownCo != null)StopCoroutine(_slowDownCo);
        
        _slowDownCo = StartCoroutine(SlowDownEntityCo(duration, slowMultiplier));
    }

    protected virtual IEnumerator SlowDownEntityCo(float duration, float slowMultiplier)
    {
        yield return null;
    }

    public void ReceiveKnockback(Vector2 knockback, float duration)
    {
        if(_knockbackCo != null)
            StopCoroutine(_knockbackCo);
        
        _knockbackCo = StartCoroutine(KnockbackCo(knockback, duration));
    }

    private IEnumerator KnockbackCo(Vector2 knockback, float duration)
    {
        _isKnocked = true;
        rb.linearVelocity = knockback;
        
        yield return new WaitForSeconds(duration);
        
        rb.linearVelocity = Vector2.zero;
        _isKnocked = false;
    }

    public void SetVelocity(float xVelocity, float yVelocity)
    {
        if (_isKnocked)
            return;
        
        rb.linearVelocity = new Vector2(xVelocity, yVelocity);
        HandleFlip(xVelocity);
    }

    public void HandleFlip(float xVelocity)
    {
        if (xVelocity > 0 && facingRight == false)
        {
            Flip();
        } else if(xVelocity < 0 && facingRight)
        {
            Flip();
        }
    }

    public void Flip()
    {
        transform.Rotate(0f, 180f, 0f);
        facingRight = !facingRight;
        facingDirection = facingDirection * -1;
        
        OnFlipped?.Invoke();
    }

    private void HandleCollisionDetection()
    {
        isGround = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
        isWall = Physics2D.Raycast(primaryWallCheck.position, Vector2.right * facingDirection, wallCheckDistance,
                     whatIsGround)
                 && Physics2D.Raycast(secondaryWallCheck.position, Vector2.right * facingDirection, wallCheckDistance,
                     whatIsGround);
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, groundCheck.position + new Vector3(0, -groundCheckDistance));
        Gizmos.DrawLine(primaryWallCheck.position, primaryWallCheck.position + new Vector3(wallCheckDistance * facingDirection, 0));
        Gizmos.DrawLine(secondaryWallCheck.position, secondaryWallCheck.position + new Vector3(wallCheckDistance * facingDirection, 0));
    }
}
