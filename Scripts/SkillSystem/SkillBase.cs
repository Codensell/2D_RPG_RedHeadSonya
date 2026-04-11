using UnityEngine;

public class SkillBase : MonoBehaviour
{
    [Header("General Details")]
    [SerializeField] protected SkillType skillType;
    [SerializeField] protected SkillUpgradeType skillUpgradeType;
    [SerializeField] private float cooldown;
    private float _lastTimeUsed;

    protected virtual void Awake()
    {
        _lastTimeUsed -= cooldown;
    }

    public void SetSkillUpgrade(UpgradeData upgrade)
    {
        skillUpgradeType = upgrade.upgradeType;
        cooldown = upgrade.cooldown;
    }

    public bool CanUseSkill()
    {
        if (OnCoolDown())
        {
            return false;
        }
        return true;
    }

    protected bool Unlocked(SkillUpgradeType upgradeToCheck) => skillUpgradeType == upgradeToCheck;
    
    private bool OnCoolDown() => Time.time < _lastTimeUsed + cooldown;
    public void SetSkillOnCoolDown() => _lastTimeUsed = Time.time;
    public void ResetCoolDown(float coolDownReduction) => _lastTimeUsed += coolDownReduction;
    public void ResetCoolDown() => _lastTimeUsed = Time.time;
}
