using UnityEngine;

public class Entity_CombatComponent : MonoBehaviour
{
    private Entity_VFX vfx;
    private EntityStats stat;
    
    [Header("Target detection")]
    [SerializeField] private Transform targetCheck;
    [SerializeField] private LayerMask whatIsTarget;
    [SerializeField] private float targetCheckRadius;

    [Header("Status effect details")] 
    [SerializeField] private float defaultDuration = 3f;
    [SerializeField] private float chillSlowMultiplier = .2f;

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

            float elementalDamage = stat.GetElementalDamage(out ElementalType elemental);
            float damage = stat.GetPhysicalDamage(out bool isCrit);
            bool targetGotHit = damagable.TakeDamage(damage, elementalDamage, elemental, transform);
            
            if(elemental != ElementalType.None)
                ApplyStatusEffect(target.transform, elemental);
            
            if (targetGotHit)
            {
                vfx.UpdateOnHitColor(elemental);
                vfx.CreateOnHitVFX(target.transform, isCrit);
            }
                
        }
    }
    public void ApplyStatusEffect(Transform target, ElementalType elemental)
    {
        EntityStatusHandler statusHandler = target.GetComponent<EntityStatusHandler>();

        if (statusHandler == null)
            return;
        
        if (elemental == ElementalType.Ice && statusHandler.CanBeApplied(ElementalType.Ice))
            statusHandler.ApplyChilledEffect(defaultDuration, chillSlowMultiplier);
        
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
