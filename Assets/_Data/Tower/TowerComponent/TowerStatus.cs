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
            FXSpawner.Instance.Spawn(empFX, EMPFXOffset(), Quaternion.identity);
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

    protected Vector3 EMPFXOffset()
    {
        string towerName = towerCtrl.gameObject.name;
        if (towerName is "HarpoonTower" or "FanTower" or "HammerTower")
            return towerCtrl.transform.position;

        return towerCtrl.transform.position + new Vector3(0, 0.5f, 0);
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