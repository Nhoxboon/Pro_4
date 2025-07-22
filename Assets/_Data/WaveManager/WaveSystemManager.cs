public abstract class WaveSystemManager : NhoxBehaviour
{
    protected static WaveSystemManager instance;

    protected override void Awake()
    {
        base.Awake();
        if (instance != null && instance.GetType() == GetType())
        {
            // DebugTool.LogError($"Only one {GetType().Name} allowed to exist");
            return;
        }

        SetInstance();
    }

    protected abstract void SetInstance();
}