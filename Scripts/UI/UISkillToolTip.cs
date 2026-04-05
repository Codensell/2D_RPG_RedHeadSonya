using TMPro;
using UnityEngine;

public class UISkillToolTip : UIToolTip
{
    [SerializeField] private TextMeshProUGUI skillTitle;
    [SerializeField] private TextMeshProUGUI skillDescription;
    [SerializeField] private TextMeshProUGUI skillRequirements;

    public override void ShowToolTip(bool show, RectTransform targetRect)
    {
        base.ShowToolTip(show, targetRect);
    }

    public void ShowToolTip(bool show, RectTransform targetRect, SkillDataSo skillData)
    {
        base.ShowToolTip(show, targetRect);

        if (show == false) return;
        
        skillTitle.text = skillData.displayName;
        skillDescription.text = skillData.skillDescription;
        skillRequirements.text = "Requirements: \n"
            + " - " + skillData.cost + " skill points.";
    }
}
