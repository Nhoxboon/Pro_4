using System.Collections;
using UnityEngine;

public class CrossbowVisual : TowerVisual
{
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
        CloneStringMaterial();
    }

    protected override void Update()
    {
        base.Update();
        UpdateString();
        UpdateAttackVisuals();
    }

    #region LoadComponents

    protected override void LoadComponents()
    {
        base.LoadComponents();
        onHitFX = "Crossbow_OnHitFX";
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

    protected override void LoadMeshRenderer()
    {
        if (meshRenderer != null) return;
        meshRenderer = transform.parent.Find("Model/CrossbowTower/TowerHead/tower_crossbow_emissionPart_2")
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
        rotor = transform.parent.Find("Model/CrossbowTower/TowerHead/tower_crossbow_head_rotor");
        rotorUnloaded = transform.parent.Find("Model/CrossbowTower/TowerHead/RotorPositionUnloaded");
        rotorLoaded = transform.parent.Find("Model/CrossbowTower/TowerHead/RotorPositionLoaded");
        DebugTool.Log(transform.name + " :LoadRotorVisual", gameObject);
    }

    protected void LoadFrontStringLeftVisual()
    {
        if (frontStringL != null) return;
        frontStringL = transform.parent.Find("Model/CrossbowTower/TowerHead/tower_visuals_strings/front_string_L")
            .GetComponent<LineRenderer>();
        DebugTool.Log(transform.name + " :LoadStringLeftVisual", gameObject);
    }

    protected void LoadFrontStringRightVisual()
    {
        if (frontStringR != null) return;
        frontStringR = transform.parent.Find("Model/CrossbowTower/TowerHead/tower_visuals_strings/front_string_R")
            .GetComponent<LineRenderer>();
        DebugTool.Log(transform.name + " :LoadStringRightVisual", gameObject);
    }

    protected void LoadBackStringLeftVisual()
    {
        if (backStringL != null) return;
        backStringL = transform.parent.Find("Model/CrossbowTower/TowerHead/tower_visuals_strings/back_string_L")
            .GetComponent<LineRenderer>();
        DebugTool.Log(transform.name + " :LoadBackStringLeftVisual", gameObject);
    }

    protected void LoadBackStringRightVisual()
    {
        if (backStringR != null) return;
        backStringR = transform.parent.Find("Model/CrossbowTower/TowerHead/tower_visuals_strings/back_string_R")
            .GetComponent<LineRenderer>();
        DebugTool.Log(transform.name + " :LoadBackStringRightVisual", gameObject);
    }

    protected void LoadFrontStartPointLeft()
    {
        if (frontStartPointL != null) return;
        frontStartPointL = transform.parent
            .Find("Model/CrossbowTower/TowerHead/tower_visuals_strings/Point/front_start_point_L")
            .GetComponent<Transform>();
        DebugTool.Log(transform.name + " :LoadFrontStartPointLeft", gameObject);
    }

    protected void LoadFrontEndPointLeft()
    {
        if (frontEndPointL != null) return;
        frontEndPointL = transform.parent
            .Find("Model/CrossbowTower/TowerHead/tower_crossbow_head_rotor/Point/front_end_point_L")
            .GetComponent<Transform>();
        DebugTool.Log(transform.name + " :LoadFrontEndPointLeft", gameObject);
    }

    protected void LoadFrontStartPointRight()
    {
        if (frontStartPointR != null) return;
        frontStartPointR = transform.parent
            .Find("Model/CrossbowTower/TowerHead/tower_visuals_strings/Point/front_start_point_R")
            .GetComponent<Transform>();
        DebugTool.Log(transform.name + " :LoadFrontStartPointRight", gameObject);
    }

    protected void LoadFrontEndPointRight()
    {
        if (frontEndPointR != null) return;
        frontEndPointR = transform.parent
            .Find("Model/CrossbowTower/TowerHead/tower_crossbow_head_rotor/Point/front_end_point_R")
            .GetComponent<Transform>();
        DebugTool.Log(transform.name + " :LoadFrontEndPointRight", gameObject);
    }

    protected void LoadBackStartPointLeft()
    {
        if (backStartPointL != null) return;
        backStartPointL = transform.parent
            .Find("Model/CrossbowTower/TowerHead/tower_visuals_strings/Point/back_start_point_L")
            .GetComponent<Transform>();
        DebugTool.Log(transform.name + " :LoadBackStartPointLeft", gameObject);
    }

    protected void LoadBackEndPointLeft()
    {
        if (backEndPointL != null) return;
        backEndPointL = transform.parent
            .Find("Model/CrossbowTower/TowerHead/tower_crossbow_head_rotor/Point/back_end_point_L")
            .GetComponent<Transform>();
        DebugTool.Log(transform.name + " :LoadBackEndPointLeft", gameObject);
    }

    protected void LoadBackStartPointRight()
    {
        if (backStartPointR != null) return;
        backStartPointR = transform.parent
            .Find("Model/CrossbowTower/TowerHead/tower_visuals_strings/Point/back_start_point_R")
            .GetComponent<Transform>();
        DebugTool.Log(transform.name + " :LoadBackStartPointRight", gameObject);
    }

    protected void LoadBackEndPointRight()
    {
        if (backEndPointR != null) return;
        backEndPointR = transform.parent
            .Find("Model/CrossbowTower/TowerHead/tower_crossbow_head_rotor/Point/back_end_point_R")
            .GetComponent<Transform>();
        DebugTool.Log(transform.name + " :LoadBackEndPointRight", gameObject);
    }

    #endregion

    protected void CloneStringMaterial()
    {
        foreach (var sR in stringRenderers)
        {
            sR.material = material;
        }
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
        if (attackVisual.enabled && myEnemy is not null)
            attackVisual.SetPosition(1, myEnemy.GetCenterPoint());
    }

    protected void UpdateStringVisuals(LineRenderer lineRenderer, Transform startPoint, Transform endPoint)
    {
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, startPoint.position);
        lineRenderer.SetPosition(1, endPoint.position);
    }

    public override void ReloadVFX(float duration)
    {
        base.ReloadVFX(duration);
        StartCoroutine(UpdateRotorPosition(duration / 2));
    }

    public override void PlayAttackVFX(Vector3 startPoint, Vector3 endPoint, Enemy newEnemy)
    {
        StartCoroutine(VFXCoroutine(startPoint, endPoint, newEnemy));
    }

    protected override IEnumerator VFXCoroutine(Vector3 startPoint, Vector3 endPoint, Enemy newEnemy)
    {
        yield return base.VFXCoroutine(startPoint, endPoint, newEnemy);
        attackVisual.enabled = true;
        attackVisual.SetPosition(0, startPoint);
        attackVisual.SetPosition(1, endPoint);

        yield return new WaitForSeconds(attackVisualDuration);
        attackVisual.enabled = false;
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
}