using System;
using UnityEngine;

public class PlayerLapTracker : MonoBehaviour
{
    public PlayerIndex playerName;
    public int currentLap = 1;
    public int lastCheckpoint = -1;
    public float totalProgress;
    private Vector3 lastCheckpointPos;
    private float distanceToNextCheckpoint;
    public int currentPosition;
    public bool playerFinished = false;

    public event Action<PlayerLapTracker> OnLapCompleted;
    public event Action<PlayerLapTracker> OnProgressUpdated;

    private LapProgress lapProgress;
    
    
    private void Start()
    {
        lapProgress = FindAnyObjectByType<LapProgress>();
        if (lapProgress != null)
            lapProgress.RegisterPlayer(this);
    }

    private void Update()
    {
        if (lapProgress != null && lastCheckpoint >= 0)
            RecalculateLiveProgress();
    }

    // Called by LapProgress when a checkpoint is hit
    public void OnCheckpointHit(int checkpointIndex, int totalCheckpoints)
    {
        lastCheckpoint = checkpointIndex;
        RecalculateLiveProgress(); // sync right away on hit
    }
    
    public void CompleteLap()
    {
        currentLap++;
        if (currentLap == lapProgress.numRaceLaps)
        {
            playerFinished = true;
            lapProgress.playersResults.Add(this);
            GhostCar();
        }
        OnLapCompleted?.Invoke(this);
    }

    private void RecalculateLiveProgress()
    {
        var checkpoints = lapProgress.Checkpoints;
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
    
    private void GhostCar()
    {
        foreach (var col in GetComponentsInChildren<Collider>())
        {
            if (!(col is WheelCollider))
                col.enabled = false;   // turns off car-to-car collisions
        }
    }

}


