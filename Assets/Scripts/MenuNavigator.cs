using System;
using UnityEngine;
using UnityEngine.EventSystems;
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
        ControllerJoining.ResetInputUsers();

        // Hook up UI buttons
        settingsButton.onClick.AddListener(() => screenSwitcher.SwitchScreen(ScreenTypes.Settings));
        setupRaceButton.onClick.AddListener(() => screenSwitcher.SwitchScreen(ScreenTypes.Selection));
        settingsBackToMainMenuButton.onClick.AddListener(() => screenSwitcher.SwitchScreen(ScreenTypes.Menu));
        selectionBackToMainMenuButton.onClick.AddListener(() => screenSwitcher.SwitchScreen(ScreenTypes.Menu));
        startRaceButton.onClick.AddListener(StartRace);

        // Subscribe to screen switch event
        screenSwitcher.OnScreenSwitched += HandleScreenSwitched;
    }

    private void OnDestroy()
    {
        // Unsubscribe to avoid memory leaks
        screenSwitcher.OnScreenSwitched -= HandleScreenSwitched;
    }

    private void StartRace()
    {
        if (_playerInputManager != null)
            _playerInputManager.DisableJoining();
        Debug.Log($"Loading scene: {gameplaySceneName}");
        SceneManager.LoadScene(gameplaySceneName);
    }

    // This method sets the default selected button for controller navigation
    private void HandleScreenSwitched(ScreenTypes newScreen)
    {
        Button defaultButton = null;
        switch (newScreen)
        {
            case ScreenTypes.Menu:
                defaultButton = settingsButton; // first button in Main Menu
                break;
            case ScreenTypes.Settings:
                defaultButton = settingsBackToMainMenuButton;
                break;
            case ScreenTypes.Selection:
                defaultButton = startRaceButton;
                break;
        }

        if (defaultButton != null)
        {
            EventSystem.current.SetSelectedGameObject(null); // clear first
            EventSystem.current.SetSelectedGameObject(defaultButton.gameObject);
        }
    }
}

