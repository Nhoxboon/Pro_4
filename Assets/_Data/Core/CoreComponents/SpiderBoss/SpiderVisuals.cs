using UnityEngine;

public class SpiderVisuals : Visuals
{
    [Header("Leg Details")] public float legSpeed = 3f;
    public float increasedSpeedLeg = 10f;
    [SerializeField] protected SpiderLeg[] legs;

    [Header("Body Animation")]
    [SerializeField] protected float bodyAnimSpeed = 10f;
    [SerializeField] protected float maxHeight = 0.05f;

    protected Vector3 startPosition;
    protected float elapsedTime;

    [Header("Smoke animation")]
    [SerializeField] protected ParticleSystem[] smokeFXs;
    [SerializeField] protected float smokeCooldown = 2f;
    protected float smokeTimer;

    protected override void Start()
    {
        base.Start();
        startPosition = visuals.localPosition;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        AnimateBody();
        ActivateSmokeFX();
        UpdateSpiderLeg();
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadLegs();
        LoadSmokeFX();
    }

    protected void LoadLegs()
    {
        if (legs is { Length: > 0 }) return;
        legs = core.Root.GetComponentsInChildren<SpiderLeg>();
        DebugTool.Log(transform.name + " :LoadLegs", gameObject);
        foreach (var leg in legs)
            leg.SetSpiderVisuals(this);
    }

    protected void LoadSmokeFX()
    {
        if(smokeFXs is { Length: > 0 }) return;
        smokeFXs = visuals.GetComponentsInChildren<ParticleSystem>();
        DebugTool.Log(transform.name + " :LoadSmokeFX", gameObject);
    }

    protected void ActivateSmokeFX()
    {
        smokeTimer -= Time.deltaTime;

        if (!(smokeTimer < 0f)) return;
        smokeTimer = smokeCooldown;
        for(int i = 0; i < smokeFXs.Length; i++)
            smokeFXs[i].Play();
    }

    protected void AnimateBody()
    {
        elapsedTime += Time.deltaTime * bodyAnimSpeed;
        float sinValue = (Mathf.Sin(elapsedTime) + 1) / 2;
        float newY = Mathf.Lerp(0, maxHeight, sinValue);

        visuals.localPosition = startPosition + new Vector3(0, newY, 0);
    }

    protected void UpdateSpiderLeg()
    {
        for (int i = 0; i < legs.Length; i++)
            legs[i].UpdateLegMovement();
    }

    public void BrieflySpeedUpLegs()
    {
        for (int i = 0; i < legs.Length; i++)
            legs[i].SpeedUpLeg();
    }
}