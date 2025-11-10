using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public enum PlayerIndex
{
    Player1,
    Player2,
    Player3,
    Player4
}

[System.Serializable]
public class PlayerData
{
    public InputDevice device;
    public int playerIndex;
    public PlayerIndex playerName;
}

public class ControllerJoining : MonoBehaviour
{
    public static ControllerJoining Instance { get; private set; }
    public static List<PlayerData> JoinedPlayers = new List<PlayerData>();

    private PlayerInputManager playerInputManager;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // prevent duplicates
            return;
        }

        Instance = this;
        playerInputManager = GetComponent<PlayerInputManager>();
        DontDestroyOnLoad(gameObject); // keep across scenes
        JoinedPlayers.Clear();
    }

    private void OnEnable()
    {
        if (playerInputManager != null)
            playerInputManager.onPlayerJoined += OnPlayerJoined;
    }

    private void OnDisable()
    {
        if (playerInputManager != null)
            playerInputManager.onPlayerJoined -= OnPlayerJoined;
    }

    private void OnPlayerJoined(PlayerInput playerInput)
    {
        if (!IsMainMenu())
            return; // ignore joins outside the lobby

        InputDevice device = playerInput.devices.Count > 0 ? playerInput.devices[0] : null;

        // Check if this device has already joined
        bool alreadyJoined = JoinedPlayers.Exists(p => p.device == device);

        if (alreadyJoined || (JoinedPlayers.Count == 2))
        {
            Debug.Log($"Device {device} is already joined.");
            Destroy(playerInput.gameObject);
            return;
        }

        // Assign next available PlayerIndex (wraps around if more than 4)
        PlayerIndex assignedEnum = (PlayerIndex)(JoinedPlayers.Count % System.Enum.GetValues(typeof(PlayerIndex)).Length);
		 

        PlayerData newPlayer = new PlayerData
        {
            device = device,
            playerIndex = playerInput.playerIndex,
            playerName = assignedEnum
        };

        JoinedPlayers.Add(newPlayer);
        Debug.Log($"Player {newPlayer.playerName} joined using {device}");

        // Destroy the auto-instantiated player prefab
        Destroy(playerInput.gameObject);
    }

    private bool IsMainMenu()
    {
        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}


