using System;
using UnityEngine;

public class PauseHandler : MonoBehaviour
{
    bool isPaused;
    public event Action<bool> GamePauseAction;

    void Awake()
    {
        GamePauseAction += OnGamePaused;
        FindObjectOfType<MazeGenerator>().GameStartAction += () => enabled = true;
        FindObjectOfType<PlayerController>().GameOverAction += OnGameOver;
    }

    void Update()
    {
        if (Input.GetButtonDown("Pause"))
            SetGamePaused(!isPaused);
    }

    public void SetGamePaused(bool pauseState)
    {
        GamePauseAction?.Invoke(pauseState);
    }

    void OnGamePaused(bool state)
    {
        isPaused = state;
    }

    void OnGameOver(bool _)
    {
        enabled = false;
    }
}