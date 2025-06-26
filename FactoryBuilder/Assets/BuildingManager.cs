using UnityEngine;

/// <summary>
/// The script you access for all things about building
/// </summary>
public class BuildingManager : MonoBehaviour
{
    public static BuildingManager instance;

    public int gridSize = 10;
    private Building[,] grid;

    [SerializeField]
    private GameObject ground;

    private void Awake()
    {
        instance = this;

        grid = new Building[gridSize, gridSize];
        //Makes the ground the same size as the grid
        ground.transform.localScale = new Vector3(gridSize * BuildingTileSize, 1, gridSize * BuildingTileSize);
        ground.transform.position = ground.transform.localScale / 2 - new Vector3(((float)BuildingTileSize) / 2, 0.5f, ((float)BuildingTileSize) / 2);
    }

    //the Size of a 1x1 building
    public const int BuildingTileSize = 3;

    [System.Serializable]
    public struct BuildingInfo
    {
        public string name;
        [Tooltip("y axis represents how much it'll take on the z axis")]
        public Vector2Int size;

        public GameObject buildingPrefab;
        public GameObject buildingPreview;
    }

    public BuildingInfo[] buildings;

    [Tooltip("Material that is used by preview buildings to indicate if it can be placed or not")]
    public Material previewMaterial;
    public Color CanPlaceColour;
    public Color CantPlaceColour;

    public void HandlePreviewBuilding(int buildingId, int buildingRotation, Vector3 buildingPosition)
    {
        BuildingInfo buildingInfo = buildings[buildingId];

        buildingInfo.buildingPreview.SetActive(true);

        Vector2Int gridPos = Vec3ToGridPos(buildingPosition);
        buildingPosition = GridPosToVec3(gridPos);

        Transform preview = buildingInfo.buildingPreview.transform;

        preview.position = buildingPosition + buildingInfo.buildingPrefab.transform.position;
        preview.rotation = Quaternion.Euler(0, 90 * buildingRotation, 0);

        if (buildingRotation % 2 == 1)
            buildingInfo.size = new Vector2Int(buildingInfo.size.y, buildingInfo.size.x);

        if (BuildingHasSpace(buildingInfo.size, gridPos))
            previewMaterial.color = CanPlaceColour;
        else 
            previewMaterial.color = CantPlaceColour;
    }

    public void CancelPreviewBuilding(int buildingId) => buildings[buildingId].buildingPreview.SetActive(false);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="buildingId"></param>
    /// <param name="buildingPosition">Building position will be rounded to grid automatically</param>
    /// <param name="buildingRotation">building rotation will be times be 90 rotation in degrees</param>
    public void PlaceBuilding(int buildingId, int buildingRotation, Vector3 buildingPosition)
    {
        BuildingInfo buildingInfo = buildings[buildingId];
        Vector2Int gridPos = Vec3ToGridPos(buildingPosition);

        if (buildingRotation % 2 == 1)
            buildingInfo.size = new Vector2Int(buildingInfo.size.y, buildingInfo.size.x);

        if (!BuildingHasSpace(buildingInfo.size, gridPos))
            return;

        buildingPosition = GridPosToVec3(gridPos);

        Building building = Instantiate(buildingInfo.buildingPrefab, buildingPosition + buildingInfo.buildingPrefab.transform.position, Quaternion.Euler(0, 90 * buildingRotation, 0)).GetComponent<Building>();

        PlaceBuildingOnGrid(in buildingInfo.size, building, gridPos);
    }

    public Vector2Int Vec3ToGridPos(Vector3 vec3) => new Vector2Int((int)(vec3.x / BuildingTileSize + 0.5f), (int) (vec3.z / BuildingTileSize + 0.5f));
    public Vector3 GridPosToVec3(Vector2Int gridPos) => new Vector3(gridPos.x * BuildingTileSize, 1, gridPos.y * BuildingTileSize);

    //add support for a chunk system later
    private void PlaceBuildingOnGrid(in Vector2Int size, Building building, Vector2Int gridPos)
    {
        for (int x = 0; x < size.x; x++)
            for (int z = 0; z < size.y; z++)
                grid[x + gridPos.x, z + gridPos.y] = building;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="size"></param>
    /// <param name="gridPos">Bottom Left of building</param>
    private bool BuildingHasSpace(in Vector2Int size, Vector2Int gridPos)
    {
        for (int x = 0; x < size.x; x++)
        {
            for (int z = 0; z < size.y; z++)
            {
                if (x + gridPos.x >= gridSize || z + gridPos.y >= gridSize || grid[x + gridPos.x, z + gridPos.y] != null)
                    return false;
            }
        }

        return true;
    }
}
