using UnityEngine;

public class UITreeConnection : MonoBehaviour
{
    [SerializeField] private RectTransform connectionPoint;
    [SerializeField] private RectTransform connectionLength;

    public void DirectionConnection(NodeDirectionType direction, float length)
    {
        bool beActive = direction !=  NodeDirectionType.None;
        float finalLength = beActive ? length : 0;
        float angle = GetDirectionAngle(direction);
        
        connectionPoint.localRotation = Quaternion.Euler(0, 0, angle);
        connectionLength.sizeDelta = new Vector2(finalLength, connectionLength.sizeDelta.y);
    }
    
    private float GetDirectionAngle(NodeDirectionType nodeDirectionType)
    {
        switch (nodeDirectionType)
        {
            case NodeDirectionType.UpLeft: return 135f;
            case NodeDirectionType.Up: return 90f;
            case NodeDirectionType.UpRight: return 45f;
            case NodeDirectionType.Left: return 180f;
            case NodeDirectionType.Right: return 0f;
            case NodeDirectionType.DownLeft: return -135f;
            case NodeDirectionType.Down: return -90f;
            case NodeDirectionType.DownRight: return -45f;
            default: return 0f;
        }
    }
}

public enum NodeDirectionType
{
    None,
    UpLeft,
    Up,
    UpRight,
    Left,
    Right,
    DownLeft,
    Down,
    DownRight
}
