using System;
using TMPro;
using UnityEngine;

public class LapInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private LapTracker tracker;

    // might be able to turn this into an event later on
    private void Update()
    {
        text.text = $"Lap: {tracker.currentLap}";
    }
}
