using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("Grid Settings")]
    public int width = 10;
    public int height = 10;
    public float cellSize = 1f;
    public float gap = 0.05f;
    public Vector3 originPosition;

    [Header("Visuals")]
    [SerializeField] private GameObject gridTilePrefab;
    [SerializeField] private int holeSize;

    private GridCell[,] grid;
    private GameObject[,] visualGrid;

    private void Awake()
    {
        originPosition = transform.position;
        CreateGrid();
    }

    private void Start()
    {
        CreateVisualGrid();        // visuals (after grid exists)
    }

    void CreateGrid()
    {
        holeSize = Mathf.Clamp(holeSize, 1, width - 1);

        grid = new GridCell[width, height];

        int startX = (width - holeSize) / 2;
        int endX = startX + holeSize;

        int startZ = (height - holeSize) / 2;
        int endZ = startZ + holeSize;

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                bool isInsideHole =
                    x >= startX && x < endX &&
                    z >= startZ && z < endZ;

                grid[x, z] = new GridCell(!isInsideHole);
            }
        }
    }

    void CreateVisualGrid()
    {
        visualGrid = new GameObject[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                if (!grid[x, z].isBuildable)
                    continue;

                Vector3 pos = GetWorldPosition(x, z) + Vector3.up * 0.01f;


                GameObject tile = Instantiate(gridTilePrefab, pos, Quaternion.identity, transform);
                
                float visualSize = cellSize - gap;

                tile.transform.localScale = new Vector3(
                    visualSize,
                    visualSize,
                    1f
                );

                tile.transform.localEulerAngles = new Vector3 (90f, 0f, 0f);

                visualGrid[x, z] = tile;
            }
        }
    }
    public void SetCellOccupied(int x, int z, bool occupied)
    {
        grid[x, z].isOccupied = occupied;
    }
    public bool IsCellOccupied(int x, int z)
    {
        return grid[x, z].isOccupied;
    }

    public bool IsCellBuildable(int x, int z)
    {
        return grid[x, z].isBuildable;
    }

    public void SetGridVisible(bool visible)
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                if (visualGrid[x, z] != null)
                    visualGrid[x, z].SetActive(visible);
            }
        }
    }

    public Vector3 GetWorldPosition(int x, int z)
    {
        return originPosition + new Vector3(
            x * cellSize + cellSize * 0.5f,
            0f,
            z * cellSize + cellSize * 0.5f
        );
    }

    public bool GetGridPosition(Vector3 worldPosition, out int x, out int z)
    {
        Vector3 localPos = worldPosition - originPosition;

        x = Mathf.FloorToInt(localPos.x / cellSize);
        z = Mathf.FloorToInt(localPos.z / cellSize);

        if (x < 0 || z < 0 || x >= width || z >= height)
            return false;

        return true;
    }

    private void OnDrawGizmos()
    {
        if (grid == null) return;

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                Vector3 pos = GetWorldPosition(x, z);

                Gizmos.color = grid[x, z].isBuildable ? Color.white : Color.red;
                Gizmos.DrawWireCube(pos, new Vector3(cellSize, 0.1f, cellSize));
            }
        }
    }
}
