using System.Collections;
using UnityEngine;
using Cinemachine;
using static CoroutineHelper;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class ScreenShakeEffect : MonoBehaviour
{
    CinemachineBasicMultiChannelPerlin perlinChannel;

    const float ScreenShakeDuration = 0.5f;
    IEnumerator screenShakeCoroutine;

    [SerializeField] AnimationCurve intensityFalloffCurve;

    void Awake()
    {
        perlinChannel = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent< CinemachineBasicMultiChannelPerlin>();
        FindObjectOfType<PlayerHarvester>().HarvestAction += OnPlayerHarvest;
    }

    void OnPlayerHarvest()
    {
        if (screenShakeCoroutine != null)
        {
            StopCoroutine(screenShakeCoroutine);
        }

        screenShakeCoroutine = ShakeCamera();
        StartCoroutine(screenShakeCoroutine);
    }

    IEnumerator ShakeCamera()
    {
        float currentLerpTime = 0f;

        while (currentLerpTime < ScreenShakeDuration)
        {
            float lerpProgress = intensityFalloffCurve.Evaluate(currentLerpTime / ScreenShakeDuration);
            perlinChannel.m_AmplitudeGain = Mathf.Lerp(1f, 0f, lerpProgress);

            yield return EndOfFrame;
            currentLerpTime += Time.deltaTime;
        }

        perlinChannel.m_AmplitudeGain = 0f;
        enabled = false;
    }
}