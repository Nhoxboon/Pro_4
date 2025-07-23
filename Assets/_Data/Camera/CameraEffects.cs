using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class CameraEffects : NhoxBehaviour
{
    [SerializeField] protected CameraController cameraController;
    private Coroutine cameraCoroutine;
    
    [SerializeField] protected Vector3 inMenuPosition = new Vector3(-2.5f, 9f, 22f);
    [SerializeField] protected Quaternion inMenuRotation = Quaternion.Euler(38f, 139f, 0f);
    [Space] 
    [SerializeField] protected Vector3 inGamePosition = new Vector3(-2.4f, 14.8f, -1.3f);
    [SerializeField] protected Quaternion inGameRotation = Quaternion.Euler(53.3f, 50f, 0f);
    [Space] 
    [SerializeField] protected Vector3 levelSelectPosition;
    [SerializeField] protected Quaternion levelSelectRotation;

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
        
        cameraCoroutine = StartCoroutine(ChangePositionAndRotation(inMenuPosition, inMenuRotation));
        // cameraController.EnableCameraControl(false);
        cameraController.AdjustPitch(inMenuRotation.eulerAngles.x);
    }

    public void SwitchToGameView()
    {
        if (cameraCoroutine != null) StopCoroutine(cameraCoroutine);
        
        cameraCoroutine = StartCoroutine(ChangePositionAndRotation(inGamePosition, inGameRotation));
        // cameraController.EnableCameraControl(true);
        cameraController.AdjustPitch(inGameRotation.eulerAngles.x);
    }

    public void SwitchToLevelSelectView()
    {
        if (cameraCoroutine != null) StopCoroutine(cameraCoroutine);
        
        cameraCoroutine = StartCoroutine(ChangePositionAndRotation(levelSelectPosition, levelSelectRotation));
        cameraController.AdjustPitch(levelSelectRotation.eulerAngles.x);
    }

    private IEnumerator ChangePositionAndRotation(Vector3 targetPosition, Quaternion targetRotation, float duration = 3,
        float delay = 0)
    {
        yield return new WaitForSeconds(delay);
        cameraController.EnableCameraControl(false); //Testing purpose

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
        cameraController.EnableCameraControl(true); //Testing purpose
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
}