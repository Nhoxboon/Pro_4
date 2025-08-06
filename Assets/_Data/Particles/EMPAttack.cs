using UnityEngine;

public class EMPAttack : NhoxBehaviour
{
    [SerializeField] protected string empFX = "TowerNotActiveFX";
    [SerializeField] protected float moveSpeed = 1f;
    [SerializeField] protected float empRadius = 2f;
    [SerializeField] protected float empEffectDuration = 2f;

    protected Vector3 destination;
    protected float shrinkSpeed = 3f;
    protected bool shouldShrink;

    private Vector3 originalScale;

    protected override void Awake()
    {
        base.Awake();
        originalScale = transform.localScale;
    }

    protected void OnEnable() => ResetEMP();

    protected void Update()
    {
        MoveToTarget();
        if (shouldShrink)
            Shrink();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out TowerCtrl tower))
            tower.Status.DeactivateTower(empEffectDuration, empFX);
    }

    public void SetUpEMP(float duration, Vector3 newTarget, float empDuration)
    {
        empEffectDuration = duration;
        destination = newTarget;
        // Invoke(nameof(DeactivateEMP), atkDuration);
    }

    protected void MoveToTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, destination) < 0.1f)
            DeactivateEMP();
    }

    protected void DeactivateEMP() => shouldShrink = true;

    protected void Shrink()
    {
        transform.localScale -= Vector3.one * (shrinkSpeed * Time.deltaTime);
        if (transform.localScale.x <= 0.01f) ProjectileSpawner.Instance.Despawn(gameObject);
    }

    protected void ResetEMP()
    {
        shouldShrink = false;
        transform.localScale = originalScale;
        destination = transform.position;
    }

    private void OnDrawGizmos() => Gizmos.DrawWireSphere(transform.position, empRadius);
}