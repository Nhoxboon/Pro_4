using System;
using System.Collections;
using UnityEngine;

public abstract class TowerVisual : NhoxBehaviour
{
    [SerializeField] protected Tower tower;

    [Header("Glow Effect")]
    [SerializeField] protected MeshRenderer meshRenderer;
    [SerializeField] protected float maxIntensity = 150f;
    [SerializeField] protected Color startColor = new Color(0f, 0f, 0f, 255f);
    [SerializeField] protected Color endColor = new Color(0x04 / 255f, 0x6E / 255f, 0xFF / 255f, 1f);

    [Header("Attack Visual")]
    [SerializeField] protected LineRenderer attackVisual;
    [SerializeField] protected float attackVisualDuration = 0.1f;

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
        LoadTower();
        LoadMeshRenderer();
        LoadAttackVisual();
    }

    protected void LoadTower()
    {
        if (tower != null) return;
        tower = GetComponentInParent<Tower>();
        Debug.Log(transform.name + " :LoadTower", gameObject);
    }

    protected abstract void LoadMeshRenderer();

    protected void LoadAttackVisual()
    {
        if (attackVisual != null) return;
        attackVisual = GetComponentInChildren<LineRenderer>();
        Debug.Log(transform.name + " :LoadAttackVisual", gameObject);
    }
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

    public virtual void ReloadFX(float duration)
    {
        StartCoroutine(ChangeEmissionIntensity(duration / 2));
    }

    public virtual void PlayAttackVFX(Vector3 startPoint, Vector3 endPoint)
    {
        StartCoroutine(FXCoroutine(startPoint, endPoint));
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

    private IEnumerator FXCoroutine(Vector3 startPoint, Vector3 endPoint)
    {
        tower.EnableRotation(false);
        attackVisual.enabled = true;
        attackVisual.SetPosition(0, startPoint);
        attackVisual.SetPosition(1, endPoint);

        yield return new WaitForSeconds(attackVisualDuration);
        attackVisual.enabled = false;
        tower.EnableRotation(true);
    }
}