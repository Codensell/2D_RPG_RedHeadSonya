using UnityEngine;

[CreateAssetMenu(menuName = "Skill Data", fileName = "Skill Data - ")]
public class SkillDataSo : ScriptableObject
{
    public int cost;
    public SkillType skillType;
    public UpgradeData upgradeData;
    
    [Header("Skill Details")]
    public string displayName;
    [TextArea]
    public string skillDescription;
    public Sprite icon;
    
}

[System.Serializable]
public class UpgradeData
{
    public SkillUpgradeType upgradeType;
    public float cooldown;
}
