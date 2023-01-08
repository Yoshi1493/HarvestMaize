using System.Collections;
using UnityEngine;
using TMPro;
using static CoroutineHelper;

public class TooltipDisplay : MonoBehaviour
{
    TextMeshProUGUI tooltipText;

    IEnumerator fadeCoroutine;
    [SerializeField] AnimationCurve fadeInterpolation;

    void Awake()
    {
        tooltipText = GetComponent<TextMeshProUGUI>();
        FindObjectOfType<MazeGenerator>().GameStartAction += () => enabled = true;
        FindObjectOfType<PlayerHarvester>().ThresholdReachedAction += OnExitCreated;

        Fade(0f, 1f, 0.5f);
    }

    void Start()
    {
        Fade(1f, 0f, 0.5f, 0.5f);
    }

    void Fade(float startValue, float endValue, float fadeDuration, float delay = 0f)
    {
        if (fadeDuration > 0f)
        {
            if (fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine);
            }

            fadeCoroutine = FadeText(startValue, endValue, fadeDuration, delay);
            StartCoroutine(fadeCoroutine);
        }
        else
        {
            tooltipText.alpha = endValue;
        }
    }

    IEnumerator FadeText(float startValue, float endValue, float fadeDuration, float delay)
    {
        tooltipText.alpha = startValue;

        if (delay > 0f) yield return WaitForSeconds(delay);

        float currentLerpTime = 0f;

        while (tooltipText.alpha != endValue)
        {
            float lerpProgress = fadeInterpolation.Evaluate(currentLerpTime / fadeDuration);
            tooltipText.alpha = Mathf.Lerp(startValue, endValue, lerpProgress);

            yield return EndOfFrame;
            currentLerpTime += Time.deltaTime;
        }

        tooltipText.alpha = endValue;
    }

    void OnExitCreated()
    {
        tooltipText.text = "An exit has opened.";
        Fade(1f, 0f, 1f, 3f);
    }
}