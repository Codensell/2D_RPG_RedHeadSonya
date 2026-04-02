using UnityEngine;
using System;

[Serializable]
public class Stat
{
    [SerializeField] private float baseValue;

    public float GetValue()
    {
        return baseValue;
    }
}
