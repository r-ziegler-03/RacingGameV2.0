using System;
using TMPro;
using UnityEngine;

public class LapInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private LapTracker _tracker;


    private void Update()
    {
        _text.text = $"Lap: {_tracker.currentLap}";
    }
}
