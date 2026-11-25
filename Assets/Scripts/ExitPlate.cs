
using UnityEngine;

public class ExitPlate : MonoBehaviour
{
    public bool isPlayerExit;

    private void OnTriggerEnter(Collider other)
    {
        CheckCharacter(other, true);
    }

    private void OnTriggerExit(Collider other)
    {
        CheckCharacter(other, false);
    }

    private void CheckCharacter(Collider other, bool entered)
    {
        bool isPlayer =
            other.GetComponentInParent<PlayerMovement>() != null ||
            other.GetComponentInChildren<PlayerMovement>() != null;

        bool isTwin =
            other.GetComponentInParent<MirrorMovement>() != null ||
            other.GetComponentInChildren<MirrorMovement>() != null;

        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager not found!");
            return;
        }

        if (isPlayerExit && isPlayer)
        {
            GameManager.Instance.SetPlayerAtExit(entered); 
        }

        if (!isPlayerExit && isTwin)
        {
            GameManager.Instance.SetTwinAtExit(entered);
        }
    }
}
