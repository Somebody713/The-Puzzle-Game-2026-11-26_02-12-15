
using UnityEngine;

public class GemPickup : MonoBehaviour
{
    private bool collected = false;

    private void OnTriggerEnter(Collider other)
    {
        if (collected)
            return;

        bool isPlayer =
            other.GetComponentInParent<PlayerMovement>() != null;

        bool isTwin =
            other.GetComponentInParent<MirrorMovement>() != null;

        if (!isPlayer && !isTwin)
            return;

        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager not found!");
            return;
        }

        collected = true;

        GameManager.Instance.RegisterGem();

        Destroy(gameObject);
    }
}
