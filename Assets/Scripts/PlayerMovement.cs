
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-100)]
[RequireComponent(typeof(Rigidbody), typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputActionReference move;
    [SerializeField] private InputActionReference sprint;

    [Header("Animation")]
    [SerializeField] private float dampTime = 0.1f;
    [SerializeField] private float speedMultiplier = 1.6f;

    private static readonly int MoveX =
        Animator.StringToHash("MoveX");

    private static readonly int MoveY =
        Animator.StringToHash("MoveY");

    private Rigidbody rb;
    private Animator anim;

    private Vector2 moveInput;
    private bool running;

    public bool InputEnabled { get; set; } = true;

    public Vector2 MoveInput => moveInput;
    public bool IsRunning => running;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        anim.applyRootMotion = true;
        anim.speed = speedMultiplier;

        rb.isKinematic = false;
        rb.useGravity = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode =
            CollisionDetectionMode.ContinuousDynamic;
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        CapsuleCollider capsule = GetComponent<CapsuleCollider>();

        if (capsule != null)
        {
            capsule.material = new PhysicsMaterial("NoFriction")
            {
                dynamicFriction = 0f,
                staticFriction = 0f,
                frictionCombine = PhysicsMaterialCombine.Minimum
            };
        }
    }

    private void OnEnable()
    {
        if (move != null && move.action != null)
            move.action.actionMap?.Enable();

        if (sprint != null && sprint.action != null)
            sprint.action.actionMap?.Enable();
    }

    private void OnDisable()
    {
        if (move != null && move.action != null)
            move.action.actionMap?.Disable();
    }

    private void Update()
    {
        moveInput = InputEnabled && move != null
            ? move.action.ReadValue<Vector2>()
            : Vector2.zero;

        running = InputEnabled &&
                  sprint != null &&
                  sprint.action.IsPressed();

        moveInput = Vector2.ClampMagnitude(moveInput, 1f);

        float x = moveInput.x * 0.5f;
        float y = moveInput.y * 0.5f;

        if (running && moveInput.y > 0f)
        {
            y = moveInput.y;
        }

        anim.SetFloat(MoveX, x, dampTime, Time.deltaTime);
        anim.SetFloat(MoveY, y, dampTime, Time.deltaTime);
    }

    private void OnAnimatorMove()
    {
        float dt = anim.updateMode == AnimatorUpdateMode.Fixed
            ? Time.fixedDeltaTime
            : Time.deltaTime;

        if (dt <= 0f)
            return;

        Vector3 velocity = anim.deltaPosition / dt;

        // Gravity remains controlled by Rigidbody.
        velocity.y = rb.linearVelocity.y;

        rb.linearVelocity = velocity;
    }

    public void ResetTo(Vector3 position)
    {
        rb.linearVelocity = Vector3.zero;
        rb.position = position;
        transform.position = position;

        moveInput = Vector2.zero;
        running = false;

        anim.SetFloat(MoveX, 0f);
        anim.SetFloat(MoveY, 0f);
    }
}
