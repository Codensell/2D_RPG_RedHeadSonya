using UnityEngine;

public class UI : MonoBehaviour
{
    public UISkillTree skillTree;
    public UISkillToolTip uiSkillToolTip;
    private bool _skillTreeEnabled;

    private void Awake()
    {
        uiSkillToolTip = GetComponentInChildren<UISkillToolTip>();
        skillTree = GetComponentInChildren<UISkillTree>(true);
    }

    public void ToggleSkillTreeUI()
    {
        _skillTreeEnabled = !_skillTreeEnabled;
        skillTree.gameObject.SetActive(_skillTreeEnabled);
        uiSkillToolTip.ShowToolTip(false, null);
    }
}
