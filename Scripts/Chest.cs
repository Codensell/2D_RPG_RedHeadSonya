using UnityEngine;

public class Chest : MonoBehaviour, IDamagable
{
    private Rigidbody2D rb =>  GetComponentInChildren<Rigidbody2D>();
    private Animator anim => GetComponentInChildren<Animator>();
    private Entity_VFX vfx => GetComponent<Entity_VFX>();

    [Header("Chest details")] 
    [SerializeField] private Vector2 knockUp;
    public bool TakeDamage(float damage,float elementalDamage, ElementalType elemental, Transform damageDealer)
    {
        vfx.PlayOnDamageVfx();
        anim.SetBool("chestOpened", true);
        rb.linearVelocity = knockUp;

        rb.angularVelocity = Random.Range(-200, 200);

        return true;

        //drop items
    }
}
