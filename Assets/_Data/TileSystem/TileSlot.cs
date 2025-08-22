using System.Collections.Generic;
using System.Linq;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;

// This class just use in the editor to switch tiles so not causing performance issues
public class TileSlot : NhoxBehaviour
{
    protected Material originalMaterial;
    protected MeshRenderer MeshRenderer => GetComponent<MeshRenderer>();
    protected MeshFilter MeshFilter => GetComponent<MeshFilter>();
    protected Collider MyCollider => GetComponent<Collider>();
    protected NavMeshSurface MyNavMesh => GetComponentInParent<NavMeshSurface>(true);
    protected TileSetHolder tileSetHolder => GetComponentInParent<TileSetHolder>(true);

    public MeshRenderer[] MeshRenderers {get; protected set;}

    protected override void Awake()
    {
        base.Awake();
        MeshRenderers = GetComponentsInChildren<MeshRenderer>();
        originalMaterial = GetComponent<MeshRenderer>().sharedMaterial;
    }

    public void SwitchTile(GameObject referenceTile)
    {
        gameObject.name = referenceTile.name;

        TileSlot newTile = referenceTile.GetComponent<TileSlot>();

        MeshFilter.mesh = newTile.GetMesh();
        MeshRenderer.material = newTile.GetMaterial();

        UpdateCollider(newTile.GetCollider());

        UpdateChildren(newTile);

        UpdateLayer(referenceTile);

        UpdateNavMesh();

        TurnIntoBuildSlot(referenceTile);
        DisableShadow();
    }

    public Material GetOriginalMaterial()
    {
        if(originalMaterial is null)
        {
            TryGetComponent(out MeshRenderer mesh);
            originalMaterial = mesh?.sharedMaterial;
        }
        return originalMaterial;
    }
    public Material GetMaterial() => MeshRenderer.sharedMaterial;
    public Mesh GetMesh() => MeshFilter.sharedMesh;

    public void TurnIntoBuildSlot(GameObject referenceTile)
    {
        if (TryGetComponent<BuildSlot>(out var buildSlot))
        {
            if (referenceTile != tileSetHolder.tileField) DestroyImmediate(buildSlot);
        }
        else
        {
            if (referenceTile == tileSetHolder.tileField) gameObject.AddComponent<BuildSlot>();
        }
    }

    public Collider GetCollider() => MyCollider;

    public List<GameObject> GetAllChildren()
    {
        return (from Transform child in transform select child.gameObject).ToList();
    }

    private void UpdateNavMesh() => MyNavMesh?.BuildNavMesh();

    private void UpdateChildren(TileSlot newTile)
    {
        foreach (var obj in GetAllChildren()) DestroyImmediate(obj);

        foreach (var obj in newTile.GetAllChildren()) Instantiate(obj, transform);
    }

    private void UpdateCollider(Collider newCollider)
    {
        DestroyImmediate(MyCollider);
        switch (newCollider)
        {
            case BoxCollider:
            {
                BoxCollider original = newCollider.GetComponent<BoxCollider>();
                BoxCollider myNewCollider = transform.AddComponent<BoxCollider>();

                myNewCollider.center = original.center;
                myNewCollider.size = original.size;
                break;
            }
            case MeshCollider:
            {
                MeshCollider original = newCollider.GetComponent<MeshCollider>();
                MeshCollider myNewCollider = transform.AddComponent<MeshCollider>();

                myNewCollider.sharedMesh = original.sharedMesh;
                myNewCollider.convex = original.convex;
                break;
            }
        }
    }

    private void UpdateLayer(GameObject referenceObj) => gameObject.layer = referenceObj.layer;

    public void RotateTile(int dir)
    {
        transform.Rotate(0, dir * 90, 0);
        UpdateNavMesh();
    }

    public void AdjustY(int verticalDir)
    {
        transform.position += new Vector3(0, verticalDir * 0.1f, 0);
        UpdateNavMesh();
    }

    public void DisableShadow()
    {
        var shadowMode = UnityEngine.Rendering.ShadowCastingMode.On;

        Vector3 point = transform.position + new Vector3(0, 0.49f, 0);
        Vector3[] direction = { Vector3.left, Vector3.right, Vector3.forward, Vector3.back, };

        int blockedSides = direction.Count(t => Physics.Raycast(point, t, 0.6f));
        if(blockedSides == direction.Length)
            shadowMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        MeshRenderer.shadowCastingMode = shadowMode;
    }
}