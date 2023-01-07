using System.Collections;
using UnityEngine;

public class MainMenu : Menu
{
    public void OnSelectQuit()
    {
        if (sceneTransitionCoroutine != null)
        {
            StopCoroutine(sceneTransitionCoroutine);
        }

        sceneTransitionCoroutine = Quit();
        StartCoroutine(sceneTransitionCoroutine);
    }

    IEnumerator Quit()
    {
        yield return backgroundController.FadeBackground(0f, 1f, 1f);

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}