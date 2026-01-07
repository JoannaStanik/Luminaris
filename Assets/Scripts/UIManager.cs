using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject panelStart;
    [SerializeField] private GameObject panelInstructions;
    [SerializeField] private GameObject panelHUD;
    [SerializeField] private GameObject panelPause;

    private bool isPaused = false;
    private bool gameStarted = false;

    private void Start()
    {
        ShowStart();
        Time.timeScale = 0f;
    }

    private void Update()
    {
        if (!gameStarted) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    // ---------- START FLOW ----------

    public void OnClickStart()
    {
        panelStart.SetActive(false);
        panelInstructions.SetActive(true);
    }

    public void OnClickOk()
    {
        panelInstructions.SetActive(false);
        panelHUD.SetActive(true);

        gameStarted = true;
        Time.timeScale = 1f;
    }

    // ---------- PAUSE FLOW ----------

    private void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        panelPause.SetActive(true);
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        panelPause.SetActive(false);
    }

    public void OnClickPauseInstructions()
    {
        panelPause.SetActive(false);
        panelInstructions.SetActive(true);
    }

    // ---------- EXIT ----------

    public void OnClickExit()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private void ShowStart()
    {
        panelStart.SetActive(true);
        panelInstructions.SetActive(false);
        panelHUD.SetActive(false);
        panelPause.SetActive(false);
    }
}
