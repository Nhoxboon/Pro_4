using UnityEngine;

public class CameraController : NhoxBehaviour
{
    [SerializeField] protected bool canControl;
    [SerializeField] protected Vector3 levelCenterPoint;
    [SerializeField] protected float maxDistanceFromCenter;

    [Header("Movement Settings")] protected float moveSpeed = 120f;
    [SerializeField] protected float mouseMovementSpeed = 5f;
    [SerializeField] protected float edgeMovementSpeed = 50f;
    [SerializeField] protected float edgeThreshold = 10f;
    protected float screenWidth;
    protected float screenHeight;

    [Header("Rotation Settings")] [SerializeField]
    protected Transform focusPoint;

    [SerializeField] protected float maxFocusDistance = 15f;
    [SerializeField] protected float rotationSpeed = 200f;
    protected float pitch;
    protected float minPitch = 5f;
    protected float maxPitch = 85f;

    [Header("Zoom Settings")] [SerializeField]
    protected float zoomSpeed = 10f;

    [SerializeField] protected float minZoom = 3f;
    [SerializeField] protected float maxZoom = 15f;

    protected float smoothTime = 0.1f;
    protected Vector3 moveVelocity = Vector3.zero;
    protected Vector3 mouseVelocity = Vector3.zero;
    protected Vector3 edgeVelocity = Vector3.zero;
    protected Vector3 zoomVelocity = Vector3.zero;

    protected InputManager inputManager;

    protected override void Start()
    {
        inputManager = InputManager.Instance;
        screenWidth = Screen.width;
        screenHeight = Screen.height;
    }

    protected void Update()
    {
        if (!canControl) return;
        HandleRotation();
        HandleZoom();
        // HandleEdgeMovement();
        HandleMouseMovement();
        HandleMovement();
        focusPoint.position = transform.position + transform.forward * GetFocusPointDistance();
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadFocusPoint();
    }

    protected void LoadFocusPoint()
    {
        if (focusPoint != null) return;
        focusPoint = transform.parent.Find("FocusPoint");
        Debug.Log(transform.name + " :LoadFocusPoint", gameObject);
    }

    public void EnableCameraControl(bool enable) => canControl = enable;
    public float AdjustPitch(float value) => pitch = value;

    protected void HandleZoom()
    {
        float scroll = inputManager.ScrollInput;
        if (Mathf.Approximately(scroll, 0f)) return;
        Vector3 zoomDirection = transform.forward * (scroll * zoomSpeed);
        Vector3 targetPosition = transform.position + zoomDirection;

        if (transform.position.y < minZoom && scroll > 0) return;
        if (transform.position.y > maxZoom && scroll < 0) return;

        // transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref zoomVelocity, smoothTime);
        MoveSmoothlyTo(targetPosition, ref zoomVelocity);
    }

    protected void HandleRotation()
    {
        if (!inputManager.IsRightMouseHeld) return;
        var horizontalRotation = inputManager.MouseLookInput.x * rotationSpeed * Time.deltaTime;
        var verticalRotation = inputManager.MouseLookInput.y * rotationSpeed * Time.deltaTime;

        pitch = Mathf.Clamp(pitch - verticalRotation, minPitch, maxPitch);

        transform.RotateAround(focusPoint.position, Vector3.up, horizontalRotation);
        transform.RotateAround(focusPoint.position, transform.right, pitch - transform.eulerAngles.x);
        transform.LookAt(focusPoint);
    }

    protected void HandleMovement()
    {
        Vector2 input = inputManager.CameraMovementInput;
        if (input == Vector2.zero) return;

        Vector3 flatForward = GetFlatForward();
        Vector3 direction = (flatForward * input.y + transform.right * input.x).normalized;

        Vector3 targetPosition = transform.position + direction * (moveSpeed * Time.deltaTime);

        MoveSmoothlyTo(targetPosition, ref moveVelocity);
    }

    protected void HandleMouseMovement()
    {
        if (!inputManager.IsMiddleMouseHeld) return;

        Vector2 mouseDelta = inputManager.MouseDelta;

        Vector3 moveRight = transform.right * (-mouseDelta.x * mouseMovementSpeed * Time.deltaTime);
        Vector3 moveForward = transform.forward * (-mouseDelta.y * mouseMovementSpeed * Time.deltaTime);

        moveRight.y = 0;
        moveForward.y = 0;

        Vector3 movement = moveRight + moveForward;
        Vector3 targetPosition = transform.position + movement;
        MoveSmoothlyTo(targetPosition, ref mouseVelocity);
    }

    private void HandleEdgeMovement()
    {
        Vector3 targetPosition = transform.position;
        Vector3 mousePosition = inputManager.MousePosition;

        if (mousePosition.x > screenWidth - edgeThreshold)
            targetPosition += transform.right * (edgeMovementSpeed * Time.deltaTime);

        if (mousePosition.x < edgeThreshold)
            targetPosition -= transform.right * (edgeMovementSpeed * Time.deltaTime);

        if (mousePosition.y > screenHeight - edgeThreshold)
            targetPosition += GetFlatForward() * (edgeMovementSpeed * Time.deltaTime);

        if (mousePosition.y < edgeThreshold)
            targetPosition -= GetFlatForward() * (edgeMovementSpeed * Time.deltaTime);

        MoveSmoothlyTo(targetPosition, ref edgeVelocity);
    }

    protected void MoveSmoothlyTo(Vector3 target, ref Vector3 velocity)
    {
        Vector3 clamped = ClampToLevelBounds(target);
        transform.position = Vector3.SmoothDamp(transform.position, clamped, ref velocity, smoothTime);
    }

    protected Vector3 ClampToLevelBounds(Vector3 position)
    {
        if (Vector3.Distance(levelCenterPoint, position) <= maxDistanceFromCenter)
            return position;

        Vector3 dir = (position - levelCenterPoint).normalized;
        return levelCenterPoint + dir * maxDistanceFromCenter;
    }

    protected Vector3 GetFlatForward()
    {
        return Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized;
    }

    protected float GetFocusPointDistance()
    {
        return Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, maxFocusDistance)
            ? hit.distance
            : maxFocusDistance;
    }
}