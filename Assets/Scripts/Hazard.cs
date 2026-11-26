
using UnityEngine;

public class Hazard : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        bool isPlayer =
            other.GetComponentInParent<PlayerMovement>() != null;

        bool isTwin =
            other.GetComponentInParent<MirrorMovement>() != null;

        if (!isPlayer && !isTwin)
            return;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.ResetCharacters();
        }
    }
}
