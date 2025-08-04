using UnityEngine;

public class Death : CoreComponent
{
    protected string deathFX = "DeathFX";
    [SerializeField] protected float deathFXScale = 0.5f;
    protected bool isDead;
    public bool IsDead => isDead;

    public virtual void Die()
    {
        SpawnDeathFX();
        DestroyEnemy();
        ManagerCtrl.Instance.GameManager.UpdateCurrency(1);
    }

    public void DestroyEnemy()
    {
        core.Enemy.MyPortal?.RemoveActiveEnemy(core.Root.gameObject);
        EnemySpawner.Instance.Despawn(transform.parent.parent.gameObject);
    }

    protected void SpawnDeathFX()
    {
        var newFX = FXSpawner.Instance.Spawn(deathFX, core.Root.transform.position + new Vector3(0, 0.15f, 0),
            Quaternion.identity);
        newFX.localScale = new Vector3(deathFXScale, deathFXScale, deathFXScale);
        newFX.gameObject.SetActive(true);
    }

    public void SetDead(bool value) => isDead = value;
}