using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] private GridManager grid;
    [SerializeField] private GridSelector selector;
    [SerializeField] private GameObject objectToPlacePrefab;

    [SerializeField] private List<GameObject> placeablePrefabs;
    private int currentIndex;

    [Header("PrefabSize")]
    private Vector2Int footprintSize;
    private PlaceableObjectData currentData;

    private bool isRotated = false;
    //public Vector2Int FootprintSize => footprintSize;

    [Header("Input")]
    [SerializeField] private InputActionReference placeAction;
    [SerializeField] private InputActionReference rotateAction;

    [Header("ObjectPreview")]
    private GameObject previewInstance;

    public Vector2Int FootprintSize
    {
        get
        {
            return isRotated
                ? new Vector2Int(footprintSize.y, footprintSize.x)
                : footprintSize;
        }
    }

    public Vector2Int GetOriginFromCenter(int centerX, int centerZ)
    {
        Vector2Int footprint = FootprintSize;

        int originX = centerX - (footprint.x / 2);
        int originZ = centerZ - (footprint.y / 2);

        return new Vector2Int(originX, originZ);
    }

    void Start()
    {
        if (placeablePrefabs.Count > 0)
        {
            SelectObject(0);
        }

        CreatePreview();
    }

    private void OnEnable()
    {
        placeAction.action.Enable();
        placeAction.action.performed += OnPlacePerformed;

        rotateAction.action.Enable();
        rotateAction.action.performed += OnRotatePerformed;

        PlacementEvents.OnSelectPlacementObject += SelectObject;
    }

    private void OnDisable()
    {
        placeAction.action.performed -= OnPlacePerformed;
        placeAction.action.Disable();

        rotateAction.action.Disable();
        rotateAction.action.performed -= OnRotatePerformed;

        PlacementEvents.OnSelectPlacementObject -= SelectObject;
    }

    private void OnPlacePerformed(InputAction.CallbackContext context)
    {
        TryPlaceObject();
    }

    private void OnRotatePerformed(InputAction.CallbackContext context)
    {
        isRotated = !isRotated;
    }

    public void SelectObject(int index)
    {
        if (index < 0 || index >= placeablePrefabs.Count)
        {
            Debug.LogError($"Invalid index: {index}");
            return;
        }

        currentIndex = index;
        objectToPlacePrefab = placeablePrefabs[index];

        currentData = objectToPlacePrefab.GetComponent<PlaceableObjectData>();
       
        if (currentData == null)
        {
            Debug.LogError("Prefab missing PlaceableObjectData!");
            return;
        }

        footprintSize = currentData.footprintSize;

        isRotated = false;

        CreatePreview();
    }

    void CreatePreview()
    {
        if (previewInstance != null)
        {
            Destroy(previewInstance);
        }

        previewInstance = Instantiate(objectToPlacePrefab);

        // Disable all colliders (IMPORTANT)
        foreach (var col in previewInstance.GetComponentsInChildren<Collider>())
        {
            col.enabled = false;
        }

        // Apply initial rotation
        Quaternion rot = isRotated
            ? Quaternion.Euler(0, 90, 0)
            : Quaternion.identity;

        previewInstance.transform.rotation = rot;
    }

    void TryPlaceObject()
    {
        if (!selector.HasValidCell())
            return;

        int centerX = selector.CurrentX;
        int centerZ = selector.CurrentZ;

        Vector2Int origin = GetOriginFromCenter(centerX, centerZ);

        int startX = origin.x;
        int startZ = origin.y;

        //  use the helper
        if (!CanPlace(centerX, centerZ))
            return;

        Vector3 basePos = grid.GetWorldPosition(startX, startZ);

        Vector2Int footprint = FootprintSize;

        Vector3 offset = new Vector3(
            (footprint.x - 1) * grid.cellSize * 0.5f,
            0,
            (footprint.y - 1) * grid.cellSize * 0.5f
        );

        Vector3 finalPos = basePos + offset;

        Quaternion rotation = isRotated ? Quaternion.Euler(0f, 90f, 0f) : Quaternion.identity;

        Instantiate(objectToPlacePrefab, finalPos, rotation);
                
        // Mark all cells occupied
        for (int x = 0; x < footprint.x; x++)
        {
            for (int z = 0; z < footprint.y; z++)
            {
                grid.SetCellOccupied(startX + x, startZ + z, true);
            }
        }
    }

    bool CanPlace(int centerX, int centerZ)
    {
        Vector2Int footprint = FootprintSize;
        Vector2Int origin = GetOriginFromCenter(centerX, centerZ);


        for (int x = 0; x < footprint.x; x++)
        {
            for (int z = 0; z < footprint.y; z++)
            {
                int checkX = origin.x + x;
                int checkZ = origin.y + z;

                if (!grid.IsCellBuildable(checkX, checkZ))
                    return false;

                if (grid.IsCellOccupied(checkX, checkZ))
                    return false;
            }
        }

        return true;
    }

    private void Update()
    {
        if (previewInstance == null)
            return;

        if (!selector.HasValidCell())
        {
            previewInstance.SetActive(false);
            return;
        }

        previewInstance.SetActive(true);

        int x = selector.CurrentX;
        int z = selector.CurrentZ;

        Vector2Int origin = GetOriginFromCenter(x, z);

        Vector3 basePos = grid.GetWorldPosition(origin.x, origin.y);

        Vector2Int footprint = FootprintSize;

        Vector3 offset = new Vector3(
            (footprint.x - 1) * grid.cellSize * 0.5f,
            0,
            (footprint.y - 1) * grid.cellSize * 0.5f
        );

        Vector3 finalPos = basePos + offset;

        Quaternion rot = isRotated
            ? Quaternion.Euler(0, 90, 0)
            : Quaternion.identity;

        previewInstance.transform.SetPositionAndRotation(finalPos, rot);

        // === COLOR FEEDBACK ===
        bool canPlace = CanPlace(x, z);

        Renderer[] renderers = previewInstance.GetComponentsInChildren<Renderer>();

        Renderer r = previewInstance.GetComponentInChildren<Renderer>();

        Material targetMat = canPlace
            ? currentData.materials[0]
            : currentData.materials[1];

        Material[] mats = r.sharedMaterials;

        for (int i = 0; i < mats.Length; i++)
        {
            mats[i] = targetMat;
        }

        r.sharedMaterials = mats;
    }
}