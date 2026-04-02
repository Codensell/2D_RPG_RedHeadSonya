using UnityEngine;
using System;

[Serializable]
public class StatDefenceGroup
{
    //Physical defence
    public Stat armor;
    public Stat evasion;

    //Elemental resistance
    public Stat fireRes;
    public Stat iceRes;
    public Stat lightningRes;
}
