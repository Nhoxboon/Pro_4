using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealthEnemy : Enemy
{
    [SerializeField] protected List<Enemy> enemiesToHide;
    public List<Enemy> EnemiesToHide => enemiesToHide;
    [SerializeField] protected float hideDuration = 0.5f;
    [SerializeField] protected ParticleSystem smokeFX;
    protected bool canHideEnemy = true;

    protected override void Awake()
    {
        base.Awake();

        InvokeRepeating(nameof(HideItSelf), 0.1f, hideDuration);
        InvokeRepeating(nameof(HideEnemies), 0.1f, hideDuration);
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadSmokeFX();
    }
    
    protected void LoadSmokeFX()
    {
        if (smokeFX != null) return;
        smokeFX = transform.GetComponentInChildren<ParticleSystem>();
        DebugTool.Log(transform.name + " :LoadSmokeFX", gameObject);
    }

    protected void HideItSelf() => HideEnemy(hideDuration);

    protected void HideEnemies()
    {
        if (!canHideEnemy) return;
        foreach (var enemy in enemiesToHide)
            enemy.HideEnemy(hideDuration);
    }

    public void EnableSmoke(bool enable)
    {
        switch (enable)
        {
            case true when !smokeFX.isPlaying:
                smokeFX.Play();
                break;
            case false when smokeFX.isPlaying:
                smokeFX.Stop();
                break;
        }
    }

    protected override IEnumerator DisableHideCoroutine(float duration)
    {
        canBeHidden = false;
        canHideEnemy = false;
        EnableSmoke(false);
        yield return new WaitForSeconds(duration);
        EnableSmoke(true);
        canHideEnemy = true;
        canBeHidden = true;
    }

    public override void ResetEnemy()
    {
        base.ResetEnemy();
        canHideEnemy = true;
    }
}