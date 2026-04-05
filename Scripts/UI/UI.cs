using UnityEngine;

public class UI : MonoBehaviour
{
    public UISkillToolTip uiSkillToolTip;

    private void Awake()
    {
        uiSkillToolTip = GetComponentInChildren<UISkillToolTip>();
    }
}
