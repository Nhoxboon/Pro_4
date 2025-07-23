using UnityEngine;

public class TileSetHolder : MonoBehaviour
{
    public GameObject levelSelectTile;

    [Header("Common Tiles")] public GameObject tileRoad;
    public GameObject tileField;
    public GameObject tileSideway;

    [Header("Corner Tiles")] public GameObject tileInnerCorner;
    public GameObject tileInnerCornerSmall;
    public GameObject tileOuterCorner;
    public GameObject tileOuterCornerSmall;

    [Header("Hills")] public GameObject tileHill1;
    public GameObject tileHill2;
    public GameObject tileHill3;

    [Header("Bridge")] public GameObject tileBridgeField;
    public GameObject tileBridgeRoad;
    public GameObject tileBridgeSideway;
}