using UnityEngine;

public class RotateObject : NhoxBehaviour
{
    [SerializeField] protected Vector3 rotVector = Vector3.right;
    [SerializeField] protected float rotSpeed = 3.5f;

    protected void Update() => Rotate();

    protected void Rotate()
    {
        float newRotSpeed = rotSpeed * 100f;
        transform.Rotate(rotVector * (newRotSpeed * Time.deltaTime));
    }

    public void SetRotationSpeed(float speed) => rotSpeed = speed;
}