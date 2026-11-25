
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public Transform player;
    public Transform twin;

    public GameObject playerExit;
    public GameObject twinExit;

    public int totalGems = 6;

    private Vector3 playerStart;
    private Vector3 twinStart;

    private int collectedGems;

    private bool playerAtExit;
    private bool twinAtExit;
    private bool hasWon;

    public int CollectedGems => collectedGems;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        playerStart = player.position;
        twinStart = twin.position;

        playerExit.SetActive(false);
        twinExit.SetActive(false);
    }

    public void RegisterGem()
    {
        collectedGems++;

        Debug.Log("Gems: " + collectedGems + "/" + totalGems);

        if (collectedGems >= totalGems)
        {
            playerExit.SetActive(true);
            twinExit.SetActive(true);

            Debug.Log("Both exits are open!");
        }
    }

    public void ResetCharacters()
    {
        ResetCharacter(player, playerStart);
        ResetCharacter(twin, twinStart);

        playerAtExit = false;
        twinAtExit = false;

        Debug.Log("Both characters reset!");
    }

    private void ResetCharacter(Transform character, Vector3 startPosition)
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

    public void SetPlayerAtExit(bool value)
    {
        playerAtExit = value;
        CheckWin();
    }

    public void SetTwinAtExit(bool value)
    {
        twinAtExit = value;
        CheckWin();
    }

    private void CheckWin()
    {
        if (hasWon)
            return;

        if (collectedGems >= totalGems &&
            playerAtExit && twinAtExit)
        {
            hasWon = true;
            Debug.Log("YOU WIN!");
        }
    }
}
