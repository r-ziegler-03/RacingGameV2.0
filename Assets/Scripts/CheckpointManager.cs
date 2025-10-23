using UnityEngine;
using System.Collections.Generic;

public class CheckpointManager : MonoBehaviour
{
    [SerializeField] private List<Checkpoint> checkpoints = new List<Checkpoint>();

    public void PlayerThroughCheckpoint(LapTracker player, int checkpointIndex)
    {
        int nextCheckpoint = (player.lastCheckpoint + 1) % checkpoints.Count;

        Debug.Log($"{player.name} hit checkpoint {checkpointIndex}, expected next = {nextCheckpoint}");

        // Only accept correct sequence
        if (checkpointIndex == nextCheckpoint)
        {
            // If we looped back to the first checkpoint after finishing the last one
            bool completedLap = (checkpointIndex == 0 && player.lastCheckpoint == checkpoints.Count - 1);

            player.lastCheckpoint = checkpointIndex;

            if (completedLap)
            {
                player.CompleteLap();
            }
        }
    }

}