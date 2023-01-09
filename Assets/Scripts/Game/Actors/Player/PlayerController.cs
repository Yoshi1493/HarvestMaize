using System;
using UnityEngine;

public class PlayerController : Actor
{
    CharacterController2D controller;
     
    public const float MoveSpeed = 5f;
    bool lastNonzeroDirectionX;

    public event Action<bool> GameOverAction;

    protected override void Awake()
    {
        base.Awake();

        controller = GetComponent<CharacterController2D>();
        controller.OnTriggerEnterAction += OnTriggerEnterEvent;
        controller.OnTriggerExitAction += OnTriggerExitEvent;

        mazeGenerator.GameStartAction += () => enabled = true;
    }

    protected override void Move()
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

    public void OnGameOver(bool playerWon)
    {
        GameOverAction?.Invoke(playerWon);
        enabled = false;
    }

    void OnTriggerEnterEvent(Collider2D other)
    {
        if (other.TryGetComponent(out Zombie zombie))
        {
            OnGameOver(false);
        }
    }

    void OnTriggerExitEvent(Collider2D other)
    {
        if (other.TryGetComponent(out Goal goal))
        {
            OnGameOver(true);
            AudioManager.Instance.PlaySound("game_win");
        }
    }
}