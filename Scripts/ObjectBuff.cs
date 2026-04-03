using UnityEngine;
using System.Collections;

public class ObjectBuff : MonoBehaviour
{
    private SpriteRenderer _sr;
    
    [Header("Buff Details")] 
    [SerializeField] private float buffDuration = 4f;
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

        StartCoroutine(BuffCo(buffDuration));
    }

    private IEnumerator BuffCo(float duration)
    {
        canBeUsed = false;
        _sr.color = Color.clear;
        
        yield return new WaitForSeconds(duration);
        
        Destroy(gameObject);
    }
}
