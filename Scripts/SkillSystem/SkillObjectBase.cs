using UnityEngine;

public class SkillObjectBase : MonoBehaviour
{
    [SerializeField] protected LayerMask whatIsEnemy;
    [SerializeField] protected Transform targetCheck;
    [SerializeField] protected float checkRadius = 1;

    protected void damageEnemiesInRadius(Transform t, float radius)
    {
        foreach (var target in EnemiesAround(t, radius))
        {
            IDamagable damagable = target.GetComponent<IDamagable>();
            if (damagable == null)
                continue;

            damagable.TakeDamage(1, 1, ElementalType.None, transform);
        }
    }

    protected Collider2D[] EnemiesAround(Transform t, float radius)
    {
        return Physics2D.OverlapCircleAll(t.position, radius, whatIsEnemy);
    }
    protected virtual void OnDrawGizmos()
    {
        if(targetCheck == null)
            targetCheck = transform;
        
        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }
    
}
