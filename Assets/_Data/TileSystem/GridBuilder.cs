using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class GridBuilder : MonoBehaviour
{
    protected NavMeshSurface navMeshSurface;
    
    [SerializeField] protected GameObject mainPrefab;

    [SerializeField] protected int gridLenght = 10;
    [SerializeField] protected int gridWidth = 10;

    [SerializeField] protected List<GameObject> createdTiles;
    public List<GameObject> CreatedTiles => createdTiles;
    
    protected void Awake() => navMeshSurface = GetComponent<NavMeshSurface>();
    
    public void UpdateNavMesh() => navMeshSurface.BuildNavMesh();

    [ContextMenu("Build Grid")]
    protected void BuildGrid()
    {
        ClearGrid();

        createdTiles = new List<GameObject>();
        for (int x = 0; x < gridLenght; x++)
        {
            for (int z = 0; z < gridWidth; z++) CreateTile(x, z);
        }
    }

    [ContextMenu("Clear Grid")]
    protected void ClearGrid()
    {
        foreach (GameObject tile in createdTiles) DestroyImmediate(tile);

        createdTiles.Clear();
    }

    protected void CreateTile(float xPosition, float zPosition)
    {
        Vector3 newPosition = new(xPosition, 0, zPosition);
        GameObject newTile = Instantiate(mainPrefab, newPosition, Quaternion.identity, transform);

        createdTiles.Add(newTile);
    }
}