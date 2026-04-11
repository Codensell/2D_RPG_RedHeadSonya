using UnityEngine;
using System.Collections;

public class PlayerVFX : Entity_VFX
{
    [Header("Image Clone VFX")] 
    [Range(0.01f, 0.2f)] 
    [SerializeField] private float imageCloneInterval = .05f;
    [SerializeField] private GameObject imageClonePrefab;
    private Coroutine _imageCloneCo;

    public void DoImageCloneEffect(float duration)
    {
        if(_imageCloneCo != null)StopCoroutine(_imageCloneCo);
        _imageCloneCo = StartCoroutine(ImageCloneEffectCo(duration));
    }
    private IEnumerator ImageCloneEffectCo(float duration)
    {
        float time = 0;
        while (time < duration)
        {
            CreateImageClone();
            yield return new WaitForSeconds(imageCloneInterval);
            time += imageCloneInterval;
        }
    }

    private void CreateImageClone()
    {
        GameObject imageClone = Instantiate(imageClonePrefab, transform.position, transform.rotation);
        imageClone.GetComponentInChildren<SpriteRenderer>().sprite = sr.sprite;
    }
}
