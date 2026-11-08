using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject gameOverPanel;
    public GameObject victoryPanel;

    private void Start()
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (victoryPanel != null) victoryPanel.SetActive(false);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameOver += ShowGameOverScreen;
            GameManager.Instance.OnGameWin += ShowVictoryScreen;
        }
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameOver -= ShowGameOverScreen;
            GameManager.Instance.OnGameWin -= ShowVictoryScreen;
        }
    }

    private void ShowGameOverScreen()
    {
        gameOverPanel?.SetActive(true);
    }

    private void ShowVictoryScreen()
    {
        victoryPanel?.SetActive(true);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        Scene activeScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(activeScene.name);
    }
}