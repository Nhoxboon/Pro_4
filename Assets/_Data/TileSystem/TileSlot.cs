
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TileSlot : MonoBehaviour
{
    //This jyst work in the editor so not causing performance issues
    protected MeshRenderer MeshRenderer => GetComponent<MeshRenderer>();
    protected MeshFilter MeshFilter => GetComponent<MeshFilter>();
    
    public void SwitchTile(GameObject referanceTile)
    {
        TileSlot newTile = referanceTile.GetComponent<TileSlot>();

        MeshFilter.mesh = newTile.GetMesh();
        MeshRenderer.material = newTile.GetMaterial();

        foreach (var obj in GetAllChildren()) DestroyImmediate(obj);
        
        foreach (var obj in newTile.GetAllChildren()) Instantiate(obj, transform);
        
    }
    
    public Material GetMaterial() => MeshRenderer.sharedMaterial;
    public Mesh GetMesh() => MeshFilter.sharedMesh;
    
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
}
