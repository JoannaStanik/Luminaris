using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{

    // Obiekty konkretnych ekranów gry
    [Header("Panels")]
    [SerializeField] private GameObject panelStart;
    [SerializeField] private GameObject panelInstructions;
    [SerializeField] private GameObject panelHUD;
    [SerializeField] private GameObject panelPause;
    [SerializeField] private GameObject panelGameOver;

    private bool isPaused = false;
    private bool gameStarted = false;
    private bool isGameOver = false;

    public PlayerHealth playerHealth;

    private void Start()
    {
        ShowStart();
        Time.timeScale = 0f;
    }

    private void Update()
    {
        if (!gameStarted) return;

        // Menu pauzy po wciœniêciu klawisza Escape
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
        isGameOver = false;
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
        if (isGameOver) return;

        isPaused = false;
        Time.timeScale = 1f;
        panelPause.SetActive(false);
    }

    public void OnClickPauseInstructions()
    {
        panelPause.SetActive(false);
        panelInstructions.SetActive(true);
    }

    // ---------- GAME OVER -----------
    public void ShowGameOver()
    {
        isGameOver = true;
        isPaused = false;

        if (panelHUD != null) panelHUD.SetActive(false);
        if (panelPause != null) panelPause.SetActive(false);
        if (panelInstructions != null) panelInstructions.SetActive(false);
        if (panelStart != null) panelStart.SetActive(false);

        if (panelGameOver != null) panelGameOver.SetActive(true);

        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void OnClickRestart()
    {
        Time.timeScale = 0f;

        isPaused = false;
        isGameOver = false;
        gameStarted = false;

        if (panelHUD != null) panelHUD.SetActive(false);
        if (panelPause != null) panelPause.SetActive(false);
        if (panelInstructions != null) panelInstructions.SetActive(false);
        if (panelGameOver != null) panelGameOver.SetActive(false);

        ShowStart();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (playerHealth != null)
            playerHealth.ResetPlayer();

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
