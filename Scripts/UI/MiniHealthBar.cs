using System;
using UnityEngine;

public class MiniHealthBar : MonoBehaviour
{
    private Entity entity;

    private void Awake()
    {
        entity = GetComponentInParent<Entity>();
    }

    private void OnEnable()
    {
        if (entity != null)
            entity.OnFlipped += HandleFlip;
    }

    private void OnDisable()
    {
        if (entity != null)
            entity.OnFlipped -= HandleFlip;
    }

    private void HandleFlip() => transform.rotation = Quaternion.identity;
}
