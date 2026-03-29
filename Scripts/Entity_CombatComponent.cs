using UnityEngine;

public class Entity_CombatComponent : MonoBehaviour
{
    public float damage = 10f;
    
    [Header("Target detection")]
    [SerializeField] private Transform targetCheck;
    [SerializeField] private LayerMask whatIsTarget;
    [SerializeField] private float targetCheckRadius;

    public void PerformAttack()
    {
        foreach (var target in GetDetectedColliders())
        {
            Entity_Health targetHealth = target.GetComponent<Entity_Health>();
            
            targetHealth?.TakeDamage(damage, transform);
                
        }
    }

    private Collider2D[] GetDetectedColliders ()
    {
        return Physics2D.OverlapCircleAll(targetCheck.position, targetCheckRadius, whatIsTarget);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(targetCheck.position, targetCheckRadius);
    }
}
