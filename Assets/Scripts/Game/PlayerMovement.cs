using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] CharacterMovementData playerData;

    CharacterController2D controller;
    Vector3 velocity;
    float smoothVelocityX = 0f;

    float maxFallSpeed;
    float jumpBufferCounter = 0f;
    float coyoteTimeCounter = 0f;

    void Awake()
    {
        controller = GetComponent<CharacterController2D>();

        maxFallSpeed = playerData.gravity.y * 0.5f;
    }

    void Update()
    {
        float xSpeedMultiplier = Input.GetAxisRaw("Horizontal");

        if (controller.IsGrounded)
        {
            coyoteTimeCounter = playerData.coyoteJumpTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = playerData.jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if (coyoteTimeCounter > 0f && jumpBufferCounter > 0f)
        {
            velocity.y = playerData.jumpHeight;

            if (!Input.GetButton("Jump"))
            {
                velocity.y *= playerData.variableJumpHeightMultiplier;
            }

            jumpBufferCounter = 0f;
        }

        if (Input.GetButtonUp("Jump"))
        {
            if (velocity.y > 0f)
            {
                velocity.y *= playerData.variableJumpHeightMultiplier;
            }

            coyoteTimeCounter = 0f;
        }

        // apply x speed + smoothing
        float dampingFactor = controller.IsGrounded ? playerData.groundedDamping : playerData.airborneDamping;
        velocity.x = Mathf.SmoothDamp(velocity.x, playerData.moveSpeed * xSpeedMultiplier, ref smoothVelocityX, dampingFactor);

        // apply gravity if airborne
        if (!controller.IsGrounded)
        {
            velocity.y = Mathf.Max(velocity.y + (playerData.gravity.y * Time.deltaTime), maxFallSpeed);
        }

        // perform movement
        controller.Move(Time.deltaTime * velocity);

        // update velocity based on controller velocity
        velocity = controller.Velocity;
    }
}