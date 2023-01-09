using System.Collections;
using UnityEngine;
using static CoroutineHelper;

public class PlayerEscapeAnimation : MonoBehaviour
{
    CharacterController2D controller;
    PlayerController player;

    [SerializeField] AnimationCurve speedFalloffCurve;

    void Awake()
    {
        controller = GetComponent<CharacterController2D>();
        player = GetComponent<PlayerController>();
        player.GameOverAction += OnGameOver;
    }

    void OnGameOver(bool playerWon)
    {
        if (playerWon)
        {
            enabled = true;
        }
    }

    void OnEnable()
    {
        StartCoroutine(RunAway());
    }

    IEnumerator RunAway()
    {
        float currentTime = 0f;
        float totalTime = 1f;

        while (currentTime < totalTime)
        {
            float speedFalloff = speedFalloffCurve.Evaluate(currentTime / totalTime);
            controller.Move(Time.deltaTime * PlayerController.MoveSpeed * speedFalloff * player.LastNonzeroDirection);

            yield return EndOfFrame;
            currentTime += Time.deltaTime;
        }
    }
}