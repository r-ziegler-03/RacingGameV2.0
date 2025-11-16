using System;
using UnityEngine;
using UnityEngine.EventSystems;
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
        settingsScreenButton?.onClick.AddListener((() => screenSwitcher.SwitchScreen(ScreenTypes.Settings)));
        returnToPauseMenuButton?.onClick.AddListener((() => screenSwitcher.SwitchScreen(ScreenTypes.Pause)));
        mainMenuButton?.onClick.AddListener((() => SceneManager.LoadScene(mainMenuSceneName)));
        gameOverReturnToMainMenuButton?.onClick.AddListener((() => SceneManager.LoadScene(mainMenuSceneName)));
        resumeButton?.onClick.AddListener((() => pauseScreenHandler.TogglePauseScreen()));
        
        screenSwitcher.OnScreenSwitched += HandleScreenSwitched;
    }
    
    private void OnDestroy()
    {
        // Unsubscribe to avoid memory leaks
        screenSwitcher.OnScreenSwitched -= HandleScreenSwitched;
    }
    
    private void HandleScreenSwitched(ScreenTypes newScreen)
    {
        Button defaultButton = null;
        switch (newScreen)
        {
            case ScreenTypes.Pause:
                defaultButton = resumeButton; // first button in Main Menu
                break;
            case ScreenTypes.Settings:
                defaultButton = returnToPauseMenuButton;
                break;
        }

        if (defaultButton != null)
        {
            EventSystem.current.SetSelectedGameObject(null); // clear first
            EventSystem.current.SetSelectedGameObject(defaultButton.gameObject);
        }
    }
}
