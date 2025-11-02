using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class LobbyPlayerManager : MonoBehaviour
{
    public static LobbyPlayerManager Instance { get; private set;}
    public static List<PlayerData> JoinedPlayers = new List<PlayerData>();

    private PlayerInputManager _playerInputManager;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
        else
        {
            Instance = this;
            _playerInputManager = GetComponent<PlayerInputManager>();
            DontDestroyOnLoad(gameObject); // Optional: Persist across scene loads
            JoinedPlayers.Clear();
        }
    }

    private void OnEnable()
    {
        if (_playerInputManager != null)
            _playerInputManager.onPlayerJoined += OnPlayerJoined;
    }

    private void OnDisable()
    {
        if (_playerInputManager != null)
            _playerInputManager.onPlayerJoined -= OnPlayerJoined;
    }

    private void OnPlayerJoined(PlayerInput playerInput)
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "MainMenu")
            return; // ignore joins outside the lobby
        Debug.Log($"Player {playerInput.playerIndex} joined using {playerInput.devices[0]}");

        PlayerData newPlayer = new PlayerData
        {
            device = playerInput.devices[0],
            playerIndex = playerInput.playerIndex
        };

        JoinedPlayers.Add(newPlayer);

        // Destroy the auto-instantiated player prefab
        Destroy(playerInput.gameObject);
    }
}

[System.Serializable]
public class PlayerData
{
    public InputDevice device;
    public int playerIndex;
}