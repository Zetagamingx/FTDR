using UnityEngine;
using UnityEngine.InputSystem;

public class AimV2 : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity;
    [SerializeField] private float minPitch;
    [SerializeField] private float maxPitch;

    public InputActionReference lookAction;

    public Transform weaponVisual; // child object (the visible weapon)
    private Rigidbody rb;

    private float pitch;
    private float yaw;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Vector2 mouseDelta = lookAction.action.ReadValue<Vector2>();

        yaw += mouseDelta.x * mouseSensitivity;
        pitch -= mouseDelta.y * mouseSensitivity;

        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        // Apply FULL rotation to weapon (visual)
        weaponVisual.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }

    void FixedUpdate()
    {
        // Apply ONLY yaw to physics body
        rb.MoveRotation(Quaternion.Euler(0f, yaw, 0f));
    }
}

