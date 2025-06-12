using System.Collections;
using UnityEngine;

public class CrossbowVisual : NhoxBehaviour
{
    [SerializeField] protected CrossbowTower tower;
    
    [SerializeField] protected LineRenderer attackVisual;
    [SerializeField] protected float attackVisualDuration = 0.1f;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadTower();
        LoadAttackVisual();
    }
    
    protected void LoadTower()
    {
        if (tower != null) return;
        tower = GetComponentInParent<CrossbowTower>();
        Debug.Log(transform.name + " :LoadTower", gameObject);
    }

    protected void LoadAttackVisual()
    {
        if (attackVisual != null) return;
        attackVisual = GetComponentInChildren<LineRenderer>();
        Debug.Log(transform.name + " :LoadAttackVisual", gameObject);
    }

    public void PlayAttackVFX(Vector3 startPoint, Vector3 endPoint)
    {
        StartCoroutine(FXCoroutine(startPoint, endPoint));
    }

    private IEnumerator FXCoroutine(Vector3 startPoint, Vector3 endPoint)
    {
        tower.EnableRotation(false);
        attackVisual.enabled = true;
        attackVisual.SetPosition(0, startPoint);
        attackVisual.SetPosition(1, endPoint);
        
        yield return new WaitForSeconds(attackVisualDuration);
        attackVisual.enabled = false;
        tower.EnableRotation(true);
    }
}