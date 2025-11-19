using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private int checkpointIndex;
    private LapProgress lapProgress;

    private void Start()
    {
        lapProgress = FindAnyObjectByType<LapProgress>();
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerLapTracker tracker = other.GetComponentInParent<PlayerLapTracker>();
        if (tracker == null) return;

        lapProgress ??= FindAnyObjectByType<LapProgress>();
        lapProgress?.PlayerThroughCheckpoint(tracker, checkpointIndex);
    }

    public void SetIndex(int index) => checkpointIndex = index;
}