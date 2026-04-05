using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UITreeNode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [SerializeField] private SkillDataSo skillData;
    [SerializeField] private string skillName;
    [SerializeField] private Image skillIcon;
    [SerializeField] private string lockedColorHex = "#8E8080";
    
    private Color _lastColor;
    public bool isUnlocked;
    public bool isLocked;

    private void OnValidate()
    {
        if (skillData == null) return;
        
        skillName = skillData.displayName;
        skillIcon.sprite = skillData.icon;
        gameObject.name = "UI TreeNode" + skillData.displayName;
    }
    private void Awake()
    {
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
        if(isUnlocked == false)
            UpdateIconColor(Color.white * .9f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(isUnlocked ==  false)
            UpdateIconColor(_lastColor);
    }

    private Color GetColorByHex(string hexNumber)
    {
        ColorUtility.TryParseHtmlString(hexNumber, out Color color);
        return color;
    }

}
