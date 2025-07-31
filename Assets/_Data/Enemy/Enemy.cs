using System.Collections;
using UnityEngine;

public class Enemy : NhoxBehaviour
{
    [SerializeField] protected EnemyType enemyType = EnemyType.None;
    [SerializeField] protected Transform centerPoint;

    protected EnemyPortal myPortal;
    public EnemyPortal MyPortal => myPortal;

    [SerializeField] protected Core core;
    public Core Core => core;

    protected bool canBeHidden = true;
    public bool isHidden;
    protected int originalLayerIndex;

    protected Coroutine hideCoroutine;

    protected override void Awake()
    {
        base.Awake();
        originalLayerIndex = gameObject.layer;
    }

    protected void OnEnable() => ResetEnemy();
    protected void OnDisable() => ResetEnemy();

    protected void Update() => core.LogicUpdate();

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadCore();
        LoadCenterPoint();
        LoadEnemyType();
    }

    protected void LoadCore()
    {
        if (core != null) return;
        core = transform.GetComponentInChildren<Core>();
        DebugTool.Log(transform.name + " LoadCore", gameObject);
    }

    protected void LoadCenterPoint()
    {
        if (centerPoint != null) return;
        centerPoint = transform.Find("CenterPoint");
        DebugTool.Log(transform.name + " :LoadCenterPoint", gameObject);
    }

    protected void LoadEnemyType()
    {
        if (enemyType != EnemyType.None) return;
        if (System.Enum.TryParse(transform.name, out EnemyType parsedType)) enemyType = parsedType;
        DebugTool.Log(transform.name + " :LoadEnemyType", gameObject);
    }

    public EnemyType GetEnemyType() => enemyType;
    public Vector3 GetCenterPoint() => centerPoint.position;
    public void SetPortal(EnemyPortal portal) => myPortal = portal;

    public void HideEnemy(float duration)
    {
        if (!canBeHidden || !gameObject.activeInHierarchy) return;

        if (hideCoroutine != null) StopCoroutine(hideCoroutine);
        hideCoroutine = StartCoroutine(HideEnemyCoroutine(duration));
    }

    protected IEnumerator HideEnemyCoroutine(float duration)
    {
        gameObject.layer = LayerMask.NameToLayer("Untargetable");
        core.Visuals.MakeTransparent(true);
        isHidden = true;

        yield return new WaitForSeconds(duration);
        gameObject.layer = originalLayerIndex;
        core.Visuals.MakeTransparent(false);
        isHidden = false;
    }

    public void ResetEnemy()
    {
        core.Movement.ResetMovement();
        core.Stats.Health.Init();
        if(core.SpawnUnit != null)
            core.SpawnUnit.ResetSpawnUnit();
    }
}