using UnityEngine;

public class Entity_CombatComponent : MonoBehaviour
{
    private Entity_VFX vfx;
    private EntityStats stat;
    
    [Header("Target detection")]
    [SerializeField] private Transform targetCheck;
    [SerializeField] private LayerMask whatIsTarget;
    [SerializeField] private float targetCheckRadius;

    private void Awake()
    {
        vfx = GetComponent<Entity_VFX>();
        stat = GetComponent<EntityStats>();
    }

    public void PerformAttack()
    {
        foreach (var target in GetDetectedColliders())
        {
            IDamagable damagable = target.GetComponent<IDamagable>();

            if (damagable == null) continue;

            float damage = stat.GetPhysicalDamage(out bool isCrit);
            bool targetGotHit = damagable.TakeDamage(damage, transform);
            if (targetGotHit)
            {
                vfx.CreateOnHitVFX(target.transform, isCrit);
            }
                
        }
    }

    protected Collider2D[] GetDetectedColliders ()
    {
        return Physics2D.OverlapCircleAll(targetCheck.position, targetCheckRadius, whatIsTarget);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(targetCheck.position, targetCheckRadius);
    }
}
