using UnityEngine;
using System;

[Serializable]
public class StatOffenceGroup
{
    //Physical
    public Stat damage;
    public Stat critPower;
    public Stat critChance;
    public Stat armorReduction;
    
    //Elemental damage
    public Stat fireDamage;
    public Stat iceDamage;
    public Stat lightningDamage;
}
