using System;
using UnityEngine;

[Serializable]
public class UIConnectionDetails
{
    public UITreeConnectionHandler childNode;
    public NodeDirectionType directionType;
    [Range(100f,350f)] public float length;
}
public class UITreeConnectionHandler : MonoBehaviour
{
    private RectTransform _rect;
    [SerializeField] private UIConnectionDetails[] details;
    [SerializeField] private UITreeConnection[] connections;

    private void OnValidate()
    {
        if(_rect == null)_rect = GetComponent<RectTransform>();
        if (details.Length != connections.Length)
        {
            Debug.Log("Details and Connections are not the same length!" + gameObject.name);
            return;
        }
        UpdateConnections();
    }
    private void UpdateConnections()
    {
        
        for (int i = 0; i < details.Length; i++)
        {
            var detail = details[i];
            var connection = connections[i];
            connection.DirectionConnection(detail.directionType, detail.length);
            Vector2 targetPosition = connection.GetConnectionPoint(_rect);
            
            connection.DirectionConnection(detail.directionType, detail.length);
            detail.childNode.SetPosition(targetPosition);
        }
    }

    public void SetPosition(Vector2 position)
    {
        _rect.anchoredPosition = position;
    }
    
}
