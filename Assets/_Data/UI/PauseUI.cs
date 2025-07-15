using System;
using UnityEngine;

public class PauseUI : NhoxBehaviour
{
    [SerializeField] protected GameObject[] pauseUIElements;

    protected void OnEnable() => Time.timeScale = 0;

    protected void OnDisable() => Time.timeScale = 1;

    protected void Update()
    {
        if (InputManager.Instance.IsF10Down) UI.Instance.SwitchToUI(UI.Instance.InGameUI.gameObject);
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadPauseUIElements();
    }

    protected void LoadPauseUIElements()
    {
        if (pauseUIElements is { Length: > 0 }) return;
        int childCount = transform.childCount;
        pauseUIElements = new GameObject[childCount];

        for (int i = 0; i < childCount; i++) pauseUIElements[i] = transform.GetChild(i).gameObject;

        Debug.Log(transform.name + " :LoadPauseUIElements", gameObject);
    }

    public void SwitchPauseUIElement(GameObject uiEnable)
    {
        foreach (var obj in pauseUIElements) obj.SetActive(false);

        uiEnable.SetActive(true);
    }
}