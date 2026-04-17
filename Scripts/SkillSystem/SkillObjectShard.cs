using System;
using UnityEngine;

public class SkillObjectShard : SkillObjectBase
{
    [SerializeField] private GameObject vfxPrefab;

    public void SetUpShard(float detanationTime)
    {
        Invoke(nameof(Explode), detanationTime);
    }
    private void Explode()
    {
        damageEnemiesInRadius(transform, checkRadius);
        Instantiate(vfxPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() == null)
            return;

        Explode();
    }
}
