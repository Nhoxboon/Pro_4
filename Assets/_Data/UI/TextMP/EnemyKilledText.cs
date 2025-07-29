public class EnemyKilledText : BaseText
{
    protected void OnEnable() => EnemiesKilledText();

    protected void EnemiesKilledText() =>
        textMeshPro.text = "Enemies Killed: " + ManagerCtrl.Instance.GameManager?.EnemyKilled;
}