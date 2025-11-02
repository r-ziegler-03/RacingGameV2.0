using NWH.VehiclePhysics2.Input;
using NWH.VehiclePhysics2; // for VehicleController
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using System.Collections;

public class RaceSceneManager : MonoBehaviour
{
    [SerializeField] private GameObject carPrefab;
    [SerializeField] private Transform[] spawnPoints;

    private bool _spawned;

    private void Start()
    {
        if (_spawned) return;
        _spawned = true;

        int playerCount = LobbyPlayerManager.JoinedPlayers.Count;
        Debug.Log($"[RaceSceneManager] Starting race scene with {playerCount} players.");

        if (playerCount == 0)
        {
            Debug.LogWarning("No players found in LobbyPlayerManager!");
            return;
        }

        for (int i = 0; i < playerCount; i++)
        {
            PlayerData player = LobbyPlayerManager.JoinedPlayers[i];
            Transform spawnPoint = spawnPoints[i % spawnPoints.Length];
            StartCoroutine(SpawnCar_Manual(player, spawnPoint, i));
        }
    }

    private IEnumerator SpawnCar_Manual(PlayerData player, Transform spawnPoint, int index)
    {
        // Spawn the car
        GameObject carGO = Instantiate(carPrefab, spawnPoint.position, spawnPoint.rotation);
        carGO.name = $"Car_{index}_Player{player.playerIndex}";
        Debug.Log($"[SpawnCar_Manual] Car instantiated: {carGO.name}");

        yield return null; // Let Unity initialize everything

        // Get required components
        var vc = carGO.GetComponent<VehicleController>();
        var provider = carGO.GetComponent<InputSystemVehicleInputProvider>();
        var playerInput = carGO.GetComponent<PlayerInput>();

        if (vc == null || provider == null || playerInput == null)
        {
            Debug.LogError($"[SpawnCar_Manual] Missing required components on {carGO.name}");
            yield break;
        }

        // Create and assign input actions for this player
        var actions = new VehicleInputActions();
        actions.Enable();

        // Create an isolated InputUser and pair the player’s device
        var user = InputUser.CreateUserWithoutPairedDevices();
        InputUser.PerformPairingWithDevice(player.device, user);
        user.AssociateActionsWithUser(actions);

        // Initialize the provider with these actions
        provider.Initialize(actions);

        // Assign this provider as the dedicated input for this car
        vc.input.dedicatedProvider = provider;

        Debug.Log($"[SpawnCar_Manual] Completed setup for player {index}");
    }
}
