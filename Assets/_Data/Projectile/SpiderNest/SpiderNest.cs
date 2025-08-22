using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(BoxCollider))]
public class SpiderNest : Projectile
{
    [SerializeField] protected BoxCollider col;
    [SerializeField] protected NavMeshAgent agent;

    protected Transform currentTarget;
    [SerializeField] protected float damageRadius = 0.8f;
    [SerializeField] protected float detonateDistance = 0.6f;
    [SerializeField] protected float enemyCheckRadius = 10f;
    [SerializeField] protected float targetUpdateInterval = 0.5f;
    [SerializeField] protected ProjectileDespawn despawn;
    [SerializeField] protected TrailRenderer tr;

    protected string explosionFX = "ExplosionFX_1";

    protected override void OnEnable()
    {
        base.OnEnable();
        InvokeRepeating(nameof(UpdateClosestTarget), 0.1f, targetUpdateInterval);
    }

    protected void OnDisable()
    {
        CancelInvoke(nameof(UpdateClosestTarget));
        DespawnWhenOutOfTime();
    }

    protected void Update()
    {
        if (!HasValidTarget()) return;

        MoveTowardsTarget();
        CheckDetonation();
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadBoxCollider();
        LoadNavMeshAgent();
        LoadDespawn();
        LoadTrailRenderer();
    }

    protected void LoadBoxCollider()
    {
        if (col != null) return;
        col = GetComponent<BoxCollider>();
        col.center = new Vector3(0, 0.05f, 0);
        col.size = new Vector3(0.2f, 0.1f, 0.2f);
        DebugTool.Log(transform.name + " :LoadBoxCollider", gameObject);
    }

    protected void LoadNavMeshAgent()
    {
        if (agent != null) return;
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = false;
        DebugTool.Log(transform.name + " :LoadNavMeshAgent", gameObject);
    }

    protected void LoadDespawn()
    {
        if (despawn != null) return;
        despawn = GetComponentInChildren<ProjectileDespawn>(true);
        DebugTool.Log(transform.name + " :LoadDespawn", gameObject);
    }

    protected void LoadTrailRenderer()
    {
        if (tr != null) return;
        tr = GetComponentInChildren<TrailRenderer>();
        DebugTool.Log(transform.name + " :LoadTrailRenderer", gameObject);
    }

    protected bool HasValidTarget() => currentTarget is not null &&
                                       currentTarget.gameObject.activeInHierarchy &&
                                       agent.enabled &&
                                       agent.isOnNavMesh;

    protected void MoveTowardsTarget()
    {
        if (agent.isOnNavMesh)
            agent.SetDestination(currentTarget.position);
    }

    protected void CheckDetonation()
    {
        if (Vector3.Distance(transform.position, currentTarget.position) <= detonateDistance && agent.enabled)
            Explode();
    }

    public void SetupSpider(float newDamage)
    {
        despawn.gameObject.SetActive(true);
        tr.Clear();
        ProjectileSpawner.Instance.BackToHolder(gameObject);
        agent.enabled = true;
        damage = newDamage;
    }

    protected void UpdateClosestTarget() => currentTarget = FindClosestEnemy();

    protected void DespawnWhenOutOfTime()
    {
        despawn.gameObject.SetActive(false);
        if (agent.enabled) agent.enabled = false;
    }

    protected void Explode()
    {
        DamageEnemies();
        SpawnOnHitFX();
        agent.enabled = false;
        ProjectileSpawner.Instance.Despawn(gameObject);
    }

    protected void DamageEnemies()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, damageRadius, whatIsEnemy);
        foreach (var coli in colliders)
        {
            if (!coli.TryGetComponent(out Enemy enemy)) continue;
            IDamageable damageable = enemy.Core.DamageReceiver;
            damageable.TakeDamage(damage);
        }
    }

    protected Transform FindClosestEnemy()
    {
        Collider[] enemiesAround = Physics.OverlapSphere(transform.position, enemyCheckRadius, whatIsEnemy);
        Transform closestEnemy = null;
        float shortestDistance = float.MaxValue;

        foreach (var enemyCol in enemiesAround)
        {
            float distance = Vector3.Distance(transform.position, enemyCol.transform.position);

            if (!(distance < shortestDistance)) continue;
            closestEnemy = enemyCol.transform;
            shortestDistance = distance;
        }

        return closestEnemy;
    }

    protected override void ResetProjectile() => currentTarget = null;

    protected override void SpawnOnHitFX()
    {
        Transform newFX = FXSpawner.Instance.Spawn(explosionFX, transform.position, Quaternion.identity);
        newFX.gameObject.SetActive(true);
    }
}