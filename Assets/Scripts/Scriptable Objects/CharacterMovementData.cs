using UnityEngine;

[CreateAssetMenu]
public class CharacterMovementData : ScriptableObject
{
    public Vector2 gravity = new(0, -49f);

    [Space]

    public float moveSpeed = 8f;
    public float jumpHeight = 20f;
    public float variableJumpHeightMultiplier = 0.5f;

    [Space]

    public float groundedDamping = 0.05f;
    public float airborneDamping = 0.10f;

    [Space]

    public float jumpBufferTime = 0.1f;
    public float coyoteJumpTime = 0.1f;
}