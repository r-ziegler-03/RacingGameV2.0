using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private int checkpointIndex; // Set this in the inspector or dynamically
    private CheckpointManager _manager;

    private void Start()
    {
        _manager = FindAnyObjectByType<CheckpointManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        LapTracker tracker = other.GetComponentInParent<LapTracker>();
        if (tracker == null) return;
        if (_manager == null)
            _manager = FindAnyObjectByType<CheckpointManager>();

        if (_manager != null)
        {
            _manager.PlayerThroughCheckpoint(tracker, checkpointIndex);
        }
        else
        {
            Debug.LogWarning($"[{name}] No CheckpointManager found in scene!");
        }
    }

}