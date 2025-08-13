using System.Collections;
using UnityEngine;

public class CrossbowVisuals : TowerVisuals
{
    [Header("Glow Effect")] [SerializeField]
    protected MeshRenderer meshRenderer;

    [SerializeField] protected float maxIntensity = 200f;
    [SerializeField] protected Color startColor = new Color(0f, 0f, 0f, 255f);
    [SerializeField] protected Color endColor = new Color(0x04 / 255f, 0x6E / 255f, 0xFF / 255f, 1f);

    [Header("Attack Visual")] [SerializeField]
    protected float attackVisualDuration = 0.1f;

    protected string onHitFX = "Crossbow_OnHitVFX";
    protected Vector3 hitPoint;

    protected Material material;
    protected float currentIntensity;

    [SerializeField] protected LineRenderer attackVisual;

    [Header("Rotor Visual")] [SerializeField]
    protected Transform rotor;

    [SerializeField] protected Transform rotorUnloaded;
    [SerializeField] protected Transform rotorLoaded;

    [Header("Front Glow String")] [SerializeField]
    protected LineRenderer frontStringL;

    [SerializeField] protected LineRenderer frontStringR;
    [SerializeField] protected Transform frontStartPointL;
    [SerializeField] protected Transform frontEndPointL;
    [SerializeField] protected Transform frontStartPointR;
    [SerializeField] protected Transform frontEndPointR;

    [Header("Back Glow String")] [SerializeField]
    protected LineRenderer backStringL;

    [SerializeField] protected LineRenderer backStringR;
    [SerializeField] protected Transform backStartPointL;
    [SerializeField] protected Transform backEndPointL;
    [SerializeField] protected Transform backStartPointR;
    [SerializeField] protected Transform backEndPointR;

    [SerializeField] protected LineRenderer[] stringRenderers;

    protected override void Awake()
    {
        base.Awake();
        CloneAndAssignMaterial();
        StartCoroutine(ChangeEmissionIntensity(1f));
        CloneStringMaterial();
    }

    protected void Update()
    {
        UpdateEmissionColor();
        UpdateString();
        UpdateAttackVisuals();
    }

    #region LoadComponents

    protected override void LoadComponents()
    {
        base.LoadComponents();

        LoadMeshRenderer();
        LoadAttackVisual();
        LoadRotorVisual();

        LoadFrontStringLeftVisual();
        LoadFrontStringRightVisual();
        LoadFrontStartPointLeft();
        LoadFrontEndPointLeft();
        LoadFrontStartPointRight();
        LoadFrontEndPointRight();
        LoadBackStringLeftVisual();
        LoadBackStringRightVisual();
        LoadBackStartPointLeft();
        LoadBackEndPointLeft();
        LoadBackStartPointRight();
        LoadBackEndPointRight();
    }

    protected void LoadMeshRenderer()
    {
        if (meshRenderer != null) return;
        meshRenderer = transform.parent.parent.Find("Model/CrossbowTower/TowerHead/tower_crossbow_emissionPart_2")
            .GetComponent<MeshRenderer>();
        DebugTool.Log(transform.name + " :LoadMeshRenderer", gameObject);
    }

    protected void LoadAttackVisual()
    {
        if (attackVisual != null) return;
        attackVisual = GetComponentInChildren<LineRenderer>();
        DebugTool.Log(transform.name + " :LoadAttackVisual", gameObject);
    }

    protected void LoadRotorVisual()
    {
        if (rotor != null) return;
        rotor = towerCtrl.transform.Find("Model/CrossbowTower/TowerHead/tower_crossbow_head_rotor");
        rotorUnloaded = towerCtrl.transform.Find("Model/CrossbowTower/TowerHead/RotorPositionUnloaded");
        rotorLoaded = towerCtrl.transform.Find("Model/CrossbowTower/TowerHead/RotorPositionLoaded");
        DebugTool.Log(transform.name + " :LoadRotorVisual", gameObject);
    }

    protected void LoadFrontStringLeftVisual()
    {
        if (frontStringL != null) return;
        frontStringL = towerCtrl.transform.Find("Model/CrossbowTower/TowerHead/tower_visuals_strings/front_string_L")
            .GetComponent<LineRenderer>();
        DebugTool.Log(transform.name + " :LoadStringLeftVisual", gameObject);
    }

    protected void LoadFrontStringRightVisual()
    {
        if (frontStringR != null) return;
        frontStringR = towerCtrl.transform.Find("Model/CrossbowTower/TowerHead/tower_visuals_strings/front_string_R")
            .GetComponent<LineRenderer>();
        DebugTool.Log(transform.name + " :LoadStringRightVisual", gameObject);
    }

    protected void LoadBackStringLeftVisual()
    {
        if (backStringL != null) return;
        backStringL = towerCtrl.transform.Find("Model/CrossbowTower/TowerHead/tower_visuals_strings/back_string_L")
            .GetComponent<LineRenderer>();
        DebugTool.Log(transform.name + " :LoadBackStringLeftVisual", gameObject);
    }

    protected void LoadBackStringRightVisual()
    {
        if (backStringR != null) return;
        backStringR = towerCtrl.transform.Find("Model/CrossbowTower/TowerHead/tower_visuals_strings/back_string_R")
            .GetComponent<LineRenderer>();
        DebugTool.Log(transform.name + " :LoadBackStringRightVisual", gameObject);
    }

    protected void LoadFrontStartPointLeft()
    {
        if (frontStartPointL != null) return;
        frontStartPointL = towerCtrl.transform
            .Find("Model/CrossbowTower/TowerHead/tower_visuals_strings/Point/front_start_point_L")
            .GetComponent<Transform>();
        DebugTool.Log(transform.name + " :LoadFrontStartPointLeft", gameObject);
    }

    protected void LoadFrontEndPointLeft()
    {
        if (frontEndPointL != null) return;
        frontEndPointL = towerCtrl.transform
            .Find("Model/CrossbowTower/TowerHead/tower_crossbow_head_rotor/Point/front_end_point_L")
            .GetComponent<Transform>();
        DebugTool.Log(transform.name + " :LoadFrontEndPointLeft", gameObject);
    }

    protected void LoadFrontStartPointRight()
    {
        if (frontStartPointR != null) return;
        frontStartPointR = towerCtrl.transform
            .Find("Model/CrossbowTower/TowerHead/tower_visuals_strings/Point/front_start_point_R")
            .GetComponent<Transform>();
        DebugTool.Log(transform.name + " :LoadFrontStartPointRight", gameObject);
    }

    protected void LoadFrontEndPointRight()
    {
        if (frontEndPointR != null) return;
        frontEndPointR = towerCtrl.transform
            .Find("Model/CrossbowTower/TowerHead/tower_crossbow_head_rotor/Point/front_end_point_R")
            .GetComponent<Transform>();
        DebugTool.Log(transform.name + " :LoadFrontEndPointRight", gameObject);
    }

    protected void LoadBackStartPointLeft()
    {
        if (backStartPointL != null) return;
        backStartPointL = towerCtrl.transform
            .Find("Model/CrossbowTower/TowerHead/tower_visuals_strings/Point/back_start_point_L")
            .GetComponent<Transform>();
        DebugTool.Log(transform.name + " :LoadBackStartPointLeft", gameObject);
    }

    protected void LoadBackEndPointLeft()
    {
        if (backEndPointL != null) return;
        backEndPointL = towerCtrl.transform
            .Find("Model/CrossbowTower/TowerHead/tower_crossbow_head_rotor/Point/back_end_point_L")
            .GetComponent<Transform>();
        DebugTool.Log(transform.name + " :LoadBackEndPointLeft", gameObject);
    }

    protected void LoadBackStartPointRight()
    {
        if (backStartPointR != null) return;
        backStartPointR = towerCtrl.transform
            .Find("Model/CrossbowTower/TowerHead/tower_visuals_strings/Point/back_start_point_R")
            .GetComponent<Transform>();
        DebugTool.Log(transform.name + " :LoadBackStartPointRight", gameObject);
    }

    protected void LoadBackEndPointRight()
    {
        if (backEndPointR != null) return;
        backEndPointR = towerCtrl.transform
            .Find("Model/CrossbowTower/TowerHead/tower_crossbow_head_rotor/Point/back_end_point_R")
            .GetComponent<Transform>();
        DebugTool.Log(transform.name + " :LoadBackEndPointRight", gameObject);
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

    protected void CloneStringMaterial()
    {
        foreach (var sR in stringRenderers)
            sR.material = material;
    }

    protected void UpdateString()
    {
        UpdateStringVisuals(frontStringL, frontStartPointL, frontEndPointL);
        UpdateStringVisuals(frontStringR, frontStartPointR, frontEndPointR);

        UpdateStringVisuals(backStringL, backStartPointL, backEndPointL);
        UpdateStringVisuals(backStringR, backStartPointR, backEndPointR);
    }

    protected void UpdateAttackVisuals()
    {
        if (attackVisual.enabled && hitPoint != Vector3.zero)
            attackVisual.SetPosition(1, hitPoint);
    }

    protected void UpdateStringVisuals(LineRenderer lineRenderer, Transform startPoint, Transform endPoint)
    {
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, startPoint.position);
        lineRenderer.SetPosition(1, endPoint.position);
    }

    public void ReloadVFX(float duration)
    {
        StartCoroutine(ChangeEmissionIntensity(duration / 2));
        StartCoroutine(UpdateRotorPosition(duration / 2));
    }

    public void PlayAttackVFX(Vector3 startPoint, Vector3 endPoint) =>
        StartCoroutine(VFXCoroutine(startPoint, endPoint));

    protected IEnumerator VFXCoroutine(Vector3 startPoint, Vector3 endPoint)
    {
        hitPoint = endPoint;

        attackVisual.enabled = true;
        attackVisual.SetPosition(0, startPoint);
        attackVisual.SetPosition(1, endPoint);

        yield return new WaitForSeconds(attackVisualDuration);
        attackVisual.enabled = false;
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

    private IEnumerator UpdateRotorPosition(float duration)
    {
        float startTime = Time.time;
        while (Time.time - startTime < duration)
        {
            float tValue = (Time.time - startTime) / duration;
            rotor.position = Vector3.Lerp(rotorUnloaded.position, rotorLoaded.position, tValue);
            yield return null;
        }

        rotor.position = rotorLoaded.position;
    }

    public void CreateOnHitFX(Vector3 hitPoint) => SpawnVFX(onHitFX, hitPoint, Random.rotation);

    public override void ResetVisual()
    {
        base.ResetVisual();
        currentIntensity = 0f;
        rotor.position = rotorUnloaded.position;
        attackVisual.enabled = false;
    }
}