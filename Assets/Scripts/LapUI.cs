using System;
using System.Text;
using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class LapUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI standingsText;
    [SerializeField] private LapProgress lapProgress;
    [SerializeField] private CanvasGroup raceStandingsUI;

    private void Awake()
    {
        raceStandingsUI.gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        if (lapProgress != null)
            lapProgress.OnRaceProgressUpdated += UpdateStandings;
    }

    private void OnDisable()
    {
        if (lapProgress != null)
            lapProgress.OnRaceProgressUpdated -= UpdateStandings;
    }

    private void UpdateStandings(List<PlayerLapTracker> orderedPlayers)
    {
        StringBuilder stringBuilder = new StringBuilder();
        for (int i = 0; i < orderedPlayers.Count; i++)
        {
            PlayerLapTracker player = orderedPlayers[i];
            stringBuilder.AppendLine($"{player.playerName} — Laps To Go {lapProgress.numRaceLaps - player.currentLap}");
            stringBuilder.AppendLine($"Position: {player.currentPosition}");
            stringBuilder.AppendLine("--------------------");
        }

        standingsText.text = stringBuilder.ToString();
    }
}