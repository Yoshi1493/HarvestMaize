using System.Collections;
using UnityEngine;
using TMPro;
using static CoroutineHelper;

public class GameOverMenu : Menu
{
    CanvasGroup canvasGroup;
    [SerializeField] TextMeshProUGUI gameoverText;

    protected override void Awake()
    {
        base.Awake();

        canvasGroup = GetComponent<CanvasGroup>();
        FindObjectOfType<PlayerController>().GameOverAction += OnGameOver;
    }

    void OnGameOver(bool playerWon)
    {
        Open();
        StartCoroutine(FadeIn());
        SetText(playerWon);
    }

    void SetText(bool playerWon)
    {
        if (playerWon)
        {
            gameoverText.text = "Escape Success";
        }
        else
        {
            gameoverText.text = "Game Over";
        }
    }

    IEnumerator FadeIn()
    {
        float currentLerpTime = 0f;
        float totalLerpTime = 0.5f;

        while (currentLerpTime < totalLerpTime)
        {
            float lerpProgress = currentLerpTime / totalLerpTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, lerpProgress);

            yield return EndOfFrame;
            currentLerpTime += Time.deltaTime;
        }

        canvasGroup.alpha = 1f;
    }

    public void OnSelectRetry()
    {
        LoadSceneAfterDelay(1);
    }

    public void OnSelectBack()
    {
        LoadSceneAfterDelay(0);
    }
}