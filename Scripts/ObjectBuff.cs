using UnityEngine;
using System.Collections;

[System.Serializable]
public class Buff
{
    public StatType type;
    public float value;
}
public class ObjectBuff : MonoBehaviour
{
    private SpriteRenderer _sr;
    private EntityStats _statsToModify;

    [Header("Buff Details")]
    [SerializeField] private Buff[] buffs;
    [SerializeField] private float buffDuration = 4f;
    [SerializeField] private string buffName;
    [SerializeField] private bool canBeUsed = true;
    
    [Header("Floating movement")]
    [SerializeField] private float floatspeed = 1f;
    [SerializeField] private float floatRange = .1f;
    private Vector3 _startPosition;

    private void Awake()
    {
        _sr = GetComponentInChildren<SpriteRenderer>();
        _startPosition = transform.position;
    }

    private void Update()
    {
        var yOffset = Mathf.Sin(Time.time * floatspeed) * floatRange;
        transform.position = _startPosition + new Vector3(0, yOffset);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (canBeUsed == false) return;

        _statsToModify = collision.GetComponent<EntityStats>();
        StartCoroutine(BuffCo(buffDuration));
    }

    private IEnumerator BuffCo(float duration)
    {
        canBeUsed = false;
        _sr.color = Color.clear;

        ApplyBuff(true);
        
        yield return new WaitForSeconds(duration);
        
        ApplyBuff(false);
        
        Destroy(gameObject);
    }

    private void ApplyBuff(bool applied)
    {
        foreach (var buff in buffs)
        {
            if(applied)
                _statsToModify.GetsStatByType(buff.type).AddModifier(buff.value, buffName);
            else 
                _statsToModify.GetsStatByType(buff.type).RemoveModifier(buffName);
        }
    }
}
