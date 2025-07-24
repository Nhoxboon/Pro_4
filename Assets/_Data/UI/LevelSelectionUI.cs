using System;
using UnityEngine;

public class LevelSelectionUI : NhoxBehaviour
{
    protected void OnEnable() => MakeBtnClickable(true);

    protected void OnDisable() => MakeBtnClickable(false);

    protected void MakeBtnClickable(bool canClick)
    {
        LevelBtnTile[] levelBtns = FindObjectsByType<LevelBtnTile>(FindObjectsSortMode.InstanceID);
        foreach (var btn in levelBtns)
        {
            btn.CheckIfLevelUnlocked();
            btn.EnableClick(canClick);
        }
    }
}