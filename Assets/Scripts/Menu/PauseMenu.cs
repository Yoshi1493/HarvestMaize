using UnityEngine;

public class PauseMenu : Menu
{
    PauseHandler pauseHandler;

    protected override void Awake()
    {
        base.Awake();

        pauseHandler = GetComponent<PauseHandler>();
        pauseHandler.GamePauseAction += OnGamePaused;
    }

    void OnGamePaused(bool state)
    {
        if (state)
            Open();

        else
        {
            Close();
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            pauseHandler.SetGamePaused(false);
            Close();
        }
    }
}