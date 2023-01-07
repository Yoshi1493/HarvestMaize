using UnityEngine;

public class PlayerController : MonoBehaviour
{
    CharacterController2D controller;
    SpriteRenderer spriteRenderer;

    const float MoveSpeed = 5f;
    Vector2 moveDirection;
    bool lastNonzeroDirection;

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

        if (moveDirection.x > 0 && lastNonzeroDirection)
        {
            lastNonzeroDirection = false;
            UpdateSprite();
        }
        else if (moveDirection.x < 0 && !lastNonzeroDirection)
        {
            lastNonzeroDirection = true;
            UpdateSprite();
        }

        controller.Move(Time.deltaTime * MoveSpeed * moveDirection.normalized);
    }

    void UpdateSprite()
    {
        spriteRenderer.flipX = lastNonzeroDirection;
    }

    void OnGamePaused(bool state)
    {
        enabled = !state;
    }
}