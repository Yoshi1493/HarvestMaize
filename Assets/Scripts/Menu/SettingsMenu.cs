using System;
using UnityEngine;
using TMPro;

public class SettingsMenu : Menu
{
    int maxMapDimensions;
    [SerializeField] GameObject[] mapDimensionChangeButtons;
    [SerializeField] TextMeshProUGUI mapDimensionText;

    [SerializeField] IntObject selectedDifficulty;

    PauseHandler pauseHandler;

    protected override void Awake()
    {
        base.Awake();

        maxMapDimensions = Enum.GetValues(typeof(MazeSize)).Length;
        UpdateUIElements();

        pauseHandler = FindObjectOfType<PauseHandler>();
        if (pauseHandler != null)
        {
            pauseHandler.GamePauseAction += OnGamePaused;
        }
    }

    void OnGamePaused(bool state)
    {
        if (!state)
        {
            Close();
        }
    }

    void UpdateUIElements()
    {
        mapDimensionText.text = ((MazeSize)selectedDifficulty.value).ToString();

        mapDimensionChangeButtons[0].SetActive(selectedDifficulty.value > 0);
        mapDimensionChangeButtons[1].SetActive(selectedDifficulty.value < maxMapDimensions - 1);
    }

    public void OnChangeMapDimensions(int amount)
    {
        selectedDifficulty.value = Mathf.Clamp(selectedDifficulty.value + amount, 0, maxMapDimensions - 1);
        UpdateUIElements();
    }
}