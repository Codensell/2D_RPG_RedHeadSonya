using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class UITreeNode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    private UI _ui;
    private RectTransform _rect;
    private UISkillTree _skillTree;
    private UITreeConnectionHandler _connectionHandler;

    [Header("Unlock Details")] 
    public UITreeNode[] neededNodes;
    public UITreeNode[] conflictNodes;
    public bool isUnlocked;
    public bool isLocked;
    
    public SkillDataSo skillData;
    [SerializeField] private string skillName;
    [SerializeField] private Image skillIcon;
    [SerializeField] private int skillCost;
    [SerializeField] private string lockedColorHex = "#8E8080";
    
    private Color _lastColor;
    
    private void Awake()
    {
        _ui = GetComponentInParent<UI>();
        _rect = GetComponent<RectTransform>();
        _skillTree = GetComponentInParent<UISkillTree>();
        _connectionHandler = GetComponent<UITreeConnectionHandler>();
        
        UpdateIconColor(GetColorByHex(lockedColorHex));
    }

    public void Refund()
    {
        isUnlocked = false;
        isLocked = false;
        UpdateIconColor(GetColorByHex(lockedColorHex));
        
        _skillTree.AddSkillPoints(skillData.cost);
        _connectionHandler.UnlockConnectionImage(false);
    }

    private void UnLock()
    {
        isUnlocked = true;
        UpdateIconColor(Color.white);
        _skillTree.RemoveSkillPoint(skillData.cost);
        _connectionHandler.UnlockConnectionImage(true);
        LockConflictNodes();
        
        _skillTree.skillManager.GetSkillByType(skillData.skillType).SetSkillUpgrade(skillData.upgradeData);
    }

    private bool CanBeUnlocked()
    {
        if (isLocked || isUnlocked) return false;
        
        if(_skillTree.EnoughSkillPoints(skillData.cost) == false) return false;

        foreach (var node in neededNodes)
        {
            if(node.isUnlocked == false) return false;
        }

        foreach (var node in conflictNodes)
        {
            if(node.isUnlocked)return false;
        }
        return true;
    }

    private void LockConflictNodes()
    {
        foreach (var node in conflictNodes)
            node.isLocked = true;
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
        else if(isLocked)
            _ui.uiSkillToolTip.LockedSkillEffect();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        _ui.uiSkillToolTip.ShowToolTip(true, _rect, this);
        if (isUnlocked == false || isLocked == false) ToggleNodeHighlight(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _ui.uiSkillToolTip.ShowToolTip(false, _rect);
        if (isUnlocked == false || isLocked == false) ToggleNodeHighlight(false);
    }

    private void ToggleNodeHighlight(bool highlight)
    {
        Color highlightColor = Color.white * .9f; highlightColor.a = 1;
        Color colorToAplly = highlight ? highlightColor : _lastColor;
        
        UpdateIconColor(colorToAplly);
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
        skillCost = skillData.cost;
        gameObject.name = "UI TreeNode" + skillData.displayName;
    }

    private void OnDisable()
    {
        if(isLocked)UpdateIconColor(GetColorByHex(lockedColorHex));
        if(isUnlocked)UpdateIconColor(Color.white);
    }

}
