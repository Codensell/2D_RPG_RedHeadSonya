using UnityEngine;
using System.Collections;

public class Entity_VFX : MonoBehaviour
{
    private SpriteRenderer sr;
    private Entity entity;
    
    [Header("VFX")] 
    [SerializeField] public Material onDamageMaterial;
    [SerializeField] public float onDamageVfxDuration;
    private Material originalMaterial;
    private Coroutine onDamageVfxCoroutine;

    [Header("On Damage Done VFX")]
    [SerializeField] private Color hitVfxColor = Color.white;
    [SerializeField] private GameObject hitVfx;
    [SerializeField] private GameObject critHitVfx;
    private void Awake()
    {
        entity = GetComponent<Entity>();
        sr = GetComponentInChildren<SpriteRenderer>();
        originalMaterial = sr.material;
    }

    public void CreateOnHitVFX(Transform target, bool isCrit)
    {
        GameObject hitPrefab = isCrit ? critHitVfx : hitVfx;
        GameObject vfx = Instantiate(hitVfx, target.position, Quaternion.identity);
        vfx.GetComponentInChildren<SpriteRenderer>().color = hitVfxColor;

        if (entity.facingDirection == -1 && isCrit)
        {
            vfx.transform.Rotate(0, 180, 0);
        }
    }

    public void PlayOnDamageVfx()
    {
        if(onDamageVfxCoroutine != null)
            StopCoroutine(onDamageVfxCoroutine);
            
        onDamageVfxCoroutine = StartCoroutine(OnDamageVfxCo());
    }

    private IEnumerator OnDamageVfxCo()
    {
        sr.material = onDamageMaterial;
        yield return new WaitForSeconds(onDamageVfxDuration);
        sr.material = originalMaterial;
    }
    
}
