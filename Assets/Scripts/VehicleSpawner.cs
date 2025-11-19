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
        GameObject carGO = SpawnCarObject(spawnPoint, index, player);
        yield return null; // wait for initialization

        if (!TryGetCarComponents(carGO, out VehicleController vc,
                out InputSystemVehicleInputProvider provider,
                out PlayerInput pInput,
                out PlayerLapTracker lap))
            yield break;

        InitializeLapTracker(lap, player, index);
        SetupInputForCar(player, vc, provider);
        SetupCameraForCar(carGO, index);

        Debug.Log($"[SpawnCar_Manual] Completed setup for Player {index}");
    }
    private GameObject SpawnCarObject(Transform spawnPoint, int index, PlayerData player)
    {
        GameObject carGO = Instantiate(carPrefab, spawnPoint.position, spawnPoint.rotation);
        carGO.name = $"Car_{index}_Player{player.playerIndex}";
        Debug.Log($"[SpawnCar_Manual] Car instantiated: {carGO.name}");
        return carGO;
    }
    
    private bool TryGetCarComponents(
        GameObject carGO,
        out VehicleController vehicleController,
        out InputSystemVehicleInputProvider inputProvider,
        out PlayerInput playerInput,
        out PlayerLapTracker lapTracker)
    {
        vehicleController = carGO.GetComponent<VehicleController>();
        inputProvider = carGO.GetComponent<InputSystemVehicleInputProvider>();
        playerInput = carGO.GetComponent<PlayerInput>();
        lapTracker = carGO.GetComponent<PlayerLapTracker>();

        if (vehicleController == null || inputProvider == null || playerInput == null)
        {
            Debug.LogError($"[SpawnCar_Manual] Missing required components on {carGO.name}");
            return false;
        }

        return true;
    }
    
    private void InitializeLapTracker(PlayerLapTracker lapTracker, PlayerData player, int index)
    {
        if (lapTracker == null)
            return;

        lapTracker.playerName = player.playerName;
        Debug.Log($"[SpawnCar_Manual] LapTracker assigned to Player {index}");
    }
    
    private void SetupInputForCar(
        PlayerData player,
        VehicleController vc,
        InputSystemVehicleInputProvider provider)
    {
        VehicleInputActions actions = new VehicleInputActions();
        actions.Enable();

        InputUser user = InputUser.CreateUserWithoutPairedDevices();
        InputUser.PerformPairingWithDevice(player.device, user);
        user.AssociateActionsWithUser(actions);

        provider.Initialize(actions);
        vc.input.dedicatedProvider = provider;
    }
    private void SetupCameraForCar(GameObject carGO, int index)
    {
        MultiplayerCameraChanger changer = carGO.GetComponentInChildren<MultiplayerCameraChanger>();
        if (changer == null)
        {
            Debug.LogWarning($"[SpawnCar_Manual] No MultiplayerCameraChanger found for {carGO.name}");
            return;
        }

        changer.playerIndex = index;
        changer.currentCameraIndex = 0;
        changer.ForceEnable(playerCount);

        Debug.Log($"[SpawnCar_Manual] Camera assigned for Player {index}");
    }
}

