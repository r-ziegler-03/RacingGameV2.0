using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine.InputSystem.Users;
using UnityEngine.SceneManagement;


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
    [HideInInspector] public int lapCount = 0;
    public event Action OnPlayerJoinedNotification;
    
    private PlayerInputManager playerInputManager;
    private int maxPlayers = 2;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // prevent duplicates
            Debug.Log("Destroyed");
            return;
        }

        Instance = this;
        playerInputManager = GetComponent<PlayerInputManager>();
        DontDestroyOnLoad(gameObject); // keep across scenes
        JoinedPlayers.Clear();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        if (playerInputManager != null)
            playerInputManager.onPlayerJoined += OnPlayerJoined;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        if (playerInputManager != null)
            playerInputManager.onPlayerJoined -= OnPlayerJoined;
    }

    private void OnPlayerJoined(PlayerInput playerInput)
    {
        if (!CanAcceptJoin(playerInput))
            return;

        InputDevice device = GetDeviceFromPlayer(playerInput);

        if (ShouldRejectDevice(device))
        {
            RejectPlayerJoin(playerInput, device);
            return;
        }

        PlayerData newPlayer = CreatePlayerData(playerInput, device);
        RegisterPlayer(newPlayer);

        CleanupAutoPlayerObject(playerInput);
        OnPlayerJoinedNotification?.Invoke();


    }
    
    private bool CanAcceptJoin(PlayerInput playerInput)
    {
        return IsMainMenu() && JoinedPlayers.Count < maxPlayers;
    }

    private InputDevice GetDeviceFromPlayer(PlayerInput playerInput)
    {
        return playerInput.devices.Count > 0 ? playerInput.devices[0] : null;
    }

    private bool ShouldRejectDevice(InputDevice device)
    {
        bool alreadyJoined = JoinedPlayers.Exists(p => p.device == device);
        bool lobbyFull = JoinedPlayers.Count == 2; // your custom rule

        return alreadyJoined || lobbyFull;
    }

    private void RejectPlayerJoin(PlayerInput playerInput, InputDevice device)
    {
        Debug.Log($"Device {device} is already joined.");
        Destroy(playerInput.gameObject);
    }

    private PlayerData CreatePlayerData(PlayerInput playerInput, InputDevice device)
    {
        PlayerIndex assignedEnum =
            (PlayerIndex)(JoinedPlayers.Count % System.Enum.GetValues(typeof(PlayerIndex)).Length);

        return new PlayerData
        {
            device = device,
            playerIndex = playerInput.playerIndex,
            playerName = assignedEnum
        };
    }

    private void RegisterPlayer(PlayerData data)
    {
        JoinedPlayers.Add(data);
        Debug.Log($"Player {data.playerName} joined using {data.device}");
    }

    private void CleanupAutoPlayerObject(PlayerInput playerInput)
    {
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

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenu")
        {
            if (playerInputManager != null)
                playerInputManager.EnableJoining();
        }
    }

    public static void ResetInputUsers()
    {
        var usersToRemove = new List<InputUser>();

        foreach (var player in ControllerJoining.JoinedPlayers)
        {
            foreach (var user in InputUser.all)
            {
                if (user.pairedDevices.Contains(player.device))
                {
                    usersToRemove.Add(user);
                    break;
                }
            }
        }

        foreach (var user in usersToRemove)
        {
            user.UnpairDevicesAndRemoveUser();
            Debug.Log($"[ControllerJoining] Unpaired user {user.id}");
        }

        ControllerJoining.JoinedPlayers.Clear();
        Debug.Log("[ControllerJoining] Cleared all paired users and player data.");
    }
    
    

}


