using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuNavigator : MonoBehaviour
{
    
    [SerializeField] private ScreenSwitcher screenSwitcher;
    private PlayerInputManager playerInputManager;
    [SerializeField] private TextMeshProUGUI playerJoinedText;

    // Main Menu
    [Header("Main Menu")]
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button setupRaceButton;
    // Settings Menu
    [Header("Settings Menu")]
    [SerializeField] private Button settingsBackToMainMenuButton;
    // Selection Menu
    [Header("Selection Menu")]
    [SerializeField] private Button selectionBackToMainMenuButton;
    [SerializeField] private Button startRaceButton;
    [SerializeField] private Button raceThreeLapsButton;
    [SerializeField] private Button raceSixLapsButton;
    [SerializeField] private Button raceNineLapsButton;
    [SerializeField] private TextMeshProUGUI instructionsText;

    [SerializeField] private string gameplaySceneName = "Gameplay";

    private void Awake()
    {
        StartCoroutine(WaitForControllerJoining());
    }
    
    private IEnumerator WaitForControllerJoining()
    {
        // Wait until ControllerJoining exists
        while (ControllerJoining.Instance == null)
            yield return null;

        // Now safe to subscribe
        ControllerJoining.Instance.OnPlayerJoinedNotification += PlayerJoinedNotification;

        // Now run your other Awake setup
        FinishAwake();
    }
    
    private void FinishAwake()
    {
        playerInputManager = FindFirstObjectByType<PlayerInputManager>();
        ControllerJoining.ResetInputUsers();
        instructionsText.enabled = false;
        playerJoinedText.enabled = false;

        settingsButton.onClick.AddListener(() => screenSwitcher.SwitchScreen(ScreenTypes.Settings));
        setupRaceButton.onClick.AddListener(() => screenSwitcher.SwitchScreen(ScreenTypes.Selection));
        settingsBackToMainMenuButton.onClick.AddListener(() => screenSwitcher.SwitchScreen(ScreenTypes.Menu));
        selectionBackToMainMenuButton.onClick.AddListener(() => screenSwitcher.SwitchScreen(ScreenTypes.Menu));
        startRaceButton.onClick.AddListener(StartRace);
        raceThreeLapsButton.onClick.AddListener(() => StoreLapCount(raceThreeLapsButton));
        raceSixLapsButton.onClick.AddListener(() => StoreLapCount(raceSixLapsButton));
        raceNineLapsButton.onClick.AddListener(() => StoreLapCount(raceNineLapsButton));

        screenSwitcher.OnScreenSwitched += HandleScreenSwitchedDefualtButton;
    }


    private void PlayerJoinedNotification()
    {
        StartCoroutine(PlayerJoinedAlert());
    }

    private IEnumerator PlayerJoinedAlert()
    {
        playerJoinedText.enabled = true;
        yield return new WaitForSeconds(2f);
        playerJoinedText.enabled = false;
    }

    private void OnDestroy()
    {
        // Unsubscribe to avoid memory leaks
        screenSwitcher.OnScreenSwitched -= HandleScreenSwitchedDefualtButton;
        if (ControllerJoining.Instance != null)
            ControllerJoining.Instance.OnPlayerJoinedNotification -= PlayerJoinedNotification;
    }

    private void StartRace()
    {
        if ((ControllerJoining.JoinedPlayers.Count != 0) & (ControllerJoining.Instance.lapCount != 0))
        {
            if (playerInputManager != null)
                playerInputManager.DisableJoining();
            Debug.Log($"Loading scene: {gameplaySceneName}");
            SceneManager.LoadScene(gameplaySceneName);
        }
        else
        {
            StartCoroutine(PlayerFailedToSelectLapOrPair());
        }
    }

    private void StoreLapCount(Button button)
    {
        TextMeshProUGUI numberFromButton = button.GetComponentInChildren<TextMeshProUGUI>();
        int lapCount = int.Parse(numberFromButton.text);
        ControllerJoining.Instance.lapCount = lapCount;
    }
    
    private void HandleScreenSwitchedDefualtButton(ScreenTypes newScreen)
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

    private IEnumerator PlayerFailedToSelectLapOrPair()
    {
        instructionsText.enabled = true;
        yield return new WaitForSeconds(3f);
        instructionsText.enabled = false;
    }
}

