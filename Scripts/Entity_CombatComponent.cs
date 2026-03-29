using UnityEngine;

public class Entity_CombatComponent : MonoBehaviour
{
    [Header("Target detection")]
    [SerializeField] private Transform targetCheck;
    [SerializeField] private LayerMask whatIsTarget;
    [SerializeField] private float targetCheckRadius;

    public void PerformAttack()
    {
        foreach (var collider in GetDetectedColliders())
        {
            Debug.Log("Performed" + collider.name);
        }
    }

    private Collider2D[] GetDetectedColliders ()
    {
        return Physics2D.OverlapCircleAll(targetCheck.position, targetCheckRadius, whatIsTarget);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(targetCheck.position, targetCheckRadius);
    }
}
