using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class EntityStats : MonoBehaviour
{
    public Stat maxHp;
    public StatMajorGroup major;
    public StatOffenceGroup offence;
    public StatDefenceGroup defence;

    public float GetElementalDamage(out ElementalType elemental)
    {
        float fireDamage = offence.fireDamage.GetValue();
        float iceDamage = offence.iceDamage.GetValue();
        float lightningDamage = offence.lightningDamage.GetValue();
        float bonusElementalDamage = major.intelligence.GetValue();

        float highestDamage = fireDamage;
        elemental = ElementalType.Fire;
        
        if(iceDamage > highestDamage)
        {
            highestDamage = iceDamage;
            elemental = ElementalType.Ice;
        }
        if(lightningDamage > highestDamage)
        {
            highestDamage = lightningDamage;
            elemental = ElementalType.Lightning;
        }

        if (highestDamage <= 0)
        {
            elemental = ElementalType.None;
            return 0;
        }

        float bonusFire = (fireDamage == highestDamage) ? 0 : fireDamage * 0.5f;
        float bonusIce = (iceDamage == highestDamage) ? 0 : iceDamage * 0.5f;
        float bonusLightning = (lightningDamage == highestDamage) ? 0 : lightningDamage * 0.5f;
        
        float weakerElementalDamage = bonusFire + bonusIce + bonusLightning;
        
        float finalDamage = highestDamage + weakerElementalDamage + bonusElementalDamage;

        return finalDamage;
    }

    public float GetElementalResistance(ElementalType elemental)
    {
        float baseResistance = 0;
        float bonusResistance = major.intelligence.GetValue() * .5f;

        switch (elemental)
        {
            case ElementalType.Fire:
                baseResistance = defence.fireRes.GetValue();
                break;
            case ElementalType.Ice:
                baseResistance = defence.iceRes.GetValue();
                break;
            case ElementalType.Lightning:
                baseResistance = defence.lightningRes.GetValue();
                break;
        }
        float resistance = baseResistance + bonusResistance;
        float resistanceCap = 75f;
        float finalResistance = Math.Clamp(resistance, 0, resistanceCap) / 100;
        
        return finalResistance;
    }

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
