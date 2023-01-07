using System.Collections;
using System.Reflection;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using static CoroutineHelper;

public class PlayerLightSource : MonoBehaviour
{
    Light2D spotlight;
    const float SpotlightFalloffStrength = 0.5f;

    readonly FieldInfo falloffField = typeof(Light2D).GetField("m_FalloffIntensity", BindingFlags.NonPublic | BindingFlags.Instance);

    void Awake()
    {
        spotlight = GetComponent<Light2D>();
        FindObjectOfType<MazeGenerator>().GameStartAction += () => enabled = true;
    }

    IEnumerator Start()
    {
        float currentLerpTime = 0f;
        float totalLerpTime = 1f;

        falloffField.SetValue(spotlight, 1f);

        while (spotlight.falloffIntensity > SpotlightFalloffStrength)
        {
            float lerpProgress = currentLerpTime / totalLerpTime;
            falloffField.SetValue(spotlight, Mathf.Lerp(1f, SpotlightFalloffStrength, lerpProgress));

            yield return EndOfFrame;
            currentLerpTime += Time.deltaTime;
        }

        falloffField.SetValue(spotlight, SpotlightFalloffStrength);
    }
}