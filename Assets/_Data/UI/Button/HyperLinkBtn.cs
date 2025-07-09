
using UnityEngine;

public class HyperLinkBtn : BaseBtn
{
    [SerializeField] protected string url;
    
    protected override void OnClick() => OpenUrl();

    protected void OpenUrl() => Application.OpenURL(url);
}
