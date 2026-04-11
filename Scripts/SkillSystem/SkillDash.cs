using UnityEngine;

public class SkillDash : SkillBase
{
    public void OnStartEffect()
    {
        if (Unlocked(SkillUpgradeType.DashCloneOnStart) || Unlocked(SkillUpgradeType.DashCloneOnStartAndArrival))
        {
            CreateClone();
        }
        if (Unlocked(SkillUpgradeType.DashShardOnStart) || Unlocked(SkillUpgradeType.DashShardOnStartAndArrival))
        {
            CreateShard();
        }
    }

    public void OnEndEffect()
    {
        if (Unlocked(SkillUpgradeType.DashCloneOnStartAndArrival))
        {
            CreateClone();
        }

        if (Unlocked(SkillUpgradeType.DashShardOnStartAndArrival))
        {
            CreateShard();
        }
    }
    private void CreateShard()
    {
        Debug.Log("Creating Shard");
    }

    private void CreateClone()
    {
        Debug.Log("Creating Clone");
    }
}
