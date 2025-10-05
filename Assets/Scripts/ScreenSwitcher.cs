using System;
using System.Collections.Generic;
using UnityEngine;

public enum ScreenTypes
{
    Menu,
    Settings,
    Selection,
    Pause,
    GameOver,
}

[Serializable]
public class ScreenEntry
{
    public ScreenTypes type;
    public CanvasGroup screen;
}

public class ScreenSwitcher : MonoBehaviour
{
    [SerializeField] private List<ScreenEntry> screenEntries = new();
    [SerializeField] private bool useDefaultScreen = true;
    [SerializeField] private ScreenTypes defaultScreen = ScreenTypes.Menu;

    private Dictionary<ScreenTypes, CanvasGroup> screensDict = new();
    private CanvasGroup currentScreen;

    private void Awake()
    {
        foreach (var entry in screenEntries)
        {
            if (!screensDict.ContainsKey(entry.type))
            {
                screensDict.Add(entry.type, entry.screen);
                SetScreenEnabled(entry.screen, false);
            }
            else
            {
                Debug.LogWarning($"Duplicate screen type: {entry.type}");
            }
        }

        if (useDefaultScreen && screensDict.TryGetValue(defaultScreen, out var startScreen))
        {
            SwitchScreen(defaultScreen);
        }
    }

    public void SwitchScreen(ScreenTypes newScreenType)
    {
        if (!screensDict.TryGetValue(newScreenType, out var newScreen))
        {
            Debug.LogWarning($"Screen type {newScreenType} not found.");
            return;
        }
        if (currentScreen != null)
            SetScreenEnabled(currentScreen, false);

        currentScreen = newScreen;
        SetScreenEnabled(currentScreen, true);
    }

    private void SetScreenEnabled(CanvasGroup screen, bool enabled)
    {
        screen.alpha = enabled ? 1 : 0;
        screen.blocksRaycasts = enabled;
        screen.interactable = enabled;
    }
}