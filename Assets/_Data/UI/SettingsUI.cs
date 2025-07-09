
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : NhoxBehaviour
{
    [SerializeField] protected Slider[] sliders;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadSliders();
    }

    protected void LoadSliders()
    {
        if(sliders is { Length: > 0 }) return;
        sliders = GetComponentsInChildren<Slider>(true);
        Debug.Log(transform.name + " :LoadSliders", gameObject);
    }
}
