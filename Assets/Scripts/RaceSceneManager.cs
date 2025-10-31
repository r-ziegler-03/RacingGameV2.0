using UnityEngine;
using UnityEngine.InputSystem;

public class RaceSceneManager : MonoBehaviour
{
    [SerializeField] private GameObject carPrefab;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private InputActionAsset playerActions;

    private void Start()
    {
        if (LobbyPlayerManager.joinedPlayers.Count == 0)
        {
            Debug.LogWarning("No players found in LobbyPlayerJoin!");
            return;
        }

        for (int i = 0; i < LobbyPlayerManager.joinedPlayers.Count; i++)
        {
            var playerData = LobbyPlayerManager.joinedPlayers[i];
            var spawn = spawnPoints[i % spawnPoints.Length];

            // 1️⃣ Instantiate prefab at spawn point
            GameObject car = Instantiate(carPrefab, spawn.position + Vector3.up * 0.5f, spawn.rotation);

            // 2️⃣ Add PlayerInput and pair with device
            PlayerInput playerInput = car.GetComponent<PlayerInput>();
            if (playerInput == null)
                playerInput = car.AddComponent<PlayerInput>();

            playerInput.actions = playerActions;
            playerInput.defaultControlScheme = null;
            playerInput.SwitchCurrentControlScheme(playerData.device);

            // 3️⃣ Freeze X/Z rotations to prevent spinning
            Rigidbody rb = car.GetComponent<Rigidbody>();
            if (rb != null)
                rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

            Debug.Log($"Spawned car for Player {playerData.playerIndex} using {playerData.device.displayName}");
        }
    }
}