using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using static CoroutineHelper;

public class FlashlightEffect : MonoBehaviour
{
    [SerializeField] VolumeProfile volumeProfile;
    Vignette vignette;
    const float VignetteIntensity = 0.5f;

    IEnumerator fadeCoroutine;

    void Awake()
    {
        FindObjectOfType<MazeGenerator>().GameStartAction += () => enabled = true;
        FindObjectOfType<PlayerController>().GameOverAction += OnGameOver;

        volumeProfile.TryGet(out vignette);
        vignette.intensity.value = 0f;
    }

    void Start()
    {
        Fade(0f, VignetteIntensity);
    }

    void Fade(float startValue, float endValue)
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        fadeCoroutine = FadeVignette(startValue, endValue);
        StartCoroutine(fadeCoroutine);
    }

    IEnumerator FadeVignette(float startValue, float endValue)
    {
        if (vignette != null)
        {
            float currentLerpTime = 0f;
            float totalLerpTime = 1f;

            vignette.intensity.value = startValue;

            while (vignette.intensity.value != endValue)
            {
                float lerpProgress = currentLerpTime / totalLerpTime;
                vignette.intensity.value = Mathf.Lerp(startValue, endValue, lerpProgress);

                yield return EndOfFrame;
                currentLerpTime += Time.deltaTime;
            }

            vignette.intensity.value = endValue;
        }
    }

    void OnGameOver(bool playerWon)
    {
        if (playerWon)
        {
            Fade(VignetteIntensity, 0.1f);
        }
    }

#if UNITY_EDITOR
    void OnApplicationQuit()
    {
        vignette.intensity.value = 0;
    }
#endif
}