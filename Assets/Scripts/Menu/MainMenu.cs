using UnityEngine;

public class MainMenu : Menu
{
    public void OnSelectQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}