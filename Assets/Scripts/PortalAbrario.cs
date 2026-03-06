using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalAbrario : MonoBehaviour
{
    [Header("Scene")]
    public string sceneName = "Level2";

    [Header("Requirements")]
    public int killsRequired = 4;

    [Header("State")]
    [SerializeField] private bool isOpen = false;
    
    private KillCounter killCounter;

    private void Start()
    {
        killCounter = KillCounter.Instance;

        if (killCounter == null)
            Debug.LogWarning("Brak KillCounter w scenie! Dodaj KillCounter na GameManager.");
    }

    public void TryOpenByAbrario()
    {
        int kills = (killCounter != null) ? killCounter.Kills : 0;
        int required = (killCounter != null) ? killCounter.killsRequired : killsRequired;

        if (kills < required)
        {
            Debug.Log($"Portal: za ma³o pokonanych przeciwników ({kills}/{required}).");
            return;
        }

        if (isOpen)
        {
            Debug.Log("Portal: ju¿ jest otwarty.");
            return;
        }

        ActivatePortal();
    }

    public void ActivatePortal()
    {
        isOpen = true;
        Debug.Log("Portal activated (test).");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (!isOpen)
        {
            Debug.Log("Portal: zablokowany. Pokonaj 4 przeciwników i u¿yj Abrario.");
            return;
        }

        SceneManager.LoadScene(sceneName);
    }
}