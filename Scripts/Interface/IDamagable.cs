using UnityEngine;

public interface IDamagable
{
    public bool TakeDamage(float damage, float elementalDamage, ElementalType elemental, Transform damageDealer);
}
