using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class EntityStats : MonoBehaviour
{
    public StatSetupSo defaultStatSetup;
    
    public StatResourceGroup resources;
    public StatMajorGroup major;
    public StatOffenceGroup offence;
    public StatDefenceGroup defence;

    public float GetElementalDamage(out ElementalType elemental, float scaleFactor = 1)
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

        return finalDamage *  scaleFactor;
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

    public float GetPhysicalDamage(out bool isCrit, float scaleFactor = 1)
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
        
        return finalDamage *  scaleFactor;
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
        float baseMaxHealth = resources.maxHealth.GetValue();
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

    public Stat GetsStatByType(StatType statType)
    {
        switch (statType)
        {
            case StatType.MaxHealth: return resources.maxHealth;
            case StatType.HealthRegen: return resources.healthRegen;
            
            case StatType.Strength: return major.strength;
            case StatType.Agility: return major.agility;
            case StatType.Intelligence: return  major.intelligence;
            case StatType.Vitality: return major.vitality;
            
            case StatType.AttackSpeed: return offence.attackSpeed;
            case StatType.Damage: return offence.damage;
            case StatType.CritChance: return offence.critChance;
            case StatType.CritPower: return offence.critPower;
            case StatType.ArmorReduction: return offence.armorReduction;
            
            case StatType.FireDamage: return offence.fireDamage;
            case StatType.IceDamage: return offence.iceDamage;
            case StatType.LightningDamage: return offence.lightningDamage;
            
            case StatType.Armor: return defence.armor;
            case StatType.Evasion: return defence.evasion;
            
            case StatType.IceResistance: return defence.iceRes;
            case StatType.FireResistance: return defence.fireRes;
            case StatType.LightningResistance: return  defence.lightningRes;
            
            default: 
                Debug.LogWarning($"Stat type {statType} not implemented yet.");
                return null;
        }
    }

    [ContextMenu("Update deefault setup")]
    public void ApplyDefaultStatSetup()
    {
        if (defaultStatSetup == null) return;
        
        resources.maxHealth.SetBaseValue(defaultStatSetup.maxHealth);
        resources.healthRegen.SetBaseValue(defaultStatSetup.healthRegen);
        
        major.strength.SetBaseValue(defaultStatSetup.strength);
        major.agility.SetBaseValue(defaultStatSetup.agility);
        major.intelligence.SetBaseValue(defaultStatSetup.intelligence);
        major.vitality.SetBaseValue(defaultStatSetup.vitality);
        
        offence.damage.SetBaseValue(defaultStatSetup.damage);
        offence.attackSpeed.SetBaseValue(defaultStatSetup.attackSpeed);
        offence.critChance.SetBaseValue(defaultStatSetup.critChance);
        offence.critPower.SetBaseValue(defaultStatSetup.critPower);
        offence.armorReduction.SetBaseValue(defaultStatSetup.armorReduction);
        
        offence.fireDamage.SetBaseValue(defaultStatSetup.fireDamage);
        offence.iceDamage.SetBaseValue(defaultStatSetup.iceDamage);
        offence.lightningDamage.SetBaseValue(defaultStatSetup.lightningDamage);
        
        defence.armor.SetBaseValue(defaultStatSetup.armor);
        defence.evasion.SetBaseValue(defaultStatSetup.evasion);
        
        defence.fireRes.SetBaseValue(defaultStatSetup.fireResistance);
        defence.lightningRes.SetBaseValue(defaultStatSetup.lightningResistance);
        defence.iceRes.SetBaseValue(defaultStatSetup.iceResistance);
    }
}
