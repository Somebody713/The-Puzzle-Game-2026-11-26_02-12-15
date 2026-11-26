
using UnityEngine;

[DefaultExecutionOrder(-90)]
[RequireComponent(typeof(Rigidbody), typeof(Animator))]
public class MirrorMovement : MonoBehaviour
{
    [Header("Mirror")]
    public PlayerMovement player;

    [SerializeField] private bool mirrored = true;

    [Header("Animation")]
    [SerializeField] private float dampTime = 0.1f;
    [SerializeField] private float speedMultiplier = 1.6f;

    private static readonly int MoveX =
        Animator.StringToHash("MoveX");

    private static readonly int MoveY =
        Animator.StringToHash("MoveY");

    private Rigidbody rb;
    private Animator anim;

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

    private void Update()
    {
        if (player == null)
        {
            anim.SetFloat(MoveX, 0f);
            anim.SetFloat(MoveY, 0f);
            return;
        }

        Vector2 input = player.InputEnabled
            ? player.MoveInput
            : Vector2.zero;

        bool running = player.InputEnabled && player.IsRunning;

        float x = input.x * 0.5f;

        if (mirrored)
        {
            x = -x;
        }

        float y = input.y * 0.5f;

        if (running && input.y > 0f)
        {
            y = input.y;
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

        // Keep gravity independent of Root Motion.
        velocity.y = rb.linearVelocity.y;

        rb.linearVelocity = velocity;
    }

    public void ResetTo(Vector3 position)
    {
        rb.linearVelocity = Vector3.zero;
        rb.position = position;
        transform.position = position;

        anim.SetFloat(MoveX, 0f);
        anim.SetFloat(MoveY, 0f);
    }
}
