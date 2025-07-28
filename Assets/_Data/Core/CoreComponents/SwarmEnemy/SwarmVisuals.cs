using System.Linq;
using UnityEngine;

public class SwarmVisuals : Visuals
{
    [Header("Visual variants")] [SerializeField]
    protected GameObject[] variants;

    [Header("Bounce settings")] [SerializeField]
    protected AnimationCurve bounceCurve = new AnimationCurve(new Keyframe(0f, 0f), 
                                                                        new Keyframe(0.666f, 0.568f),
                                                                        new Keyframe(1f, 0f));
    [SerializeField] protected float bounceSpeed = 2f;
    protected float bounceTimer;
    [SerializeField] protected float minHeight = 0.1f;
    [SerializeField] protected float maxHeight = 0.4f;

    protected override void Start()
    {
        base.Start();
        ChooseVisualVariant();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        BounceEffect();
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadVariants();
    }

    protected void LoadVariants()
    {
        if (variants is { Length: > 0 }) return;
        Transform modelTransform = core.Root.transform.Find("Model");

        variants = modelTransform.Cast<Transform>()
            .Select(t => t.gameObject)
            .ToArray();
        DebugTool.Log(transform.name + " LoadVariants", gameObject);
    }

    protected void ChooseVisualVariant()
    {
        foreach (Transform child in visuals)
            child.gameObject.SetActive(false);

        int randomIndex = Random.Range(0, visuals.childCount);
        visuals.GetChild(randomIndex).gameObject.SetActive(true);
    }

    protected void BounceEffect()
    {
        bounceTimer += Time.deltaTime * bounceSpeed;
        float bounceValue = bounceCurve.Evaluate(bounceTimer % 1);
        float bounceHeight = Mathf.Lerp(minHeight, maxHeight, bounceValue);

        visuals.localPosition = new Vector3(visuals.localPosition.x, bounceHeight, visuals.localPosition.z);
    }
}