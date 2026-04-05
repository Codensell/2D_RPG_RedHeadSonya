using UnityEngine;

public class UI : MonoBehaviour
{
    public UIToolTip uiToolTip;

    private void Awake()
    {
        uiToolTip = GetComponentInChildren<UIToolTip>();
    }
}
