using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tower : NhoxBehaviour
{
    public Enemy currentTarget;

    [Tooltip("Enabling this allows tower to change target between attacks")] [SerializeField]
    protected bool dynamicTargetChange = true;

    [SerializeField] protected float attackCooldown = 2f;

    protected float lastAttackTime;
    protected bool towerActive = true;
    protected Coroutine deactivatedTowerCoroutine;
    protected Transform currentEMPFX;

    [Header("Tower Setup")] [SerializeField]
    protected Transform towerHead;

    [SerializeField] protected Transform gunPoint;
    [SerializeField] protected EnemyType enemyPriorityType = EnemyType.None;

    [Header("Rotation Settings")] protected bool canRotate = true;
    [SerializeField] protected float rotationSpeed = 10f;

    [Header("Attack Settings")] [SerializeField]
    protected float attackRange = 3.5f;

    public float AttackRange => attackRange;

    [SerializeField] protected LayerMask whatIsEnemy;
    [SerializeField] protected LayerMask whatIsTargetable;

    protected float targetCheckInterval = 0.1f;
    protected float lastTimeCheckedTarget;

    [Header("SFX Details")] [SerializeField]
    protected AudioSource attackSFX;

    protected Vector3 AttackCenter => transform.position - Vector3.up * 1.5f;

    protected virtual void Update()
    {
        LoseTarget();
        UpdateTarget();
        if (!towerActive) return;

        if (CanAttack())
            Attack();
        HandleRotation();
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadTowerHead();
        LoadEnemyLayerMask();
        LoadTargetLayerMask();
        LoadAttackSFX();
    }

    protected void LoadTowerHead()
    {
        if (towerHead != null) return;
        towerHead = transform.Find("Model/CrossbowTower/TowerHead");
        DebugTool.Log(transform.name + " :LoadTowerHead", gameObject);
    }

    protected void LoadEnemyLayerMask()
    {
        if (whatIsEnemy != 0) return;
        whatIsEnemy = LayerMask.GetMask("Enemy");
        DebugTool.Log(transform.name + " :LoadLayerMask", gameObject);
    }

    protected void LoadTargetLayerMask()
    {
        if (whatIsTargetable != 0) return;
        whatIsTargetable = LayerMask.GetMask("Default", "Enemy");
        DebugTool.Log(transform.name + " :LoadTargetLayerMask", gameObject);
    }

    protected void LoadAttackSFX()
    {
        if (attackSFX != null) return;
        attackSFX = GetComponentInChildren<AudioSource>();
        DebugTool.Log(transform.name + " :LoadAttackSFX", gameObject);
    }

    protected bool CanAttack() => Time.time > lastAttackTime + attackCooldown && currentTarget is not null;

    protected void LoseTarget()
    {
        if (currentTarget is null || !currentTarget.gameObject.activeInHierarchy) return;
        if (Vector3.Distance(AttackCenter, currentTarget.GetCenterPoint()) > attackRange)
            currentTarget = null;
    }

    protected void UpdateTarget()
    {
        if (currentTarget is null || !currentTarget.gameObject.activeInHierarchy)
        {
            currentTarget = FindRandomTargetWithinRange();
            return;
        }

        if (!dynamicTargetChange) return;

        if (Time.time > lastTimeCheckedTarget + targetCheckInterval)
        {
            lastTimeCheckedTarget = Time.time;
            currentTarget = FindRandomTargetWithinRange();
        }
    }

    protected virtual void Attack() => lastAttackTime = Time.time;

    public void DeactivateTower(float duration, string empFX)
    {
        if (deactivatedTowerCoroutine != null) StopCoroutine(deactivatedTowerCoroutine);

        if (currentEMPFX != null)
            FXSpawner.Instance.Despawn(currentEMPFX.gameObject);

        currentEMPFX =
            FXSpawner.Instance.Spawn(empFX, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
        currentEMPFX.gameObject.SetActive(true);
        deactivatedTowerCoroutine = StartCoroutine(DeactivateTowerCoroutine(duration));
    }

    private IEnumerator DeactivateTowerCoroutine(float duration)
    {
        towerActive = false;
        EnableRotation(false);
        yield return new WaitForSeconds(duration);
        EnableRotation(true);
        towerActive = true;
        lastAttackTime = Time.time;
        FXSpawner.Instance.Despawn(currentEMPFX.gameObject);
    }

    protected virtual void HandleRotation() => RotateTowardsTarget();

    protected virtual void RotateTowardsTarget()
    {
        if (!canRotate || towerHead is null || currentTarget is null ||
            !currentTarget.gameObject.activeInHierarchy) return;

        Vector3 dirToTarget = DirectionToTarget(towerHead);
        Quaternion lookRotation = Quaternion.LookRotation(dirToTarget);
        Vector3 rotation = Quaternion.Slerp(towerHead.rotation, lookRotation, rotationSpeed * Time.deltaTime)
            .eulerAngles;

        towerHead.rotation = Quaternion.Euler(rotation);
    }

    protected virtual Enemy FindRandomTargetWithinRange()
    {
        List<Enemy> priorityTargets = new List<Enemy>();
        List<Enemy> possibleTargets = new List<Enemy>();

        Collider[] enemies = Physics.OverlapSphere(AttackCenter, attackRange, whatIsEnemy);
        foreach (var enemy in enemies)
        {
            if (!enemy.TryGetComponent<Enemy>(out var newEnemy)) continue;

            (newEnemy.GetEnemyType() == enemyPriorityType ? priorityTargets : possibleTargets).Add(newEnemy);
        }

        return GetMostAdvancedEnemy(priorityTargets.Count > 0 ? priorityTargets : possibleTargets);
    }

    protected Enemy GetMostAdvancedEnemy(List<Enemy> targets)
    {
        Enemy mostAdvancedEnemy = null;
        float minRemainingDistance = float.MaxValue;

        foreach (Enemy enemy in targets)
        {
            if (!enemy.gameObject.activeInHierarchy) continue;

            float remainingDistance = enemy.Core.Movement.DistanceToFinishLine();
            if (remainingDistance < minRemainingDistance)
            {
                minRemainingDistance = remainingDistance;
                mostAdvancedEnemy = enemy;
            }
        }

        return mostAdvancedEnemy;
    }

    protected Vector3 DirectionToTarget(Transform startPoint)
    {
        if (currentTarget is null || !currentTarget.gameObject.activeInHierarchy) return Vector3.zero;
        return (currentTarget.GetCenterPoint() - startPoint.position).normalized;
    }

    public void EnableRotation(bool enable) => canRotate = enable;
    public void DestroyTower() => TowerSpawner.Instance.Despawn(gameObject);
    protected virtual void OnDrawGizmos() => Gizmos.DrawWireSphere(AttackCenter, attackRange);
}