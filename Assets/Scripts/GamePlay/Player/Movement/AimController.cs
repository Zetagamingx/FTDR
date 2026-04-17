using UnityEngine;
using UnityEngine.InputSystem;


public class AimController : MonoBehaviour
{
    [SerializeField] public float mouseSensitivity;
    [SerializeField] public float pitch;
    [SerializeField] public float minPitch;
    [SerializeField] public float maxPitch;
    [SerializeField] public float yaw = 0f;

    public InputActionReference lookAction;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnEnable()
    {
        lookAction.action.Enable();
    }

    private void OnDisable()
    {
        lookAction.action.Disable();
    }

    void Update()
    {
        Vector2 mouseDelta = lookAction.action.ReadValue<Vector2>();

        yaw += mouseDelta.x * mouseSensitivity;
        pitch -= mouseDelta.y * mouseSensitivity;

        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
    }
}
