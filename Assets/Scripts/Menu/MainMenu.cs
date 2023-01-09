using System.Collections;
using UnityEngine;
using TMPro;
using static CoroutineHelper;

public class MainMenu : Menu
{
    [SerializeField] Material textMaterial;
    [SerializeField] AnimationCurve dilateInterpolation;

    IEnumerator Start()
    {
        float currentLerpTime = 0f;
        float totalLerpTime = 2f;

        while (currentLerpTime < totalLerpTime)
        {
            float lerpProgress = dilateInterpolation.Evaluate(currentLerpTime / totalLerpTime);
            textMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, Mathf.Lerp(-1f, 0f, lerpProgress));

            yield return EndOfFrame;
            currentLerpTime += Time.deltaTime;
        }

        textMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, 0f);
    }

    void OnApplicationQuit()
    {
        textMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, 0f);
    }

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