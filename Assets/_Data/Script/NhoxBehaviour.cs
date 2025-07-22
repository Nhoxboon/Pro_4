using UnityEngine;

public class NhoxBehaviour : MonoBehaviour
{
    protected virtual void Awake()
    {
        this.LoadComponents();
    }

    protected virtual void Start()
    {
        //For override
    }

    protected virtual void LoadComponents()
    {
        //For override
    }

    public virtual void Reset()
    {
        this.LoadComponents();
    }
}