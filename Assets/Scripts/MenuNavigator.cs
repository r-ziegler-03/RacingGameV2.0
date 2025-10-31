using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuNavigator : MonoBehaviour
{
    [SerializeField] private ScreenSwitcher screenSwitcher;
    private PlayerInputManager _playerInputManager;

    // Main Menu
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button setupRaceButton;
    // Settings Menu
    [SerializeField] private Button settingsBackToMainMenuButton;
    // Selection Menu
    [SerializeField] private Button selectionBackToMainMenuButton;
    [SerializeField] private Button startRaceButton;

    [SerializeField] private string gameplaySceneName = "Gameplay";

    private void Awake()
    {
        _playerInputManager = FindFirstObjectByType(typeof(PlayerInputManager)) as PlayerInputManager;
        
        

        // Hook up UI buttons
        settingsButton.onClick.AddListener(() => screenSwitcher.SwitchScreen(ScreenTypes.Settings));
        setupRaceButton.onClick.AddListener(() => screenSwitcher.SwitchScreen(ScreenTypes.Selection));
        settingsBackToMainMenuButton.onClick.AddListener(() => screenSwitcher.SwitchScreen(ScreenTypes.Menu));
        selectionBackToMainMenuButton.onClick.AddListener(() => screenSwitcher.SwitchScreen(ScreenTypes.Menu));
        startRaceButton.onClick.AddListener(StartRace);
    }

    private void StartRace()
    {
        if (_playerInputManager != null)
            _playerInputManager.DisableJoining();
        Debug.Log($"Loading scene: {gameplaySceneName}");
        SceneManager.LoadScene(gameplaySceneName);
    }
}