using UnityEngine;
using System.Collections;

public class Entity_VFX : MonoBehaviour
{
    private SpriteRenderer _sr;
    private Entity _entity;
    
    [Header("VFX")] 
    [SerializeField] public Material onDamageMaterial;
    [SerializeField] public float onDamageVfxDuration;
    private Material _originalMaterial;
    private Coroutine _onDamageVfxCoroutine;

    [Header("On Damage Done VFX")]
    [SerializeField] private Color hitVfxColor = Color.white;
    [SerializeField] private GameObject hitVfx;
    [SerializeField] private GameObject critHitVfx;
    
    [Header("Elemental Colors")]
    [SerializeField] private Color chillVfx = Color.cyan;
    [SerializeField] private Color burnVfx = Color.red;
    private Color _originalHitVfxColor;
    
    private void Awake()
    {
        _entity = GetComponent<Entity>();
        _sr = GetComponentInChildren<SpriteRenderer>();
        _originalMaterial = _sr.material;
        _originalHitVfxColor = hitVfxColor;
    }

    public void PlayOnStatusVfx(float duration, ElementalType elemental)
    {
        if(elemental == ElementalType.Ice)StartCoroutine(PlayStatusVfxCo(duration, chillVfx));
        if(elemental == ElementalType.Fire)StartCoroutine(PlayStatusVfxCo(duration, burnVfx));
    }

    private IEnumerator PlayStatusVfxCo(float duration, Color effectColor)
    {
        float tickInterval = .25f;
        float timePassed = 0;

        Color lightColor = effectColor * 1.2f;
        Color darkColor = effectColor * 1.2f;

        bool toggle = false;

        while (timePassed < duration)
        {
            _sr.color = toggle ? lightColor : darkColor;
            toggle = !toggle;
            
            yield return new WaitForSeconds(tickInterval);
            timePassed += tickInterval;
        }
        
        _sr.color = Color.white;
    }

    public void CreateOnHitVFX(Transform target, bool isCrit)
    {
        GameObject hitPrefab = isCrit ? critHitVfx : hitVfx;
        GameObject vfx = Instantiate(hitVfx, target.position, Quaternion.identity);
        vfx.GetComponentInChildren<SpriteRenderer>().color = hitVfxColor;

        if (_entity.facingDirection == -1 && isCrit)
        {
            vfx.transform.Rotate(0, 180, 0);
        }
    }

    public void UpdateOnHitColor(ElementalType elemental)
    {
        if (elemental == ElementalType.Ice)hitVfxColor = chillVfx;
        
        if(elemental == ElementalType.None)hitVfxColor = _originalHitVfxColor;
    }

    public void PlayOnDamageVfx()
    {
        if(_onDamageVfxCoroutine != null)
            StopCoroutine(_onDamageVfxCoroutine);
            
        _onDamageVfxCoroutine = StartCoroutine(OnDamageVfxCo());
    }

    private IEnumerator OnDamageVfxCo()
    {
        _sr.material = onDamageMaterial;
        yield return new WaitForSeconds(onDamageVfxDuration);
        _sr.material = _originalMaterial;
    }
    
}
