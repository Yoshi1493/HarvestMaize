public class SettingsMenu : Menu
{
    PauseHandler pauseHandler;

    protected override void Awake()
    {
        base.Awake();

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
}