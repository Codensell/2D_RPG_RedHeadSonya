using UnityEngine;

[CreateAssetMenu(menuName = "Default Stat Setup", fileName = "Default Stat Setup")]
public class StatSetupSo : ScriptableObject
{
    [Header("Resources")] 
    public float maxHealth = 100;
    public float healthRegen;

    [Header("Offence - physical damage")] 
    public float attackSpeed = 1;
    public float damage = 10;
    public float critChance;
    public float critPower = 150;
    public float armorReduction;
    
    [Header("Offence - elemental damage")]
    public float fireDamage;
    public float iceDamage;
    public float lightningDamage;

    [Header("Defence - physical damage")] 
    public float armor;
    public float evasion;
    
    [Header("Defence - elemental damage")]
    public float fireResistance;
    public float iceResistance;
    public float lightningResistance;
    
    [Header("Major Stats")]
    public float strength;
    public float agility;
    public float intelligence;
    public float vitality;
}
