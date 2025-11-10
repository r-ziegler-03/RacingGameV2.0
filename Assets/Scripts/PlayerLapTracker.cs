using System;
using UnityEngine;

public class PlayerLapTracker : MonoBehaviour
{
    public PlayerIndex playerName;
    public int currentLap = 1;
    public int lastCheckpoint = -1;
    public float totalProgress;

    public event Action<PlayerLapTracker> OnLapCompleted;
    public event Action<PlayerLapTracker> OnProgressUpdated;

    private LapProgress manager;

    private void Start()
    {
        manager = FindAnyObjectByType<LapProgress>();
        if (manager != null)
            manager.RegisterPlayer(this);
    }

    public void UpdateProgress(int checkpointIndex, int totalCheckpoints)
    {
        totalProgress = currentLap + (checkpointIndex / (float)totalCheckpoints);
        OnProgressUpdated?.Invoke(this);
    }

    public void CompleteLap()
    {
        currentLap++;
        OnLapCompleted?.Invoke(this);
    }
}


