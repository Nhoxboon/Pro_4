public class ProjectileSpawner : Spawner
{
    private static ProjectileSpawner instance;
    public static ProjectileSpawner Instance => instance;

    protected override void Awake()
    {
        base.Awake();
        if (instance != null)
        {
            // DebugTool.LogError("Only one ProjectileSpawner allowed to exist");
            return;
        }

        instance = this;
    }
}