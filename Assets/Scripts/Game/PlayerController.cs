using UnityEngine;

public class PlayerController : MonoBehaviour
{
    new Transform transform;

    const float MoveSpeed = 5f;
    Vector2 velocity;

    void Awake()
    {
        transform = GetComponent<Transform>();
        FindObjectOfType<PauseHandler>().GamePauseAction += OnGamePaused;
    }

    void Update()
    {
        GetMovementInput();
    }

    void GetMovementInput()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        velocity = new(x, y);

        transform.Translate(Time.deltaTime * MoveSpeed * velocity.normalized, Space.World);
    }

    void OnGamePaused(bool state)
    {
        enabled = !state;
    }
}