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

    void Awake()
    {
        FindObjectOfType<MazeGenerator>().GameStartAction += () => enabled = true;
    }

    IEnumerator Start()
    {
        if (volumeProfile.TryGet(out vignette))
        {
            float currentLerpTime = 0f;
            float totalLerpTime = 1f;

            vignette.intensity.value = 0f;

            while (vignette.intensity.value < VignetteIntensity)
            {
                float lerpProgress = currentLerpTime / totalLerpTime;
                vignette.intensity.value = Mathf.Lerp(0f, VignetteIntensity, lerpProgress);

                yield return EndOfFrame;
                currentLerpTime += Time.deltaTime;
            }

            vignette.intensity.value = VignetteIntensity;
        }
    }

#if UNITY_EDITOR
    void OnApplicationQuit()
    {
        vignette.intensity.value = 0;
    }
#endif
}