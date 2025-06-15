using System;
using System.Collections;
using UnityEngine;

public class CrossbowVisual : NhoxBehaviour
{
    [SerializeField] protected CrossbowTower tower;
    
    [SerializeField] protected LineRenderer attackVisual;
    [SerializeField] protected float attackVisualDuration = 0.1f;

    [Header("Glow Effect")]
    [SerializeField] protected MeshRenderer meshRenderer;
    protected Material material;
    protected float currentIntensity;
    [SerializeField] protected float maxIntensity = 150f;
    [SerializeField] protected Color startColor;
    [SerializeField] protected Color endColor;

    protected override void Awake()
    {
        base.Awake();
        CloneAndAssignMaterial();
        StartCoroutine(ChangeEmissionIntensity(1f));
    }

    protected void Update()
    {
        UpdateEmissionColor();
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadTower();
        LoadAttackVisual();
        LoadMeshRenderer();
    }
    
    protected void LoadTower()
    {
        if (tower != null) return;
        tower = GetComponentInParent<CrossbowTower>();
        Debug.Log(transform.name + " :LoadTower", gameObject);
    }

    protected void LoadAttackVisual()
    {
        if (attackVisual != null) return;
        attackVisual = GetComponentInChildren<LineRenderer>();
        Debug.Log(transform.name + " :LoadAttackVisual", gameObject);
    }

    protected void LoadMeshRenderer()
    {
        if(meshRenderer != null) return;
        meshRenderer = transform.parent.Find("Model/CrossbowTower/TowerHead/tower_crossbow_emissionPart_2")
            .GetComponent<MeshRenderer>();
        Debug.Log(transform.name + " :LoadMeshRenderer", gameObject);
    }

    protected void CloneAndAssignMaterial()
    {
        material = new Material(meshRenderer.material);
        meshRenderer.material = material;
    }

    public void PlayAttackVFX(Vector3 startPoint, Vector3 endPoint)
    {
        StartCoroutine(FXCoroutine(startPoint, endPoint));
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

    protected void UpdateEmissionColor()
    {
        Color emissionColor = Color.Lerp(startColor, endColor, currentIntensity / maxIntensity);
        emissionColor *= Mathf.LinearToGammaSpace(currentIntensity);
        material.SetColor("_EmissionColor", emissionColor);
    }

    public void ReloadFX(float duration)
    {
        StartCoroutine(ChangeEmissionIntensity(duration / 2));
    }

    private IEnumerator ChangeEmissionIntensity(float duration)
    {
        float startTime = Time.time;
        float startIntensity = 0f;

        while (Time.time - startTime < duration)
        {
            //Calculates the proportion of the duration that has elapsed since the start of coroutine
            currentIntensity = Mathf.Lerp(startIntensity, maxIntensity, (Time.time - startTime) / duration);
            yield return null;
        }
        currentIntensity = maxIntensity;
    }
}