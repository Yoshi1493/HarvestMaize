using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    CharacterController2D controller;
    SpriteRenderer spriteRenderer;

    public const float MoveSpeed = 5f;
    Vector2 moveDirection;

    public Vector2 LastNonzeroDirection { get; private set; }
    public bool lastNonzeroDirectionX;

    public event Action<bool> GameOverAction;

    void Awake()
    {
        controller = GetComponent<CharacterController2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        FindObjectOfType<PauseHandler>().GamePauseAction += OnGamePaused;
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        moveDirection.x = Input.GetAxisRaw("Horizontal");
        moveDirection.y = Input.GetAxisRaw("Vertical");

        if (!moveDirection.Equals(Vector2.zero) && !moveDirection.Equals(LastNonzeroDirection))
        {
            LastNonzeroDirection = moveDirection;
        }

        if (moveDirection.x > 0 && lastNonzeroDirectionX)
        {
            lastNonzeroDirectionX = false;
            UpdateSprite();
        }
        else if (moveDirection.x < 0 && !lastNonzeroDirectionX)
        {
            lastNonzeroDirectionX = true;
            UpdateSprite();
        }

        controller.Move(Time.deltaTime * MoveSpeed * moveDirection.normalized);
    }

    void UpdateSprite()
    {
        spriteRenderer.flipX = lastNonzeroDirectionX;
    }

    void OnGamePaused(bool state)
    {
        enabled = !state;
    }

    public void OnGameOver(bool playerWon)
    {
        GameOverAction?.Invoke(playerWon);
        enabled = false;
    }
}