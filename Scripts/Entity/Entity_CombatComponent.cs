using UnityEngine;

public class Entity_CombatComponent : MonoBehaviour
{
    private Entity_VFX _vfx;
    private EntityStats _stat;
    
    [Header("Target detection")]
    [SerializeField] private Transform targetCheck;
    [SerializeField] private LayerMask whatIsTarget;
    [SerializeField] private float targetCheckRadius;

    [Header("Status effect details")] 
    [SerializeField] private float defaultDuration = 3f;
    [SerializeField] private float chillSlowMultiplier = .2f;
    [SerializeField] private float lightningChargeBuildUp = .4f;
    [Space] 
    [SerializeField] private float fireScale = .8f;
    [SerializeField] private float lightningScale = 2.5f;

    private void Awake()
    {
        _vfx = GetComponent<Entity_VFX>();
        _stat = GetComponent<EntityStats>();
    }

    public void PerformAttack()
    {
        foreach (var target in GetDetectedColliders())
        {
            IDamagable damagable = target.GetComponent<IDamagable>();

            if (damagable == null) continue;

            float elementalDamage = _stat.GetElementalDamage(out ElementalType elemental);
            float damage = _stat.GetPhysicalDamage(out bool isCrit);
            bool targetGotHit = damagable.TakeDamage(damage, elementalDamage, elemental, transform);
            
            if(elemental != ElementalType.None)
                ApplyStatusEffect(target.transform, elemental);
            
            if (targetGotHit)
            {
                _vfx.UpdateOnHitColor(elemental);
                _vfx.CreateOnHitVFX(target.transform, isCrit);
            }
        }
    }
    public void ApplyStatusEffect(Transform target, ElementalType elemental, float scaleFactor = 1)
    {
        EntityStatusHandler statusHandler = target.GetComponent<EntityStatusHandler>();

        if (statusHandler == null)
            return;
        
        if (elemental == ElementalType.Ice && statusHandler.CanBeApplied(ElementalType.Ice))
            statusHandler.ApplyChilledEffect(defaultDuration, chillSlowMultiplier *  scaleFactor);

        if (elemental == ElementalType.Fire && statusHandler.CanBeApplied(ElementalType.Fire))
        {
            scaleFactor = fireScale;
            float fireDamage = _stat.offence.fireDamage.GetValue() * scaleFactor;
            statusHandler.ApplyBurnEffect(defaultDuration, fireDamage);
        }

        if (elemental == ElementalType.Lightning && statusHandler.CanBeApplied(ElementalType.Lightning))
        {
            scaleFactor = lightningScale;
            float lightningDamage = _stat.offence.lightningDamage.GetValue() * scaleFactor;
            statusHandler.ApplyLightningEffect(defaultDuration, lightningDamage, lightningChargeBuildUp);
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
