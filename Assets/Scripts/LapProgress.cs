using System.Collections.Generic;
using UnityEngine;

using System;
using System.Collections.Generic;
using UnityEngine;

public class LapProgress : MonoBehaviour
{
    [SerializeField] private List<Checkpoint> checkpoints = new();
    [SerializeField] private List<PlayerLapTracker> players = new();

    public event Action<List<PlayerLapTracker>> OnRaceProgressUpdated;

    public IReadOnlyList<PlayerLapTracker> Players => players;

    private void Awake()
    {
        if (checkpoints.Count == 0)
            checkpoints.AddRange(FindObjectsOfType<Checkpoint>());

        for (int i = 0; i < checkpoints.Count; i++)
            checkpoints[i].SetIndex(i);
    }

    public void RegisterPlayer(PlayerLapTracker tracker)
    {
        if (!players.Contains(tracker))
        {
            players.Add(tracker);
            tracker.OnProgressUpdated += HandlePlayerProgress;
            tracker.OnLapCompleted += HandlePlayerLapComplete;
        }
    }

    private void HandlePlayerProgress(PlayerLapTracker player)
    {
        OnRaceProgressUpdated?.Invoke(GetSortedRaceOrder());
    }

    private void HandlePlayerLapComplete(PlayerLapTracker player)
    {
        Debug.Log($"{player.name} completed a lap!");
        OnRaceProgressUpdated?.Invoke(GetSortedRaceOrder());
    }

    public void PlayerThroughCheckpoint(PlayerLapTracker player, int checkpointIndex)
    {
        if (checkpoints.Count == 0) return;

        int nextCheckpoint = (player.lastCheckpoint + 1) % checkpoints.Count;
        if (checkpointIndex != nextCheckpoint) return;

        bool completedLap = (checkpointIndex == 0 && player.lastCheckpoint == checkpoints.Count - 1);
        player.lastCheckpoint = checkpointIndex;
        player.UpdateProgress(checkpointIndex, checkpoints.Count);

        if (completedLap)
            player.CompleteLap();
    }

    public List<PlayerLapTracker> GetSortedRaceOrder()
    {
        List<PlayerLapTracker> sorted = new(players);
        sorted.Sort((a, b) => b.totalProgress.CompareTo(a.totalProgress));
        return sorted;
    }
}
