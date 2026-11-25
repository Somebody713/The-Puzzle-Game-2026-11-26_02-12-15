
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class MirrorMovement : MonoBehaviour
{
    public PlayerMovement player;

    private Rigidbody rb;
    private Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        // Twin uses Player's Root Motion velocity.
        animator.applyRootMotion = false;

        rb.isKinematic = false;
        rb.useGravity = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void Update()
    {
        if (player == null)
            return;

        float speed = player.MoveInput == Vector2.zero
            ? 0f
            : player.IsRunning ? 1f : 0.5f;

        animator.SetFloat("Speed", speed);

        // Twin faces the opposite direction (180 degrees).
        animator.SetFloat("Direction", -player.MoveInput.x);
    }

    private void FixedUpdate()
    {
        if (player == null)
            return;

        Vector3 playerVelocity = player.RootMotionVelocity;

        Vector3 velocity = rb.linearVelocity;

        // Mirror only horizontal movement.
        velocity.x = -playerVelocity.x;
        velocity.z = playerVelocity.z;

        rb.linearVelocity = velocity;
    }
}
