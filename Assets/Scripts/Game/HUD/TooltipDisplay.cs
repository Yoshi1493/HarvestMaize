using System.Collections;
using UnityEngine;
using TMPro;
using static CoroutineHelper;

public class TooltipDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI instructionsText;
    [SerializeField] TextMeshProUGUI tooltipText;

    IEnumerator fadeCoroutine;
    [SerializeField] AnimationCurve fadeInterpolation;

    void Awake()
    {
        FindObjectOfType<MazeGenerator>().GameStartAction += () => enabled = true;
        FindObjectOfType<PlayerHarvester>().AllHarvestedAction += OnExitCreated;

        instructionsText.text = $"{MazeGenerator.ContaminatedWallCount} crops have been contaminated by zombies.\n\nHarvest them in order to exit the maze,\nbefore the zombies harvest you.";

        Fade(instructionsText, 0f, 1f, 0.5f);
        Fade(tooltipText, 0f, 1f, 0.5f);
    }

    void Start()
    {
        Fade(instructionsText, 1f, 0f, 0.5f, 0.5f);
        Fade(tooltipText, 1f, 0f, 0.5f, 0.5f);
    }

    void Fade(TextMeshProUGUI text, float startValue, float endValue, float fadeDuration, float delay = 0f)
    {
        if (fadeDuration > 0f)
        {
            fadeCoroutine = FadeText(text, startValue, endValue, fadeDuration, delay);
            StartCoroutine(fadeCoroutine);
        }
        else
        {
            text.alpha = endValue;
        }
    }

    IEnumerator FadeText(TextMeshProUGUI text, float startValue, float endValue, float fadeDuration, float delay)
    {
        text.alpha = startValue;

        if (delay > 0f) yield return WaitForSeconds(delay);

        float currentLerpTime = 0f;

        while (text.alpha != endValue)
        {
            float lerpProgress = fadeInterpolation.Evaluate(currentLerpTime / fadeDuration);
            text.alpha = Mathf.Lerp(startValue, endValue, lerpProgress);

            yield return EndOfFrame;
            currentLerpTime += Time.deltaTime;
        }

        text.alpha = endValue;
    }

    void OnExitCreated()
    {
        tooltipText.text = "An exit has been opened.";
        Fade(tooltipText, 1f, 0f, 1f, 3f);
    }
}