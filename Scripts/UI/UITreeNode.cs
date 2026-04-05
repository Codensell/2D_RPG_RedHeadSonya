using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UITreeNode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    private UI _ui;
    private RectTransform _rect;
    
    [SerializeField] private SkillDataSo skillData;
    [SerializeField] private string skillName;
    [SerializeField] private Image skillIcon;
    [SerializeField] private string lockedColorHex = "#8E8080";
    
    private Color _lastColor;
    public bool isUnlocked;
    public bool isLocked;
    
    private void Awake()
    {
        _ui = GetComponentInParent<UI>();
        _rect = GetComponent<RectTransform>();
        
        UpdateIconColor(GetColorByHex(lockedColorHex));
    }

    private void UnLock()
    {
        isUnlocked = true;
        UpdateIconColor(Color.white);
    }

    private bool CanBeUnlocked()
    {
        if (isLocked || isUnlocked) return false;
        
        return true;
    }

    private void UpdateIconColor(Color color)
    {
        if (skillIcon == null) return;
        
        _lastColor = skillIcon.color;
        skillIcon.color = color;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (CanBeUnlocked()) UnLock();
        else Debug.Log("Can't unlock skill");
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        _ui.uiSkillToolTip.ShowToolTip(true, _rect, skillData);
        if(isUnlocked == false)
            UpdateIconColor(Color.white * .9f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _ui.uiSkillToolTip.ShowToolTip(false, _rect);
        if(isUnlocked ==  false)
            UpdateIconColor(_lastColor);
    }

    private Color GetColorByHex(string hexNumber)
    {
        ColorUtility.TryParseHtmlString(hexNumber, out Color color);
        return color;
    }
    private void OnValidate()
    {
        if (skillData == null) return;
        
        skillName = skillData.displayName;
        skillIcon.sprite = skillData.icon;
        gameObject.name = "UI TreeNode" + skillData.displayName;
    }

}
