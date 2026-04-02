using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class EntityStats : MonoBehaviour
{
    public Stat maxHp;
    public StatMajorGroup major;
    public StatOffenceGroup offence;
    public StatDefenceGroup defence;

    public float GetPhysicalDamage(out bool isCrit)
    {
        float baseDamage = offence.damage.GetValue();
        float bonusDamage = major.strength.GetValue();
        float totalBaseDamage = baseDamage + bonusDamage;
        
        float baseCritChance = offence.critChance.GetValue();
        float bonusCritChance = major.agility.GetValue() * .3f;
        float critChance = baseCritChance + bonusCritChance;
        
        float baseCritPower = offence.critPower.GetValue();
        float bonusCritPower = major.strength.GetValue() * .5f;
        float critPower = (baseCritPower + bonusCritPower) / 100; //multiplier

        isCrit = Random.Range(0, 100) < critChance;
        float finalDamage = isCrit ? totalBaseDamage * critPower : totalBaseDamage;
        
        return finalDamage;
    }

    public float GetArmorMitigation(float armorReduction)
    {
        float baseArmor = defence.armor.GetValue();
        float bonusArmor = major.vitality.GetValue(); // +1 per vitality
        float totalArmor = baseArmor + bonusArmor;
        
        float reductionMultiplier = Math.Clamp(1 - armorReduction, 0, 1);
        float effectiveArmor = totalArmor * reductionMultiplier;
        
        float mitigation = effectiveArmor / (effectiveArmor + 100);
        float mitigationCap = .85f; //cap for mitigation
        
        float finalMitigation = Math.Clamp(mitigation, 0, mitigationCap);
        
        return finalMitigation;
    }

    public float GetArmorReduction()
    {
        float finalReduction = offence.armorReduction.GetValue() / 100;
        
        return  finalReduction;
    }

    public float GetMaxHealth()
    {
        float baseMaxHealth = maxHp.GetValue();
        float bonusMaxHealth = major.vitality.GetValue() * 5;
        
        float finalMaxHealth = baseMaxHealth + bonusMaxHealth;
        
        return finalMaxHealth;
    }

    public float GetEvasion()
    {
        float baseEvasion = defence.evasion.GetValue();
        float bonusEvasion = major.agility.GetValue() * .5f;
        float totalEvasion = baseEvasion + bonusEvasion;
        float evasionCap = 85f;
        float finalEvasion = Math.Clamp(totalEvasion, 0, evasionCap);
        
        return finalEvasion;
    }
    
}
