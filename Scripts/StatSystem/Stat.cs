using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class Stat
{
    [SerializeField] private float baseValue;
    [SerializeField] private List<StatModifier> modifiers = new List<StatModifier>();

    private float _finalValue;
    private bool _needToCalculate = true;

    public float GetValue()
    {
        if(_needToCalculate)
        {
            _finalValue = GetFinalValue();
            _needToCalculate = false;
        }
        
        return _finalValue;
    }
    
    public void SetBaseValue(float value) => baseValue = value;

    public void AddModifier(float value, string source)
    {
        StatModifier modToAdd = new StatModifier(value, source);
        modifiers.Add(modToAdd);
        _needToCalculate = true;
    }

    private float GetFinalValue()
    {
        float finalValue = baseValue;

        foreach (var modifier in modifiers)
        {
            finalValue += modifier.value;
        }
        
        return finalValue;
    }

    public void RemoveModifier(string source)
    {
        modifiers.RemoveAll(modifier => modifier.source == source);
        _needToCalculate = true;
    }
}

[Serializable]
public class StatModifier
{
    public float value;
    public string source;

    public StatModifier(float value, string source)
    {
        this.value = value;
        this.source = source;
    }
}
