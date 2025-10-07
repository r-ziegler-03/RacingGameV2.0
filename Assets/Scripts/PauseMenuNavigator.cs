using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuNavigator : MonoBehaviour
{
    [SerializeField] private ScreenSwitcher screenSwitcher;
    [SerializeField] private Button settingsScreenButton;
    [SerializeField] private Button returnToPauseMenuButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private String mainMenuSceneName = "MainMenu";
    [SerializeField] private PauseScreenHandler pauseScreenHandler;
    [SerializeField] private Button gameOverReturnToMainMenuButton;
    


    private void Awake()
    {
        settingsScreenButton.onClick.AddListener((() => screenSwitcher.SwitchScreen(ScreenTypes.Settings)));
        returnToPauseMenuButton.onClick.AddListener((() => screenSwitcher.SwitchScreen(ScreenTypes.Pause)));
        mainMenuButton.onClick.AddListener((() => SceneManager.LoadScene(mainMenuSceneName)));
        gameOverReturnToMainMenuButton.onClick.AddListener((() => SceneManager.LoadScene(mainMenuSceneName)));
        resumeButton.onClick.AddListener((() => pauseScreenHandler.TogglePauseScreen()));
    }
}
