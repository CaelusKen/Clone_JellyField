using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public TMP_Text levelText;
    public TMP_Text jellyObjectiveText;

    public GameObject gameOverPanel;
    public GameObject pausePanel;
    public GameObject completeLevelPanel;

    private bool isPaused = false;

    private void Start()
    {
        HideAllPanels();
    }

    public void UpdateLevelUI(int level)
    {
        levelText.text = "Level: " + level;
    }

    public void UpdateJellyObjective(Dictionary<JellyColor, int> collected, Dictionary<JellyColor, int> total)
    {
        // Display objectives in the format "Collect X Red, Y Blue"
        string objectiveText = "Goals:\n";
        foreach (var objective in total)
        {
            JellyColor color = objective.Key;
            int requiredAmount = objective.Value;
            int collectedAmount = collected.ContainsKey(color) ? collected[color] : 0;

            objectiveText += $"{color}: {collectedAmount}/{requiredAmount}\n";
        }
        jellyObjectiveText.text = objectiveText;
    }

    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
    }

    public void HideGameOver()
    {
        gameOverPanel.SetActive(false);
    }

    public void ShowCompleteLevelPanel()
    {
        completeLevelPanel.SetActive(true);
    }

    public void HideCompleteLevelPanel()
    {
        completeLevelPanel.SetActive(false);
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        pausePanel.SetActive(isPaused);

        if (isPaused)
        {
            Time.timeScale = 0f; // Pause game
        }
        else
        {
            Time.timeScale = 1f; // Resume game
        }
    }

    public void RetryLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitToHome()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("HomeScene"); // Ensure this is your home scene's actual name
    }

    private void HideAllPanels()
    {
        gameOverPanel.SetActive(false);
        pausePanel.SetActive(false);
        completeLevelPanel.SetActive(false);
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
