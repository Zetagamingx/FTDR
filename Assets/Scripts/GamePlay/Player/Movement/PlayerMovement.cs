using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;

    public InputActionReference moveAction;

    private Rigidbody rb;
    private Vector2 input;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        moveAction.action.Enable();
    }

    private void OnDisable()
    {
        moveAction.action.Disable();
    }

    private void Update()
    {
        // Read input in Update (input system best practice)
        input = moveAction.action.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        // Flatten forward/right so movement stays on ground plane
        Vector3 forward = Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized;
        Vector3 right = Vector3.ProjectOnPlane(transform.right, Vector3.up).normalized;

        Vector3 moveDirection = forward * input.y + right * input.x;

        Vector3 velocity = moveDirection * moveSpeed;

        // Preserve vertical velocity (gravity, jumps later)
        velocity.y = rb.linearVelocity.y;

        rb.linearVelocity = velocity;
    }
}