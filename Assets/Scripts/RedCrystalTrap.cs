
using UnityEngine;

public class RedCrystalTrap : MonoBehaviour
{
    public PlayerMovement player;
    public MirrorMovement twin;

    private Vector3 playerStart;
    private Vector3 twinStart;

    void Start()
    {
        playerStart = player.transform.position;
        twinStart = twin.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        bool hitPlayer =
            other.GetComponentInParent<PlayerMovement>() != null;

        bool hitTwin =
            other.GetComponentInParent<MirrorMovement>() != null;

        if (hitPlayer || hitTwin)
        {
            ResetCharacter(player.transform, playerStart);
            ResetCharacter(twin.transform, twinStart);

            Debug.Log("Red crystal touched! Both characters reset.");
        }
    }

    void ResetCharacter(Transform character, Vector3 startPosition)
    {
        Rigidbody rb = character.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.position = startPosition;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
        else
        {
            character.position = startPosition;
        }
    }
}
