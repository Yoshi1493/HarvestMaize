using UnityEngine;

[CreateAssetMenu]
public class CharacterMovementData : ScriptableObject
{
    public Vector2 gravity = new(0, -9.8f);

    [Space]

    public float moveSpeed = 10f;
    public float jumpHeight = 12f;
    public float variableJumpHeightMultiplier = 0.5f;

    [Space]

    public float groundedDamping = 20f;
    public float airborneDamping = 8f;

    [Space]

    public float jumpBufferTime = 0.1f;
    public float coyoteJumpTime = 0.1f;
}