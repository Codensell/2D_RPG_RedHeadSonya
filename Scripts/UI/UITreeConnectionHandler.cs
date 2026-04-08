using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class UIConnectionDetails
{
    public UITreeConnectionHandler childNode;
    public NodeDirectionType directionType;
    [Range(100f,350f)] public float length;
    [Range(-25f, 25f)] public float rotation;
}
public class UITreeConnectionHandler : MonoBehaviour
{
    private RectTransform _rect => GetComponent<RectTransform>();
    private Image _connectionImage;
    private Color _originalColor;
    
    [SerializeField] private UIConnectionDetails[] details;
    [SerializeField] private UITreeConnection[] connections;

    private void Awake()
    {
        if (_connectionImage != null) _originalColor = _connectionImage.color;
    }

    private void OnValidate()
    {
        if(details.Length <= 0) return;
        if (details.Length != connections.Length)
        {
            Debug.Log("Details and Connections are not the same length!" + gameObject.name);
            return;
        }
        UpdateConnections();
    }
    public void UpdateConnections()
    {
        
        for (int i = 0; i < details.Length; i++)
        {
            var detail = details[i];
            var connection = connections[i];
            
            Vector2 targetPosition = connection.GetConnectionPoint(_rect);
            Image connectionImage = connection.GetConnectionImage();
            
            connection.DirectionConnection(detail.directionType, detail.length, detail.rotation);
            detail.childNode?.SetPosition(targetPosition);
            detail.childNode?.SetConnectionImage(connectionImage);
            detail.childNode?.transform.SetAsLastSibling();
        }
    }

    public void UpdateAllConnection()
    {
        UpdateConnections();
        foreach (var detail in details)
        {
            if (detail.childNode == null) continue;
            detail.childNode.UpdateConnections();
        }
    }

    public void UnlockConnectionImage(bool unlocked)
    {
        if (_connectionImage == null) return;
        
        _connectionImage.color = unlocked ? Color.white :  _originalColor;
    }

    public void SetConnectionImage(Image image) => _connectionImage = image;
    public void SetPosition(Vector2 position) => _rect.anchoredPosition = position;
    
}
