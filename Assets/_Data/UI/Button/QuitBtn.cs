using UnityEditor;
using UnityEngine;

public class QuitBtn : BaseBtn
{
    protected override void OnClick() => Quit();
    
    protected void Quit()
    {
#if UNITY_EDITOR
        if (EditorApplication.isPlaying)
            EditorApplication.isPlaying = false;
        else
#endif
            Application.Quit();
    }
}