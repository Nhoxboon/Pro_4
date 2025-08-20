using System.Collections;
using UnityEngine;

public class TowerStatus : TowerComponent
{
    protected bool isActive = true;
    protected Coroutine deactivatedTowerCoroutine;
    protected Transform currentEMPFX;
    public bool IsActive => isActive;

    [SerializeField] protected bool towerAttackForward;
    public bool TowerAttackForward => towerAttackForward;

    protected override void Awake()
    {
        base.Awake();
        if (towerCtrl is FanCtrl fan)
            towerAttackForward = true;
    }

    public void DeactivateTower(float duration, string empFX)
    {
        if (deactivatedTowerCoroutine != null) StopCoroutine(deactivatedTowerCoroutine);

        if (currentEMPFX != null)
            FXSpawner.Instance.Despawn(currentEMPFX.gameObject);

        currentEMPFX =
            FXSpawner.Instance.Spawn(empFX, towerCtrl.transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
        currentEMPFX.gameObject.SetActive(true);
        deactivatedTowerCoroutine = StartCoroutine(DeactivateTowerCoroutine(duration));
    }

    private IEnumerator DeactivateTowerCoroutine(float duration)
    {
        isActive = false;
        towerCtrl.Rotation?.EnableRotation(false);
        yield return new WaitForSeconds(duration);
        towerCtrl.Rotation?.EnableRotation(true);
        isActive = true;
        FXSpawner.Instance.Despawn(currentEMPFX.gameObject);
        currentEMPFX = null;
    }

    public void DestroyTower() => TowerSpawner.Instance.Despawn(towerCtrl.gameObject);
    
    public void ResetStatus()
    {
        isActive = true;
        if (deactivatedTowerCoroutine != null) StopCoroutine(deactivatedTowerCoroutine);

        if (currentEMPFX != null)
            FXSpawner.Instance.Despawn(currentEMPFX.gameObject);
        currentEMPFX = null;
    }
}