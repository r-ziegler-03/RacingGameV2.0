using System.Collections.Generic;
using UnityEngine;

using System;
using System.Collections.Generic;
using UnityEngine;

public class LapProgress : MonoBehaviour
{
    [SerializeField] private GameResultsHandler gameResultsHandler;
    [SerializeField] private List<Checkpoint> checkpoints = new();
    [SerializeField] private List<PlayerLapTracker> players = new();
    [field: SerializeField] 
    public int NumRaceLaps { get; private set; } = 2;

    public List<PlayerLapTracker> playersResults = new();
    public event Action<List<PlayerLapTracker>> OnRaceProgressUpdated;
    public event Action<PlayerLapTracker> OnRaceCompleted;

    public IReadOnlyList<PlayerLapTracker> Players => players;
    public IReadOnlyList<Checkpoint> Checkpoints => checkpoints;

    private void Awake()
    {
        if (checkpoints.Count == 0)
            checkpoints.AddRange(FindObjectsOfType<Checkpoint>(true));

        for (int i = 0; i < checkpoints.Count; i++)
            checkpoints[i].SetIndex(i);
        OnRaceCompleted += EndOfRace;
    }

    private void EndOfRace(PlayerLapTracker obj)
    {
        gameResultsHandler.handle();
        //new panel showing results
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
        UpdatePlayerPositions();
    }
    

    private void HandlePlayerLapComplete(PlayerLapTracker player)
    {
        Debug.Log($"{player.name} completed a lap!");
        if (players.TrueForAll(p => p.playerFinished))
        {
            OnRaceCompleted?.Invoke(null);
        }
        UpdatePlayerPositions();
    }

    private void UpdatePlayerPositions()
    {
        List<PlayerLapTracker> sorted = GetSortedRaceOrder();

        for (int i = 0; i < sorted.Count; i++)
            sorted[i].currentPosition = i + 1; // 1-based rank

        OnRaceProgressUpdated?.Invoke(sorted);
    }

    public void PlayerThroughCheckpoint(PlayerLapTracker player, int checkpointIndex)
    {
        if (checkpoints.Count == 0) return;

        int nextCheckpoint = (player.lastCheckpoint + 1) % checkpoints.Count;
        if (checkpointIndex != nextCheckpoint) return;

        bool completedLap = (checkpointIndex == 0 && player.lastCheckpoint == checkpoints.Count - 1);

        player.OnCheckpointHit(checkpointIndex, checkpoints.Count);

        if (completedLap)
            player.CompleteLap();
    }

    public List<PlayerLapTracker> GetSortedRaceOrder()
    {
        List<PlayerLapTracker> sorted = new(players);
        sorted.Sort((a, b) => b.totalProgress.CompareTo(a.totalProgress)); // descending
        return sorted;
    }
}
