using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using static CoroutineHelper;

public class Flashlight : MonoBehaviour
{
    [SerializeField] VolumeProfile volumeProfile;
    Vignette vignette;
    const float VignetteIntensity = 0.5f;

    IEnumerator Start()
    {
        if (volumeProfile.TryGet(out vignette))
        {
            float currentLerpTime = 0f;
            float totalLerpTime = 1f;

            while (vignette.intensity.value < VignetteIntensity)
            {
                float lerpProgress = currentLerpTime / totalLerpTime;
                vignette.intensity.value = lerpProgress;

                yield return EndOfFrame;
                currentLerpTime += Time.deltaTime;
            }

            vignette.intensity.Override(VignetteIntensity);
        }
    }

    void OnApplicationQuit()
    {
        vignette.intensity.value = 0;
    }
}