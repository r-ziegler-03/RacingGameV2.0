using UnityEngine;

public class LapTracker : MonoBehaviour
{
    public int currentLap = 1;
    public int lastCheckpoint = 0;

    private CheckpointManager _manager;

    private void Start()
    {
        _manager = FindAnyObjectByType<CheckpointManager>();
        Debug.Log($"{name} found manager: {_manager != null}");
    }

    public void CompleteLap()
    {
        currentLap++;
        Debug.Log($"{name} completed lap {currentLap}");
    }
    
}


