using UnityEngine;
using System.Collections;

public class EntityStatusHandler : MonoBehaviour
{
    private Entity _entity;
    private Entity_VFX _entityVFX;
    private EntityStats _entityStats;
    private Entity_Health _entityHealth;
    private ElementalType _currentEffect = ElementalType.None;

    private void Awake()
    {
        _entityStats = GetComponent<EntityStats>();
        _entityHealth = GetComponent<Entity_Health>();
        _entity = GetComponent<Entity>();
        _entityVFX = GetComponent<Entity_VFX>();
    }

    public void ApplyBurnEffect(float duration, float fireDamage)
    {
        float fireResistance = _entityStats.GetElementalResistance(ElementalType.Fire);
        float finalDamage = fireDamage * (1 - fireResistance);

        StartCoroutine(BurnEffectCo(duration, finalDamage));
    }

    public void ApplyChilledEffect(float duration, float slowMultiplier)
    {
        float iceResistance = _entityStats.GetElementalResistance(ElementalType.Ice);
        float reduceDuration = duration * (1 - iceResistance);
        StartCoroutine(ChilledEffectCo(reduceDuration, slowMultiplier));
    }

    private IEnumerator ChilledEffectCo(float duration, float slowMultiplier)
    {
        _entity.SlowDownEntityBy(duration, slowMultiplier);
        _currentEffect = ElementalType.Ice;
        _entityVFX.PlayOnStatusVfx(duration, ElementalType.Ice);
        
        yield return new WaitForSeconds(duration);
        
        _currentEffect = ElementalType.None;
    }
    private IEnumerator BurnEffectCo(float duration, float totalDamage)
    {
        _currentEffect = ElementalType.Fire;
        _entityVFX.PlayOnStatusVfx(duration, ElementalType.Fire);
        
        int ticksPerSecond = 2;
        int tickCount = Mathf.RoundToInt(ticksPerSecond * duration);
        
        float damagePerTick = totalDamage / tickCount;
        float tickInterval = 1f / ticksPerSecond;

        for (int i = 0; i < tickCount; i++)
        {
            _entityHealth.ReduceHp(damagePerTick);
            yield return new WaitForSeconds(tickInterval);
        }
        
        _currentEffect =  ElementalType.None;
    }

    public bool CanBeApplied(ElementalType elemental)
    {
        return _currentEffect == ElementalType.None;
    }
    
}
