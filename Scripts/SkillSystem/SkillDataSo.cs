using UnityEngine;

[CreateAssetMenu(menuName = "Skill Data", fileName = "Skill Data - ")]
public class SkillDataSo : ScriptableObject
{
    public int cost;
    
    [Header("Skill Details")]
    public string displayName;
    [TextArea]
    public string skillDescription;
    public Sprite icon;
    
}
