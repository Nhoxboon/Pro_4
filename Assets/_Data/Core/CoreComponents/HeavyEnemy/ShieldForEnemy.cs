using System.Collections;
using UnityEngine;

public class ShieldForEnemy : CoreComponent
{
    [Header("Impact Details")] [SerializeField]
    protected Material shieldMaterial;

    [SerializeField] protected float defaultShieldGlow = 1f;
    [SerializeField] protected float impactShieldGlow = 3f;
    [SerializeField] protected float impactScaleMultiplier = 0.97f;
    [SerializeField] protected float impactSpeed = 0.1f;
    [SerializeField] protected float impactResetDuration = 0.5f;

    protected float defaultScale;
    protected string shieldFresnelParameter = "_FresnelPower";
    protected Coroutine currentActiveCoroutine;

    protected override void Awake()
    {
        base.Awake();
        defaultScale = transform.localScale.x;
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadShieldMaterial();
    }

    protected void LoadShieldMaterial()
    {
        if (shieldMaterial != null) return;
        shieldMaterial = GetComponent<MeshRenderer>().material;
        DebugTool.Log(transform.name + " :LoadShieldMaterial", gameObject);
    }

    public void DeactivateShield() => gameObject.SetActive(false);

    public void ShieldImpact()
    {
        if (!gameObject.activeInHierarchy) return;
        if (currentActiveCoroutine != null) StopCoroutine(currentActiveCoroutine);
        StartCoroutine(ImpactCoroutine());
    }

    private IEnumerator ImpactCoroutine()
    {
        yield return StartCoroutine(ShieldChangeCoroutine(impactShieldGlow, defaultScale * impactScaleMultiplier,
            impactSpeed));

        StartCoroutine(ShieldChangeCoroutine(defaultShieldGlow, defaultScale, impactResetDuration));
    }

    private IEnumerator ShieldChangeCoroutine(float targetGlow, float targetScale, float duration)
    {
        float time = 0f;
        float startGlow = shieldMaterial.GetFloat(shieldFresnelParameter);
        Vector3 initialScale = transform.localScale;
        Vector3 newScale = new Vector3(targetScale, targetScale, targetScale);

        while (time < duration)
        {
            transform.localScale = Vector3.Lerp(initialScale, newScale, time / duration);

            float newGlow = Mathf.Lerp(startGlow, targetGlow, time / duration);
            shieldMaterial.SetFloat(shieldFresnelParameter, newGlow);

            time += Time.deltaTime;
            yield return null;
        }

        transform.localScale = newScale;
        shieldMaterial.SetFloat(shieldFresnelParameter, targetGlow);
    }

    public void EnableShield()
    {
        if (core.Stats is HeavyEnemyStats stats && stats.ShieldAmount.CurrentValue > 0)
            gameObject.SetActive(true);
    }
}