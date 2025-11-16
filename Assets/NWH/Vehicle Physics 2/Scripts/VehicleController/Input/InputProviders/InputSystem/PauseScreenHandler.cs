using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseScreenHandler : MonoBehaviour
{
    public static PauseScreenHandler Instance { get; private set; }
    //private UI_InputActions inputActions;
    [SerializeField] private CanvasGroup pauseScreen;
    [SerializeField] private CanvasGroup raceStandings;
    private bool isPaused;
    

    private void Awake()
    {
        Instance = this;
        //inputActions = new UI_InputActions();
    }
    
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        //inputActions.Enable();
        //inputActions.UIMapping.Escape.performed += PauseGame;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        //inputActions.UIMapping.Escape.performed -= PauseGame;
        //inputActions.Dispose();
    }
    
    private void PauseGame(InputAction.CallbackContext context)
    {
        TogglePauseScreen();
    }

    public void TogglePauseScreen()
    {
        isPaused = !isPaused;
        pauseScreen.alpha = isPaused ? 1 : 0;
        pauseScreen.blocksRaycasts = isPaused;
        pauseScreen.interactable = isPaused;
        Time.timeScale = isPaused ? 0 : 1;
        raceStandings.alpha = !isPaused ? 1 : 0;
        raceStandings.blocksRaycasts = !isPaused;
        raceStandings.interactable = !isPaused;
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Time.timeScale = 1f;
        isPaused = false;
        pauseScreen.alpha = 0;
        pauseScreen.blocksRaycasts = false;
        pauseScreen.interactable = false;
    }
    
    
}
