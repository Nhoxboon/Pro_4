using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class CameraEffects : NhoxBehaviour
{
    [SerializeField] protected CameraController cameraController;
    private Coroutine cameraCoroutine;
    public Coroutine CameraCoroutine => cameraCoroutine;

    [Header("Transition Details")] [SerializeField]
    protected float transitionDuration = 3f;

    [SerializeField] protected Vector3 inMenuPosition = new Vector3(-2.5f, 9f, 22f);
    [SerializeField] protected Quaternion inMenuRotation = Quaternion.Euler(38f, 139f, 0f);
    [Space] 
    [SerializeField] protected Vector3 inGamePosition = new Vector3(-2.4f, 14.8f, -1.3f);
    [SerializeField] protected Quaternion inGameRotation = Quaternion.Euler(53.3f, 50f, 0f);
    [Space] 
    [SerializeField] protected Vector3 levelSelectPosition;
    [SerializeField] protected Quaternion levelSelectRotation;

    [Header("Castle Focus Details")] [SerializeField]
    protected float focusOnCastleDuration = 2f;

    [SerializeField] protected float highOffset = 3f;
    [SerializeField] protected float distanceToCastle = 7f;

    protected override void Start()
    {
        base.Start();
        SwitchToMenuView();
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadCameraController();
    }

    protected void LoadCameraController()
    {
        if (cameraController != null) return;
        cameraController = GetComponent<CameraController>();
        DebugTool.Log(transform.name + " :LoadCameraController", gameObject);
    }

    public void ScreenShake(float magnitude, float duration) => StartCoroutine(ScreenShakeFX(magnitude, duration));

    public void SwitchToMenuView()
    {
        if (cameraCoroutine != null) StopCoroutine(cameraCoroutine);

        cameraCoroutine = StartCoroutine(ChangePositionAndRotation(inMenuPosition, inMenuRotation, transitionDuration));
        cameraController.AdjustPitch(inMenuRotation.eulerAngles.x);
    }

    public void SwitchToGameView()
    {
        if (cameraCoroutine != null) StopCoroutine(cameraCoroutine);

        cameraCoroutine = StartCoroutine(ChangePositionAndRotation(inGamePosition, inGameRotation, transitionDuration));
        cameraController.AdjustPitch(inGameRotation.eulerAngles.x);

        StartCoroutine(EnableCameraControlAfterDelay(transitionDuration + 0.1f));
    }

    public void SwitchToLevelSelectView()
    {
        if (cameraCoroutine != null) StopCoroutine(cameraCoroutine);

        cameraCoroutine =
            StartCoroutine(ChangePositionAndRotation(levelSelectPosition, levelSelectRotation, transitionDuration));
        cameraController.AdjustPitch(levelSelectRotation.eulerAngles.x);
    }

    private IEnumerator ChangePositionAndRotation(Vector3 targetPosition, Quaternion targetRotation, float duration = 3,
        float delay = 0)
    {
        yield return new WaitForSeconds(delay);
        cameraController.EnableCameraControl(false);

        float timeElapsed = 0f;

        var startPosition = transform.position;
        var startRotation = transform.rotation;

        while (timeElapsed < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, timeElapsed / duration);
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, timeElapsed / duration);

            timeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
        transform.rotation = targetRotation;
    }

    private IEnumerator EnableCameraControlAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        cameraController.EnableCameraControl(true);
    }

    private IEnumerator ScreenShakeFX(float magnitude, float duration)
    {
        Vector3 originalPos = transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            var x = Random.Range(-1f, 1f) * magnitude;
            var y = Random.Range(-1f, 1f) * magnitude;

            transform.position = originalPos + new Vector3(x, y, 0f);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = originalPos;
    }

    public void FocusOnCastle()
    {
        Transform castle = FindFirstObjectByType<Castle>().transform;
        if (!castle) return;
        Vector3 directionToCastle = (castle.position - transform.position).normalized;
        Vector3 targetPosition = castle.position - (directionToCastle * distanceToCastle);
        targetPosition.y = castle.position.y + highOffset;

        Quaternion targetRotation = Quaternion.LookRotation(castle.position - targetPosition);

        if (cameraCoroutine != null) StopCoroutine(cameraCoroutine);
        cameraCoroutine =
            StartCoroutine(ChangePositionAndRotation(targetPosition, targetRotation, focusOnCastleDuration));
        StartCoroutine(EnableCameraControlAfterDelay(focusOnCastleDuration));
    }
}