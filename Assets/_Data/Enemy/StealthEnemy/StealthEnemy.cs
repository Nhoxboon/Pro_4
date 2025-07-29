using System.Collections.Generic;
using UnityEngine;

public class StealthEnemy : Enemy
{
    [SerializeField] protected List<Enemy> enemiesToHide;
    public List<Enemy> EnemiesToHide => enemiesToHide;
    [SerializeField] protected float hideDuration = 0.5f;

    protected override void Awake()
    {
        base.Awake();

        InvokeRepeating(nameof(HideItSelf), 0.1f, hideDuration);
        InvokeRepeating(nameof(HideEnemies), 0.1f, hideDuration);
    }

    protected void HideItSelf() => HideEnemy(hideDuration);

    protected void HideEnemies()
    {
        foreach (var enemy in enemiesToHide)
            enemy.HideEnemy(hideDuration);
    }
}