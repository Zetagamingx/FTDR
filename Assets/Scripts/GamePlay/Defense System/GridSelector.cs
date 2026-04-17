using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GridSelector : MonoBehaviour
{
    [SerializeField] private GameObject highlightPrefab;
    [SerializeField] private float gap;
    [SerializeField] private float highlightPadding = 0.02f;
    [SerializeField] private PlacementSystem placementSystem;

    public Camera playerCamera;
    public GridManager grid;

    public LayerMask placementMask;

    private int currentX;
    private int currentZ;
    private bool hasValidCell;

    private List<Transform> highlightTiles = new List<Transform>();
    private List<Renderer> highlightRenderers = new List<Renderer>();

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, placementMask))
        {

            if (grid.GetGridPosition(hit.point, out currentX, out currentZ))
            {
                hasValidCell = true;

                UpdateHighlightVisuals(currentX, currentZ);
                DebugCurrentTile();
                return;
            }
        }

        // If no valid cell  hide all highlights
        hasValidCell = false;
        HideAllHighlights();
    }

    void UpdateHighlightVisuals(int startX, int startZ)
    {
       
        Vector2Int footprint = placementSystem.FootprintSize;

        Vector2Int origin = placementSystem.GetOriginFromCenter(startX, startZ);

        int totalCells = footprint.x * footprint.y;

        UpdateHighlightTiles(totalCells);

        int index = 0;

        for (int x = 0; x < footprint.x; x++)
        {
            for (int z = 0; z < footprint.y; z++)
            {
                int checkX = origin.x + x;
                int checkZ = origin.y + z;

                Transform tile = highlightTiles[index];

                Renderer r = highlightRenderers[index];

                // --- BOUNDS CHECK FIRST ---
                if (checkX < 0 || checkZ < 0 || checkX >= grid.width || checkZ >= grid.height)
                {
                    tile.gameObject.SetActive(false);
                    index++;
                    continue;
                }

                // --- POSITION ---
                Vector3 pos = grid.GetWorldPosition(checkX, checkZ) + Vector3.up * 0.02f;
                tile.position = pos;

                // --- SCALE ---
                float visualSize = grid.cellSize - gap;
                float highlightSize = visualSize - highlightPadding;

                tile.localScale = new Vector3(highlightSize, highlightSize, 1f);

                // --- COLOR ---
                if (!grid.IsCellBuildable(checkX, checkZ) ||
                    grid.IsCellOccupied(checkX, checkZ))
                {
                    r.material.color = Color.red;
                }
                else
                {
                    r.material.color = Color.green;
                }

                // --- ENABLE ---
                tile.gameObject.SetActive(true);

                index++;
            }
        }
    }

    void UpdateHighlightTiles(int requiredCount)
    {
        // Create missing tiles
        while (highlightTiles.Count < requiredCount)
        {
            GameObject tile = Instantiate(highlightPrefab);

            highlightTiles.Add(tile.transform);

            //  Get renderer ONCE here
            Renderer r = tile.GetComponentInChildren<Renderer>();

            //  Force material instance (fixes your previous bug)
            r.material = new Material(r.material);

            highlightRenderers.Add(r);
        }

        // Enable only needed ones
        for (int i = 0; i < highlightTiles.Count; i++)
        {
            highlightTiles[i].gameObject.SetActive(i < requiredCount);
        }
    }

    void HideAllHighlights()
    {
        foreach (var tile in highlightTiles)
        {
            tile.gameObject.SetActive(false);
        }
    }

    void DebugCurrentTile()
    {
        if (!HasValidCell())
            return;

        bool isBuildable = grid.IsCellBuildable(currentX, currentZ);
        bool isOccupied = grid.IsCellOccupied(currentX, currentZ);

        //Debug.Log($"Tile [{currentX}, {currentZ}] | Buildable: {isBuildable} | Occupied: {isOccupied}");
    }

    private void OnDrawGizmos()
    {
        if (!hasValidCell || grid == null) return;

        Vector3 pos = grid.GetWorldPosition(currentX, currentZ);

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(pos, new Vector3(grid.cellSize, 0.2f, grid.cellSize));
    }

    public int CurrentX => currentX;
    public int CurrentZ => currentZ;

    public bool HasValidCell()
    {
        return hasValidCell;
    }

}