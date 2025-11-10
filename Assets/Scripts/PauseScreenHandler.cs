using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseScreenHandler : MonoBehaviour
{
    private UI_InputActions inputActions;
    [SerializeField] private CanvasGroup pauseScreen;
    private bool isPaused;

    private void Awake()
    {
        inputActions = new UI_InputActions();
    }
    
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        inputActions.UIMapping.Enable();
        inputActions.UIMapping.Escape.performed += PauseGame;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        inputActions.UIMapping.Escape.performed -= PauseGame;
        inputActions.UIMapping.Disable();
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
