using System.Collections;
using UnityEngine;
using static CoroutineHelper;

public class PlayerSlashEffect : MonoBehaviour
{
    PlayerController player;
    
    SpriteRenderer spriteRenderer;
    Material slashMaterial;

    IEnumerator slashAnimationCoroutine;
    float animationSpeed;

    void Awake()
    {
        player = GetComponentInParent<PlayerController>();

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.enabled = false;

        slashMaterial = spriteRenderer.material;
        slashMaterial.SetFloat("_AnimationTime", 0f);
        animationSpeed = slashMaterial.GetFloat("_AnimationSpeed");

        GetComponentInParent<PlayerHarvester>().HarvestAction += OnPlayerHarvest;
    }

    void Update()
    {
        Vector2 dir = player.LastNonzeroDirection;
        float zRotation = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        transform.eulerAngles = zRotation * Vector3.forward;
    }

    void OnPlayerHarvest()
    {
        if (slashAnimationCoroutine != null)
        {
            StopCoroutine(slashAnimationCoroutine);
        }

        slashAnimationCoroutine = AnimateSlash();
        StartCoroutine(slashAnimationCoroutine);
    }

    IEnumerator AnimateSlash()
    {
        float currentLerpTime = 0f;
        float totalLerpTime = 1 / animationSpeed;

        spriteRenderer.enabled = true;

        while (currentLerpTime < totalLerpTime)
        {
            float lerpProgress = currentLerpTime / totalLerpTime;
            float offset = Mathf.Lerp(0f, 1f, lerpProgress);
            slashMaterial.SetFloat("_AnimationTime", offset);

            yield return EndOfFrame;
            currentLerpTime += Time.deltaTime;
        }

        spriteRenderer.enabled = false;
    }
}