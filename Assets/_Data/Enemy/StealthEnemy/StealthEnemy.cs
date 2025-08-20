using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealthEnemy : Enemy
{
    [SerializeField] protected List<Enemy> enemiesToHide;
    public List<Enemy> EnemiesToHide => enemiesToHide;
    [SerializeField] protected float hideDuration = 0.5f;
    protected bool canHideEnemy = true;

    protected void HideItSelf() => HideEnemy(hideDuration);

    protected void HideEnemies()
    {
        if (!canHideEnemy) return;
        foreach (var enemy in enemiesToHide)
            enemy.HideEnemy(hideDuration);
    }

    protected override IEnumerator DisableHideCoroutine(float duration)
    {
        if (core.Visuals is not StealthVisuals visuals) yield break;
        canBeHidden = false;
        canHideEnemy = false;
        visuals.EnableSmoke(false);
        yield return new WaitForSeconds(duration);
        visuals.EnableSmoke(true);
        canHideEnemy = true;
        canBeHidden = true;
    }

    public override void ResetEnemy()
    {
        base.ResetEnemy();
        InvokeRepeating(nameof(HideItSelf), 0.1f, hideDuration);
        InvokeRepeating(nameof(HideEnemies), 0.1f, hideDuration);
        canHideEnemy = true;
    }
}