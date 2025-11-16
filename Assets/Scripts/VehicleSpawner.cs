using NWH.VehiclePhysics2.Input;
using NWH.VehiclePhysics2;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using System.Collections;
using NWH.Common.Cameras;

public class VehicleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject carPrefab;
    [SerializeField] private Transform[] spawnPoints;

    private bool _spawned;
    private int playerCount;

    private void Start()
    {
        AudioListener.pause = false;
        if (_spawned) return;
        _spawned = true;

        playerCount = ControllerJoining.JoinedPlayers.Count;
        Debug.Log($"[RaceSceneManager] Starting race scene with {playerCount} players.");

        if (playerCount == 0)
        {
            Debug.LogWarning("No players found in LobbyPlayerManager!");
            return;
        }

        for (int i = 0; i < playerCount; i++)
        {
            PlayerData player = ControllerJoining.JoinedPlayers[i];
            Transform spawnPoint = spawnPoints[i % spawnPoints.Length];
            StartCoroutine(SpawnCar_Manual(player, spawnPoint, i));
        }
    }

    private IEnumerator SpawnCar_Manual(PlayerData player, Transform spawnPoint, int index)
    {
        // Spawn the car prefab
        GameObject carGO = Instantiate(carPrefab, spawnPoint.position, spawnPoint.rotation);
        carGO.name = $"Car_{index}_Player{player.playerIndex}";
        Debug.Log($"[SpawnCar_Manual] Car instantiated: {carGO.name}");

        yield return null; // Let Unity initialize everything for one frame

        // Grab necessary components
        VehicleController vehicleController = carGO.GetComponent<VehicleController>();
        InputSystemVehicleInputProvider inputProvider = carGO.GetComponent<InputSystemVehicleInputProvider>();
        PlayerInput playerInput = carGO.GetComponent<PlayerInput>();
        PlayerLapTracker lapTracker = carGO.GetComponent<PlayerLapTracker>();
        
        if (lapTracker != null)
        {
            lapTracker.playerName = player.playerName;
            Debug.Log($"[SpawnCar_Manual] LapTracker assigned to Player {index}");
        }

        if (vehicleController == null || inputProvider == null || playerInput == null)
        {
            Debug.LogError($"[SpawnCar_Manual] Missing required components on {carGO.name}");
            yield break;
        }

        // Set up unique InputUser and actions for this player
        VehicleInputActions actions = new VehicleInputActions();
        actions.Enable();

        InputUser user = InputUser.CreateUserWithoutPairedDevices();
        InputUser.PerformPairingWithDevice(player.device, user);
        user.AssociateActionsWithUser(actions);

        inputProvider.Initialize(actions);
        vehicleController.input.dedicatedProvider = inputProvider;

        // --- SPLIT-SCREEN CAMERA SETUP ---
        // Find the MultiplayerCameraChanger attached to the car or its children
        MultiplayerCameraChanger cameraChanger = carGO.GetComponentInChildren<MultiplayerCameraChanger>();

        if (cameraChanger != null)
        {
            cameraChanger.playerIndex = index; // 0-based player number
            cameraChanger.currentCameraIndex = 0; // default camera
            cameraChanger.ForceEnable(playerCount); // activates correct viewport rect
            Debug.Log($"[SpawnCar_Manual] Camera assigned for Player {index}");
        }
        else
        {
            Debug.LogWarning($"[SpawnCar_Manual] No MultiplayerCameraChanger found for {carGO.name}");
        }

        Debug.Log($"[SpawnCar_Manual] Completed setup for Player {index}");
    }
}

