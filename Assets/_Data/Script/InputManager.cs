using UnityEngine;

public class InputManager : NhoxBehaviour
{
    private static InputManager instance;
    public static InputManager Instance => instance;

    public bool IsEscDown { get; private set; }

    public bool IsF10Down {get; private set;}

    [Header("Camera Input")] public Vector2 CameraMovementInput { get; private set; } // WASD / Arrow keys
    public float ScrollInput { get; private set; } // Mouse Scroll
    public Vector3 MousePosition { get; private set; }
    public Vector2 MouseLookInput { get; private set; } // Mouse X/Y for rotation

    public bool IsLeftMouseDown { get; private set; }
    public bool IsLeftMouseHeld { get; private set; }

    public bool IsRightMouseHeld { get; private set; } // For rotation
    public bool IsMiddleMouseHeld { get; private set; } // For panning
    public bool IsMiddleMouseDown { get; private set; } // For panning start
    public Vector2 MouseDelta { get; private set; } // Mouse drag movement

    private Vector3 lastMousePosition;

    protected override void Awake()
    {
        base.Awake();

        if (instance != null && instance != this)
        {
            Debug.LogError("Only one InputManager allowed to exist");
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    private void Update()
    {
        ProcessCameraMovementInput();
        UpdateMousePosition();
        ProcessMouseLookInput();
        ReadScrollInput();
        ReadMouseButtons();
        UpdateMouseDelta();

        ReadF10Key();
        ReadEscKey();
    }

    protected void ProcessCameraMovementInput()
    {
        float vertical = Input.GetAxisRaw("Vertical");
        float horizontal = Input.GetAxisRaw("Horizontal");
        CameraMovementInput = new Vector2(horizontal, vertical);
    }

    protected void UpdateMousePosition()
    {
        MousePosition = Input.mousePosition;
    }

    protected void ProcessMouseLookInput()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        MouseLookInput = new Vector2(mouseX, mouseY);
    }

    protected void ReadScrollInput()
    {
        ScrollInput = Input.GetAxis("Mouse ScrollWheel");
    }

    protected void ReadMouseButtons()
    {
        IsLeftMouseDown = Input.GetMouseButtonDown(0);
        IsLeftMouseHeld = Input.GetMouseButton(0);
        IsRightMouseHeld = Input.GetMouseButton(1);
        IsMiddleMouseDown = Input.GetMouseButtonDown(2);
        IsMiddleMouseHeld = Input.GetMouseButton(2);
    }

    protected void UpdateMouseDelta()
    {
        if (IsMiddleMouseDown)
        {
            lastMousePosition = Input.mousePosition;
            MouseDelta = Vector2.zero;
            return;
        }

        if (IsMiddleMouseHeld)
        {
            MouseDelta = (Vector2)(Input.mousePosition - lastMousePosition);
            lastMousePosition = Input.mousePosition;
        }
        else
        {
            MouseDelta = Vector2.zero;
        }
    }

    protected void ReadEscKey() => IsEscDown = Input.GetKeyDown(KeyCode.Escape);

    protected void ReadF10Key() => IsF10Down = Input.GetKeyDown(KeyCode.F10);
}