using UnityEngine;

public class SpiderNestVisuals : TowerVisuals
{
    [SerializeField] protected Transform[] webSet;
    public Transform[] WebSet => webSet;
    
    [SerializeField] protected Transform[] attachPoint;
    public Transform[] AttachPoint => attachPoint;
    
    [SerializeField] protected Transform[] attachPointRef;

    protected void Update() => UpdateAttachPointPos();
    
    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadWeb();
        LoadAttachPoint();
        LoadAttachPointRef();
    }

    protected void LoadWeb()
    {
        if(webSet is { Length: > 0 }) return;
        webSet = new Transform[4];
        for (int i = 0; i < webSet.Length; i++)
            webSet[i] = towerCtrl.transform.Find($"Model/tower_spider_nest_spinner/spider_nest_web_{i + 1}");
        
        DebugTool.Log(transform.name + " :LoadWeb", gameObject);
    }

    protected void LoadAttachPoint()
    {
        if (attachPoint is { Length: > 0 }) return;
        attachPoint = new Transform[4];
        for (int i = 0; i < attachPoint.Length; i++)
            attachPoint[i] = towerCtrl.transform.Find($"Model/spider_bot_holder_{i + 1}");
        
        DebugTool.Log(transform.name + " :LoadAttachPoint", gameObject);
    }

    protected void LoadAttachPointRef()
    {
        if (attachPointRef is { Length: > 0 }) return;
        attachPointRef = new Transform[webSet.Length];
        for(int i = 0; i < attachPointRef.Length; i++)
            attachPointRef[i] = webSet[i].Find("AttachPointRef");
        
        DebugTool.Log(transform.name + " :LoadAttachPointRef", gameObject);
    }

    protected void UpdateAttachPointPos()
    {
        for (int i = 0; i < attachPoint.Length; i++)
            attachPoint[i].position = attachPointRef[i].position;
    }
    
    public override void ResetVisual()
    {
        base.ResetVisual();
        foreach (var web in WebSet)
            web.localScale = new Vector3(1f, 0.1f, 1f);
    }
}