using UnityEngine;
using System.Collections;

public class EntityStatusHandler : MonoBehaviour
{
    private Entity _entity;
    private Entity_VFX _entityVFX;
    private EntityStats _entityStats;
    private Entity_Health _entityHealth;
    private ElementalType _currentEffect = ElementalType.None;
    
    [Header("Lighting details")]
    [SerializeField] private GameObject _lightningStrikeVfxPrefab;
    [SerializeField] private float currentCharge;
    [SerializeField] private float maxCharge = 1f;
    private Coroutine _lightningStrikeCo;

    private void Awake()
    {
        _entityStats = GetComponent<EntityStats>();
        _entityHealth = GetComponent<Entity_Health>();
        _entity = GetComponent<Entity>();
        _entityVFX = GetComponent<Entity_VFX>();
    }

    public void ApplyLightningEffect(float duration, float damage, float charge)
    {
        float lightningResistance = _entityStats.GetElementalResistance(ElementalType.Lightning);
        float finalCharge = charge * (1 - lightningResistance);
        currentCharge = currentCharge + finalCharge;
        
        if (currentCharge >= maxCharge)
        {
            DoLightningStrike(damage);
            StopLightningStrikeEffect();
            return;
        }
        if(_lightningStrikeCo != null)
            StopCoroutine(_lightningStrikeCo);
        
        _lightningStrikeCo = StartCoroutine(LightningStrikeCo(duration));
    }

    public void StopLightningStrikeEffect()
    {
        _currentEffect =  ElementalType.None;
        currentCharge = 0f;
        _entityVFX.StopAllVfx();
    }

    private void DoLightningStrike(float damage)
    {
        Instantiate(_lightningStrikeVfxPrefab, transform.position, Quaternion.identity);
        _entityHealth.ReduceHealth(damage);
    }

    private IEnumerator LightningStrikeCo(float duration)
    {
        _currentEffect = ElementalType.Lightning;
        _entityVFX.PlayOnStatusVfx(duration, ElementalType.Lightning);
        yield return new WaitForSeconds(duration);
        StopLightningStrikeEffect();
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
            _entityHealth.ReduceHealth(damagePerTick);
            yield return new WaitForSeconds(tickInterval);
        }
        
        _currentEffect =  ElementalType.None;
    }

    public bool CanBeApplied(ElementalType elemental)
    {
        if(elemental == ElementalType.Lightning && _currentEffect == ElementalType.Lightning)
            return true;
        
        return _currentEffect == ElementalType.None;
    }
    
}
