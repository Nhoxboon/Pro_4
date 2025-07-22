using UnityEngine;
using UnityEngine.UI;

public abstract class BaseSlider : NhoxBehaviour
{
    [Header("Base Slider")] [SerializeField]
    protected Slider slider;

    protected virtual void OnEnable() => slider.onValueChanged.AddListener(OnValueChanged);

    protected virtual void OnDisable() => slider.onValueChanged.RemoveListener(OnValueChanged);

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadSlider();
    }

    protected void LoadSlider()
    {
        if (slider != null) return;
        slider = GetComponent<Slider>();
        DebugTool.Log(transform.name + " :LoadSlider", gameObject);
    }

    protected abstract void OnValueChanged(float value);
}