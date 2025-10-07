using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuNavigator : MonoBehaviour
{
    [SerializeField] private ScreenSwitcher screenSwitcher;
    
    //Main Menu
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button setupRaceButton;
    //Settings Menu
    [SerializeField] private Button settingsBackToMainMenuButton;
    //Selection Menu
    [SerializeField] private Button selectionBackToMainMenuButton;
    [SerializeField] private Button startRaceButton;
    
    [SerializeField] private String gameplaySceneName = "Gameplay";


    private void Awake()
    {
        settingsButton.onClick.AddListener(() => screenSwitcher.SwitchScreen(ScreenTypes.Settings));
        setupRaceButton.onClick.AddListener(() => screenSwitcher.SwitchScreen(ScreenTypes.Selection));
        settingsBackToMainMenuButton.onClick.AddListener(() => screenSwitcher.SwitchScreen(ScreenTypes.Menu));
        selectionBackToMainMenuButton.onClick.AddListener(() => screenSwitcher.SwitchScreen(ScreenTypes.Menu));
        startRaceButton.onClick.AddListener(() => SceneManager.LoadScene(gameplaySceneName));
    }
}
