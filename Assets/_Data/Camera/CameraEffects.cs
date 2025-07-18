using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class CameraEffects : NhoxBehaviour
{
    [SerializeField] protected CameraController cameraController;
    [SerializeField] protected Vector3 inMenuPosition;
    [SerializeField] protected Quaternion inMenuRotation;
    [Space] [SerializeField] protected Vector3 inGamePosition;
    [SerializeField] protected Quaternion inGameRotation;

    //ForTesting purposes
    protected override void Start()
    {
        base.Start();
        SwitchToGameView();
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
        Debug.Log(transform.name + " :LoadCameraController", gameObject);
    }

    public void ScreenShake(float magnitude, float duration) => StartCoroutine(ScreenShakeFX(magnitude, duration));

    public void SwitchToMenuView()
    {
        StartCoroutine(ChangePositionAndRotation(inMenuPosition, inMenuRotation));
        // GameManager.Instance.SetInGame(false);
        cameraController.AdjustPitch(inMenuRotation.eulerAngles.x);
    }

    public void SwitchToGameView()
    {
        StartCoroutine(ChangePositionAndRotation(inGamePosition, inGameRotation));
        GameManager.Instance.SetInGame(true);
        cameraController.AdjustPitch(inGameRotation.eulerAngles.x);
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
}