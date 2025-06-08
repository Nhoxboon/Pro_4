using System;
using UnityEngine;

public class RotateObject : NhoxBehaviour
{
    [SerializeField] protected Vector3 rotVector = Vector3.right;
    [SerializeField] protected float rotSpeed = 3.5f;

    protected void Update()
    {
        Rotate();
    }
    
    protected void Rotate()
    {
        // if (rotVector == Vector3.zero) return;
        //
        // Quaternion targetRotation = Quaternion.Euler(rotVector * (rotSpeed * Time.deltaTime));
        // transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation * transform.rotation, Time.deltaTime);

        float newRotSpeed = rotSpeed * 100f;
        transform.Rotate(rotVector * (newRotSpeed * Time.deltaTime));
    }
}
