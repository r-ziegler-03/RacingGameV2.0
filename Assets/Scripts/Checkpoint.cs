using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private int checkpointIndex;
    private LapProgress manager;

    private void Start()
    {
        manager = FindAnyObjectByType<LapProgress>();
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerLapTracker tracker = other.GetComponentInParent<PlayerLapTracker>();
        if (tracker == null) return;

        manager ??= FindAnyObjectByType<LapProgress>();
        manager?.PlayerThroughCheckpoint(tracker, checkpointIndex);
    }

    public void SetIndex(int index) => checkpointIndex = index;
}