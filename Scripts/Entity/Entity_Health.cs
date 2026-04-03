using UnityEngine;
using UnityEngine.UI;

public class Entity_Health : MonoBehaviour, IDamagable
{
    private Slider _healthBar;
    private Entity_VFX _entityVfx;
    private Entity _entity;
    private EntityStats _entityStats;

    [SerializeField] protected float currentHealth;
    [SerializeField] protected bool isDead;

    [Header("Health Regen")] 
    [SerializeField] private float regenInterval = 1f;
    [SerializeField] private bool canRegeneratehealth = true;

    [Header("On Damage Knockback")] 
    [SerializeField] private Vector2 knockbackPower = new Vector2(1.5f, 2.5f);
    [SerializeField] private Vector2 heavyKnockbackPower = new Vector2(6f, 6f);
    [SerializeField] private float knockbackDuration = 0.2f;
    [SerializeField] private float heavyKnockbackDuration = .5f;
    
    [Header("On Heavy Damage Knockback")]
    [SerializeField] private float heavyDamageTreshold = 0.3f;

    protected virtual void Awake()
    { 
        _entityVfx = GetComponent<Entity_VFX>(); _entity = GetComponent<Entity>();
        _entityStats = GetComponent<EntityStats>();
        _healthBar = GetComponentInChildren<Slider>();
        
        currentHealth = _entityStats.GetMaxHealth();
        UpdateHealthBar();
        
        InvokeRepeating(nameof(RegenerateHealth), 0, regenInterval);
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
        
        float mitigation = _entityStats.GetArmorMitigation(armorReduction);
        float physicalDamageTaken = damage * (1 - mitigation);

        float resistance = _entityStats.GetElementalResistance(elemental);
        float elementalDamageTaken = elementalDamage * (1 - resistance);
        
        TakeKnockback(damageDealer, physicalDamageTaken);
        ReduceHealth(physicalDamageTaken + elementalDamageTaken);
        
        return true;
    }

    private void TakeKnockback(Transform damageDealer, float physicalDamageTaken)
    {
        Vector2 knockback = CalculateKnockback(physicalDamageTaken, damageDealer);
        float duration = CalculateDuration(physicalDamageTaken);
        _entity?.ReceiveKnockback(knockback, duration);
    }

    private bool AttackEvaded()
    {
        return Random.Range(0, 100) < _entityStats.GetEvasion();
    }

    private void RegenerateHealth()
    {
        if (canRegeneratehealth == false) return;
        
        float regenAmount = _entityStats.resources.healthRegen.GetValue();
        IncreaseHealth(regenAmount);
    }
    public void IncreaseHealth(float healAmount)
    {
        if (isDead) return;
        
        float newHealth = currentHealth + healAmount;
        float maxHealth = _entityStats.GetMaxHealth();
        
        currentHealth = Mathf.Min(newHealth, maxHealth);
        UpdateHealthBar();
        
    }

    public void ReduceHealth(float damage)
    { 
        _entityVfx?.PlayOnDamageVfx();
        currentHealth -= damage;
        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true; _entity.EntityDeath();
    }

    private void UpdateHealthBar()
    {
        if (_healthBar == null) return;
        
        _healthBar.value = currentHealth / _entityStats.GetMaxHealth();
    }

    private Vector2 CalculateKnockback(float damage, Transform damageDealer)
    {
        int direction = transform.position.x > damageDealer.position.x ? 1 : -1;
        
        Vector2 knockback = IsHeavyDamage(damage) ? heavyKnockbackPower : knockbackPower;
        knockback.x = knockback.x * direction;
        
        return knockback;
    }
    
    private float CalculateDuration(float damage) => IsHeavyDamage(damage) ? heavyKnockbackDuration : knockbackDuration;
    private bool IsHeavyDamage(float damage) => damage / _entityStats.GetMaxHealth() > heavyDamageTreshold;
    
}
