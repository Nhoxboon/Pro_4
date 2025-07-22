using System.Collections.Generic;
using System.Linq;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;

// This class just use in the editor to switch tiles so not causing performance issues
public class TileSlot : MonoBehaviour
{
    private MeshRenderer MeshRenderer => GetComponent<MeshRenderer>();
    private MeshFilter MeshFilter => GetComponent<MeshFilter>();
    private Collider MyCollider => GetComponent<Collider>();
    private NavMeshSurface MyNavMesh => GetComponentInParent<NavMeshSurface>(true);
    private TileSetHolder tileSetHolder => GetComponentInParent<TileSetHolder>(true);

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

        // Same as above but using a for loop
        // List<GameObject> children = new List<GameObject>();
        // foreach (Transform child in transform)
        // {
        //     children.Add(child.gameObject);
        // }
        // return children;
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
}