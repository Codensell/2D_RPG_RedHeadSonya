using UnityEngine;
using System.Collections;

public class EntityStatusHandler : MonoBehaviour
{
    private Entity _entity;
    private Entity_VFX _entityVFX;
    private EntityStats _entityStats;
    private ElementalType _currentEffect = ElementalType.None;

    private void Awake()
    {
        _entityStats = GetComponent<EntityStats>();
        _entity = GetComponent<Entity>();
        _entityVFX = GetComponent<Entity_VFX>();
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

    public bool CanBeApplied(ElementalType elemental)
    {
        return _currentEffect == ElementalType.None;
    }
    
}
