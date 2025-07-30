using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class TowerVisual : NhoxBehaviour
{
    protected Enemy myEnemy;

    [Header("Glow Effect")] [SerializeField]
    protected MeshRenderer meshRenderer;

    [SerializeField] protected float maxIntensity = 200f;
    [SerializeField] protected Color startColor = new Color(0f, 0f, 0f, 255f);
    [SerializeField] protected Color endColor = new Color(0x04 / 255f, 0x6E / 255f, 0xFF / 255f, 1f);

    [Header("Attack Visual")] [SerializeField]
    protected float attackVisualDuration = 0.1f;

    [SerializeField] protected string onHitFX;

    protected Material material;
    protected float currentIntensity;

    protected override void Awake()
    {
        base.Awake();
        CloneAndAssignMaterial();
        StartCoroutine(ChangeEmissionIntensity(1f));
    }

    protected virtual void Update()
    {
        UpdateEmissionColor();
    }

    #region LoadComponents

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadMeshRenderer();
    }

    protected abstract void LoadMeshRenderer();

    #endregion

    protected void CloneAndAssignMaterial()
    {
        material = new Material(meshRenderer.material);
        meshRenderer.material = material;
    }

    protected void UpdateEmissionColor()
    {
        Color emissionColor = Color.Lerp(startColor, endColor, currentIntensity / maxIntensity);
        emissionColor *= Mathf.LinearToGammaSpace(currentIntensity);
        material.SetColor("_EmissionColor", emissionColor);
    }

    public virtual void ReloadVFX(float duration)
    {
        StartCoroutine(ChangeEmissionIntensity(duration / 2));
    }

    public abstract void PlayAttackVFX(Vector3 startPoint, Vector3 endPoint, Enemy newEnemy);

    protected virtual IEnumerator VFXCoroutine(Vector3 startPoint, Vector3 endPoint, Enemy newEnemy)
    {
        myEnemy = newEnemy;
        yield break;
    }

    protected IEnumerator ChangeEmissionIntensity(float duration)
    {
        float startTime = Time.time;
        float startIntensity = 0f;

        while (Time.time - startTime < duration)
        {
            currentIntensity = Mathf.Lerp(startIntensity, maxIntensity, (Time.time - startTime) / duration);
            yield return null;
        }

        currentIntensity = maxIntensity;
    }

    //Note: Spawn on shield of heavy enemy kinda weird
    public void CreateOnHitFX(Vector3 hitPoint)
    {
         FXSpawner.Instance.Spawn(onHitFX, hitPoint, Random.rotation);
    }
}