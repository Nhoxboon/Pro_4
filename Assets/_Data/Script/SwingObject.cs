using System;
using UnityEngine;

public class SwingObject : NhoxBehaviour
{
    [Header("Swing Settings")]
    [SerializeField] protected Vector3 swingAxis = Vector3.right;
    [SerializeField] protected float swingDegree = 5f;
    [SerializeField] protected float swingSpeed = 10f;

    protected Quaternion startRotation;

    protected override void Start()
    {
        base.Start();
        startRotation = transform.localRotation;
    }

    protected void Update() => Swing();

    protected void Swing()
    {
        float angle = Mathf.Sin(Time.time * swingSpeed) * swingDegree;
        transform.localRotation = startRotation * Quaternion.AngleAxis(angle, swingAxis.normalized);
    }
}