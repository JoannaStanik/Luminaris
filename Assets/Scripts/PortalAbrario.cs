using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalAbrario : MonoBehaviour
{
    public string sceneName = "Level2";

    public void ActivatePortal()
    {
        Debug.Log("Portal activated (test).");
    }

    private void OnTriggerEnter(Collider other)
    {
        SceneManager.LoadScene(sceneName);
    }
}