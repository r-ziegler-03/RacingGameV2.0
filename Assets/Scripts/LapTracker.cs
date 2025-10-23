using UnityEngine;

public class LapTracker : MonoBehaviour
{
    public int currentLap = 1;
    public int lastCheckpoint = 0;

    private CheckpointManager manager;

    private void Start()
    {
        manager = FindAnyObjectByType<CheckpointManager>();
        Debug.Log($"{name} found manager: {manager != null}");
    }

    public void CompleteLap()
    {
        currentLap++;
        Debug.Log($"{name} completed lap {currentLap}");
    }
    
}