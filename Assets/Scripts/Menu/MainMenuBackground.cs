using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using static CoroutineHelper;

public class MainMenuBackground : MonoBehaviour
{
    [SerializeField] VolumeProfile volumeProfile;
    Bloom bloom;
    const float MaxBloomIntensity = 0.4f;

    void Awake()
    {
        volumeProfile.TryGet(out bloom);
        bloom.intensity.value = 0f;
    }

    IEnumerator Start()
    {
        while (enabled)
        {
            if (bloom != null)
            {
                float currentLerpTime = 0f;
                float totalLerpTime = 4f;

                while (bloom.intensity.value != MaxBloomIntensity)
                {
                    float lerpProgress = currentLerpTime / totalLerpTime;
                    bloom.intensity.value = Mathf.Lerp(0f, MaxBloomIntensity, lerpProgress);

                    yield return EndOfFrame;
                    currentLerpTime += Time.deltaTime;
                }

                bloom.intensity.value = MaxBloomIntensity;
                currentLerpTime = 0f;

                while (bloom.intensity.value != 0f)
                {
                    float lerpProgress = currentLerpTime / totalLerpTime;
                    bloom.intensity.value = Mathf.Lerp(MaxBloomIntensity, 0f, lerpProgress);

                    yield return EndOfFrame;
                    currentLerpTime += Time.deltaTime;
                }

                bloom.intensity.value = 0f;
            }
        }
    }

#if UNITY_EDITOR
    void OnApplicationQuit()
    {
        bloom.intensity.value = 0;
    }
#endif
}