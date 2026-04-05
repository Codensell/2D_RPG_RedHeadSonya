using System;
using UnityEngine;

[Serializable]
public class UIConnectionDetails
{
    public NodeDirectionType directionType;
    [Range(100f,350f)] public float length;
}
public class UITreeConnectionHandler : MonoBehaviour
{
    [SerializeField] private UIConnectionDetails[] details;
    [SerializeField] private UITreeConnection[] connections;

    private void OnValidate()
    {
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
        }
    }
    
}
