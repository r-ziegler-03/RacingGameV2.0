using System;
using UnityEngine;

public class PlayerLapTracker : MonoBehaviour
{
    public PlayerIndex playerName;
    public int currentLap = 1;
    public int lastCheckpoint = -1;
    public float totalProgress;
    public Vector3 lastCheckpointPos;
    public float distanceToNextCheckpoint;
    public int currentPosition;

    public event Action<PlayerLapTracker> OnLapCompleted;
    public event Action<PlayerLapTracker> OnProgressUpdated;

    private LapProgress manager;

    private void Start()
    {
        manager = FindAnyObjectByType<LapProgress>();
        if (manager != null)
            manager.RegisterPlayer(this);
    }

    private void Update()
    {
        if (manager != null && lastCheckpoint >= 0)
            RecalculateLiveProgress();
    }

    // Called by LapProgress when a checkpoint is hit
    public void OnCheckpointHit(int checkpointIndex, int totalCheckpoints)
    {
        lastCheckpoint = checkpointIndex;
        RecalculateLiveProgress(); // sync right away on hit
    }

    private void RecalculateLiveProgress()
    {
        var checkpoints = manager.Checkpoints;
        int total = checkpoints.Count;
        int nextIndex = (lastCheckpoint + 1) % total;

        Vector3 from = checkpoints[lastCheckpoint].transform.position;
        Vector3 to = checkpoints[nextIndex].transform.position;
        Vector3 pos = transform.position;

        float segmentLength = Vector3.Distance(from, to);
        float distToNext = Vector3.Distance(pos, to);
        float segmentProgress = 1f - Mathf.Clamp01(distToNext / segmentLength);

        totalProgress = currentLap + ((lastCheckpoint + segmentProgress) / total);
        distanceToNextCheckpoint = distToNext;
        lastCheckpointPos = from;

        OnProgressUpdated?.Invoke(this);
    }

    public void CompleteLap()
    {
        currentLap++;
        OnLapCompleted?.Invoke(this);
    }
}


