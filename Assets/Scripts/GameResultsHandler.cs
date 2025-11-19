using System;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameResultsHandler : MonoBehaviour
{
    [SerializeField] private ScreenSwitcher screenSwitcher;
    [SerializeField] private TextMeshProUGUI resultsToText;
    [SerializeField] private LapProgress lapProgress;
    [SerializeField] private Button gameOverReturnToMainMenuButton;
    [SerializeField] private CanvasGroup raceStandingsUI;
    [SerializeField] private String mainMenuSceneName = "MainMenu";

    public void Awake()
    {
        gameOverReturnToMainMenuButton?.onClick.AddListener(() => SceneManager.LoadScene(mainMenuSceneName));
    }

    public void handleGameOver()
    {
        screenSwitcher.SwitchScreen(ScreenTypes.GameOver);
        AudioListener.pause = true;
        raceStandingsUI.gameObject.SetActive(false);
        Time.timeScale = 0;
        ResultsToText();

    }

    private void ResultsToText()
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine($"Race Results: ");
        for (int i = 0; i < lapProgress.playersResults.Count; i++)
        {
            stringBuilder.AppendLine($"{i + 1} --> {lapProgress.playersResults[i].playerName}");
        }
        resultsToText.text = stringBuilder.ToString();
    }
}
