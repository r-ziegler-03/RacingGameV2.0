using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class LobbyPlayerManager : MonoBehaviour
{
    public static List<PlayerData> joinedPlayers = new List<PlayerData>();

    private PlayerInputManager playerInputManager;

    private void Awake()
    {
        Debug.Log("Awake");
        playerInputManager = GetComponent<PlayerInputManager>();
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
        Debug.Log($"Player {playerInput.playerIndex} joined using {playerInput.devices[0]}");

        PlayerData newPlayer = new PlayerData
        {
            device = playerInput.devices[0],
            playerIndex = playerInput.playerIndex
        };

        joinedPlayers.Add(newPlayer);

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