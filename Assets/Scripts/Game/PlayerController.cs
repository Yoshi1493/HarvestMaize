using UnityEngine;

public class PlayerController : MonoBehaviour
{
    CharacterController2D controller;

    const float MoveSpeed = 5f;
    Vector2 moveDirection;

    void Awake()
    {
        controller = GetComponent<CharacterController2D>();

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

        controller.Move(Time.deltaTime * MoveSpeed * moveDirection.normalized);
    }

    void OnGamePaused(bool state)
    {
        enabled = !state;
    }
}