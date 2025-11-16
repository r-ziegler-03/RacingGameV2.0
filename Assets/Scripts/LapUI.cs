using System;
using System.Text;
using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class LapUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI standingsText;
    [SerializeField] private LapProgress manager;
    [SerializeField] private CanvasGroup raceStandingsUI;

    private void Awake()
    {
        raceStandingsUI.gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        if (manager != null)
            manager.OnRaceProgressUpdated += UpdateStandings;
    }

    private void OnDisable()
    {
        if (manager != null)
            manager.OnRaceProgressUpdated -= UpdateStandings;
    }

    private void UpdateStandings(List<PlayerLapTracker> orderedPlayers)
    {
        StringBuilder stringBuilder = new StringBuilder();
        for (int i = 0; i < orderedPlayers.Count; i++)
        {
            PlayerLapTracker player = orderedPlayers[i];
            stringBuilder.AppendLine($"{i + 1}. {player.playerName} — Lap {player.currentLap}");
            stringBuilder.AppendLine($"Position: {player.currentPosition}");
        }

        standingsText.text = stringBuilder.ToString();
    }
}