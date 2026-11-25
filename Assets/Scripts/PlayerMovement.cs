
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Root Motion Speed Limits")]
    [SerializeField] private float maxWalkSpeed = 4f;
    [SerializeField] private float maxRunSpeed = 7f;

    private Rigidbody rb;
    private Animator animator;

    private Vector2 moveInput;
    private bool isRunning;

    private Vector3 rootMotionVelocity;

    public Vector2 MoveInput => moveInput;
    public bool IsRunning => isRunning;
    public Vector3 RootMotionVelocity => rootMotionVelocity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        animator.applyRootMotion = true;

        rb.isKinematic = false;
        rb.useGravity = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void Update()
    {
        moveInput = Vector2.zero;
        isRunning = false;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed)
                moveInput.y += 1;

            if (Keyboard.current.sKey.isPressed)
                moveInput.y -= 1;

            if (Keyboard.current.aKey.isPressed)
                moveInput.x -= 1;

            if (Keyboard.current.dKey.isPressed)
                moveInput.x += 1;

            isRunning = Keyboard.current.leftShiftKey.isPressed;
        }

        moveInput = Vector2.ClampMagnitude(moveInput, 1f);

        float speed = moveInput == Vector2.zero
            ? 0f
            : isRunning ? 1f : 0.5f;

        animator.SetFloat("Speed", speed);
        animator.SetFloat("Direction", moveInput.x);
    }

    private void OnAnimatorMove()
    {
        if (Time.deltaTime <= 0f || moveInput.sqrMagnitude < 0.001f)
        {
            rootMotionVelocity = Vector3.zero;
            return;
        }

        Vector3 localDelta =
            transform.InverseTransformDirection(animator.deltaPosition);

        // Reverse forward root motion when pressing S.
        if (Mathf.Abs(moveInput.y) > 0.1f)
        {
            localDelta.z =
                Mathf.Abs(localDelta.z) * Mathf.Sign(moveInput.y);
        }

        Vector3 worldDelta =
            transform.TransformDirection(localDelta);

        worldDelta.y = 0f;

        Vector3 velocity = worldDelta / Time.deltaTime;

        float maxSpeed = isRunning ? maxRunSpeed : maxWalkSpeed;

        // Prevent sudden root motion spikes.
        rootMotionVelocity =
            Vector3.ClampMagnitude(velocity, maxSpeed);
    }

    private void FixedUpdate()
    {
        Vector3 velocity = rb.linearVelocity;

        velocity.x = rootMotionVelocity.x;
        velocity.z = rootMotionVelocity.z;

        rb.linearVelocity = velocity;
    }
}
