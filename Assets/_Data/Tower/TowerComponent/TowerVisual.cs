using System.Collections;
using UnityEngine;

public abstract class TowerVisual : TowerComponent
{
    protected void SpawnVFX(string fxName, Vector3 position, Quaternion rotation)
    {
        var vfx = FXSpawner.Instance.Spawn(fxName, position, rotation);
        vfx.gameObject.SetActive(true);
    }

    public IEnumerator ChangeScaleCoroutine(Transform transform, float newScale, float duration = 0.25f)
    {
        float time = 0f;
        Vector3 initialScale = transform.localScale;
        Vector3 targetScale = new Vector3(1, newScale, 1);

        while (time < duration)
        {
            transform.localScale = Vector3.Lerp(initialScale, targetScale, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale;
    }
    
    public virtual void ResetVisual() => StopAllCoroutines();
}