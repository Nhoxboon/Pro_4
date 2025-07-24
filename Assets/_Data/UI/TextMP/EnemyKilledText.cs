public class EnemyKilledText : BaseText
{
    protected void OnEnable() => EnemiesKilledText();

    protected void EnemiesKilledText()
    {
        if (GameManager.Instance != null)
            textMeshPro.text = "Enemies Killed: " + GameManager.Instance.EnemyKilled;
    }
}