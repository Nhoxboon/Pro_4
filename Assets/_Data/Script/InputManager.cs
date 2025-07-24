using UnityEngine;

public class InputManager : NhoxBehaviour
{
    private static InputManager instance;
    public static InputManager Instance => instance;

    [Header("Keyboard Input")] public bool IsEscDown { get; private set; }
    public bool IsF10Down { get; private set; }
    public bool IsSpaceDown { get; private set; }
    public bool[] IsNumberKeyDown { get; private set; } = new bool[10];

    [Header("Camera Input")] public Vector2 CameraMovementInput { get; private set; }
    public float ScrollInput { get; private set; }
    public Vector3 MousePosition { get; private set; }
    public Vector2 MouseLookInput { get; private set; }

    [Header("Mouse Input")] public bool IsLeftMouseDown { get; private set; }
    public bool IsLeftMouseHeld { get; private set; }
    public bool IsRightMouseHeld { get; private set; }
    public bool IsMiddleMouseHeld { get; private set; }
    public bool IsMiddleMouseDown { get; private set; }
    public Vector2 MouseDelta { get; private set; }

    private Vector3 lastMousePosition;

    protected override void Awake()
    {
        base.Awake();
        if (instance != null && instance != this)
        {
            DebugTool.LogError("Only one InputManager allowed to exist");
            return;
        }

        instance = this;
    }

    private void Update()
    {
        ReadKeyboardInput();
        ReadMouseInput();
    }

    private void ReadKeyboardInput()
    {
        ReadMovementKeys();
        ReadFunctionKeys();
        ReadNumberKeys();
    }

    private void ReadMouseInput()
    {
        UpdateMousePosition();
        ProcessMouseLookInput();
        ReadScrollInput();
        ReadMouseButtons();
        UpdateMouseDelta();
    }

    private void ReadMovementKeys()
    {
        float vertical = Input.GetAxisRaw("Vertical");
        float horizontal = Input.GetAxisRaw("Horizontal");
        CameraMovementInput = new Vector2(horizontal, vertical);
    }

    private void ReadFunctionKeys()
    {
        IsEscDown = Input.GetKeyDown(KeyCode.Escape);
        IsF10Down = Input.GetKeyDown(KeyCode.F10);
        IsSpaceDown = Input.GetKeyDown(KeyCode.Space);
    }

    private void ReadNumberKeys()
    {
        for (int i = 0; i < 10; i++)
            IsNumberKeyDown[i] = Input.GetKeyDown(KeyCode.Alpha0 + i);
    }

    private void UpdateMousePosition() => MousePosition = Input.mousePosition;

    private void ProcessMouseLookInput()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        MouseLookInput = new Vector2(mouseX, mouseY);
    }

    private void ReadScrollInput() => ScrollInput = Input.GetAxis("Mouse ScrollWheel");


    private void ReadMouseButtons()
    {
        IsLeftMouseDown = Input.GetMouseButtonDown(0);
        IsLeftMouseHeld = Input.GetMouseButton(0);
        IsRightMouseHeld = Input.GetMouseButton(1);
        IsMiddleMouseDown = Input.GetMouseButtonDown(2);
        IsMiddleMouseHeld = Input.GetMouseButton(2);
    }

    private void UpdateMouseDelta()
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
}