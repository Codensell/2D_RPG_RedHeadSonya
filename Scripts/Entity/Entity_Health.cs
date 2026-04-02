using UnityEngine;
using UnityEngine.UI;

public class Entity_Health : MonoBehaviour, IDamagable
{
    private Slider healthBar;
    private Entity_VFX entityVfx;
    private Entity entity;
    private EntityStats entityStats;

    [SerializeField] protected float currentHp;
    [SerializeField] protected bool isDead;

    [Header("On Damage Knockback")] 
    [SerializeField] private Vector2 knockbackPower = new Vector2(1.5f, 2.5f);
    [SerializeField] private Vector2 heavyKnockbackPower = new Vector2(6f, 6f);
    [SerializeField] private float knockbackDuration = 0.2f;
    [SerializeField] private float heavyKnockbackDuration = .5f;
    
    [Header("On Heavy Damage Knockback")]
    [SerializeField] private float heavyDamageTreshold = 0.3f;

    protected virtual void Awake()
    {
        entityVfx = GetComponent<Entity_VFX>();
        entity = GetComponent<Entity>();
        entityStats = GetComponent<EntityStats>();
        healthBar = GetComponentInChildren<Slider>();
        
        currentHp = entityStats.GetMaxHealth();
        UpdateHealthBar();
    }
    public virtual bool TakeDamage(float damage, float elementalDamage, ElementalType elemental, Transform damageDealer)
    {
        if (isDead)
            return false;
        if (AttackEvaded())
        {
            Debug.Log($"Evaded + {gameObject.name}");
            return false;
        }

        EntityStats offensiveStats = damageDealer.GetComponent<EntityStats>();
        float armorReduction = offensiveStats != null ? offensiveStats.GetArmorReduction() : 0;
        
        float mitigation = entityStats.GetArmorMitigation(armorReduction);
        float physicalDamageTaken = damage * (1 - mitigation);

        float resistance = entityStats.GetElementalResistance(elemental);
        float elementalDamageTaken = elementalDamage * (1 - resistance);
        
        TakeKnockback(damageDealer, physicalDamageTaken);
        
        entityVfx?.PlayOnDamageVfx();
        
        ReduceHp(physicalDamageTaken + elementalDamageTaken);
        
        return true;
    }

    private void TakeKnockback(Transform damageDealer, float physicalDamageTaken)
    {
        Vector2 knockback = CalculateKnockback(physicalDamageTaken, damageDealer);
        float duration = CalculateDuration(physicalDamageTaken);
        
        entity?.ReceiveKnockback(knockback, duration);
    }

    private bool AttackEvaded()
    {
        return Random.Range(0, 100) < entityStats.GetEvasion();
    }

    protected void ReduceHp(float damage)
    {
        currentHp -= damage;
        UpdateHealthBar();

        if (currentHp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        entity.EntityDeath();
    }

    private void UpdateHealthBar()
    {
        if (healthBar == null) return;
        
        healthBar.value = currentHp / entityStats.GetMaxHealth();
    }

    private Vector2 CalculateKnockback(float damage, Transform damageDealer)
    {
        int direction = transform.position.x > damageDealer.position.x ? 1 : -1;
        
        Vector2 knockback = IsHeavyDamage(damage) ? heavyKnockbackPower : knockbackPower;
        knockback.x = knockback.x * direction;
        
        return knockback;
    }
    
    private float CalculateDuration(float damage) => IsHeavyDamage(damage) ? heavyKnockbackDuration : knockbackDuration;
    private bool IsHeavyDamage(float damage) => damage / entityStats.GetMaxHealth() > heavyDamageTreshold;
    
}
