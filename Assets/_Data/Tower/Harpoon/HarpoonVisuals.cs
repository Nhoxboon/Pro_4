using System;
using System.Collections.Generic;
using UnityEngine;

public class HarpoonVisuals : TowerVisuals
{
    protected string chainLink = "ChainLink";
    [SerializeField] protected Transform startPoint;
    protected Transform endPoint;
    [SerializeField] protected Transform linkParent;
    
    [SerializeField] protected float linkDistance = 0.1f;
    [SerializeField] protected int maxLinks = 100;
    
    protected List<ChainLink> links = new ();

    protected string onElectrifyFX = "OnElectrifyFX";
    protected Vector3 vfxOffset = new (0, -0.2f, 0);
    protected Transform currentFX;

    protected void Update()
    {
        if (endPoint is null || !endPoint.gameObject.activeInHierarchy)
        {
            EnableChainVisuals(false);
            return;
        }
        ActivateLinks();
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadStartPoint();
        LoadLinkParent();
    }
    
    protected void LoadStartPoint()
    {
        if (startPoint != null) return;
        startPoint = towerCtrl.Attack.GunPoint;
        DebugTool.Log(transform.name + " :LoadStartPoint", gameObject);
    }

    protected void LoadLinkParent()
    {
        if(linkParent != null) return;
        linkParent = transform.parent.parent.Find("Model/LinkParent");
        DebugTool.Log(transform.name + " :LoadLinkParent", gameObject);
    }

    protected void ActivateLinks()
    {
        Vector3 direction = (endPoint.position - startPoint.position).normalized;
        float distance = Vector3.Distance(startPoint.position, endPoint.position);
        int activeLinkAmount = Mathf.Min(maxLinks, Mathf.CeilToInt(distance / linkDistance));

        InitializeLinks(activeLinkAmount);

        for (int i = 0; i < links.Count; i++)
        {
            if (i < activeLinkAmount)
            {
                Vector3 newPosition = startPoint.position + direction * (linkDistance * (i + 1));
                links[i].PositionLink(newPosition);
            }
            else
            {
                links[i].DespawnLink();
                links.RemoveAt(i);
                i--;
            }
            
            if(i != links.Count - 1)
                links[i].UpdateLineRenderer(links[i], links[i + 1]);
        }
    }

    public void CreateElectrifyVFX(Transform target)
    {
        currentFX = FXSpawner.Instance.Spawn(onElectrifyFX, target.position + vfxOffset, Quaternion.identity);
        currentFX.gameObject.SetActive(true);
        currentFX.SetParent(target);
    }

    protected void DestroyElectrifyVFX()
    {
        if(currentFX is null) return;
        FXSpawner.Instance.Despawn(currentFX.gameObject);
    }
    
    public void EnableChainVisuals(bool enable, Transform newEndPoint = null)
    {
        if(enable)
            endPoint = newEndPoint;
        else
        {
            endPoint = startPoint;
            DestroyElectrifyVFX();
            foreach (ChainLink link in links)
                link.DespawnLink();
            
            links.Clear();
        }
    }

    protected void InitializeLinks(int neededLink)
    {
        while (links.Count < neededLink)
        {
            Transform link = ProjectileSpawner.Instance.Spawn(chainLink, startPoint.position, Quaternion.identity);
            link.gameObject.SetActive(true);
            link.SetParent(linkParent);
            if (link.TryGetComponent(out ChainLink chain))
                links.Add(chain);
        }
    }
}