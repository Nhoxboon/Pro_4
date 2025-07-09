using UnityEditor;
using UnityEngine;

public class QuitBtn : BaseBtn
{
    protected override void OnClick() => Quit();
    
    protected void Quit()
    {
        if (EditorApplication.isPlaying)
            EditorApplication.isPlaying = false;
        else
            Application.Quit();
    }
}