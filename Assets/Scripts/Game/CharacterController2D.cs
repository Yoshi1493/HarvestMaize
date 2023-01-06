using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
    struct RaycastOrigins
    {
        public Vector3 bottomLeft;
        public Vector3 bottomRight;
        public Vector3 topLeft;
    }

    public class CollisionState
    {
        public bool right;
        public bool left;
        public bool top;
        public bool bottom;
        public bool becameGroundedThisFrame;
        public bool wasGroundedLastFrame;

        public bool IsColliding => right || left || top || bottom;

        public void ResetCollision() => right = left = top = bottom = becameGroundedThisFrame = false;
    }

    [HideInInspector] public new Transform transform;
    [HideInInspector] public new BoxCollider2D collider;
    [HideInInspector] public new Rigidbody2D rigidbody;

    [SerializeField] LayerMask platformMask;

    RaycastOrigins raycastOrigins;
    RaycastHit2D raycastHit;

    const int HorizontalRayCount = 8;
    const int VerticalRayCount = 5;
    float horizontalRayDistance;                // vertical distance between horizontal rays
    float verticalRayDistance;                  // horizontal distance between vertical rays

    const float SkinWidth = 0.01f;

    [HideInInspector] public CollisionState collisionState = new();
    public bool IsGrounded => collisionState.bottom;

    public Vector3 Velocity { get; private set; }

    void Awake()
    {
        // cache components
        transform = GetComponent<Transform>();
        collider = GetComponent<BoxCollider2D>();
        rigidbody = GetComponent<Rigidbody2D>();

        CalculateRayDistances();
    }

    // compute distance between horizontal/vertical rays used for collision detection, taking skin width constant into account
    void CalculateRayDistances()
    {
        float actualColliderHeight = collider.size.y - (SkinWidth * 2f);
        horizontalRayDistance = actualColliderHeight / (HorizontalRayCount - 1);

        float actualColliderWidth = collider.size.x - (SkinWidth * 2f);
        verticalRayDistance = actualColliderWidth / (VerticalRayCount - 1);
    }

    public void Move(Vector3 deltaMovement)
    {
        // save current grounded state
        collisionState.wasGroundedLastFrame = collisionState.bottom;

        // reset state
        collisionState.ResetCollision();

        RecalculateRaycastOrigins();

        // check horizontal movement
        if (deltaMovement.x != 0f)
        {
            MoveX(ref deltaMovement);
        }

        // check vertical movement
        if (deltaMovement.y != 0f)
        {
            MoveY(ref deltaMovement);
        }

        // ensure no delta z-movement since this is 2D
        deltaMovement.z = 0f;

        // perform movement
        transform.Translate(deltaMovement, Space.World);

        // update velocity if time has passed
        if (Time.deltaTime > 0f)
        {
            Velocity = deltaMovement / Time.deltaTime;
        }

        // set becameGrounded state based on previous and current collision state
        if (!collisionState.wasGroundedLastFrame && IsGrounded)
        {
            collisionState.becameGroundedThisFrame = true;
        }
    }

    // reposition raycast origins to the current bounds of the collider, inset by the skin width constant
    void RecalculateRaycastOrigins()
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(SkinWidth * -2f);

        raycastOrigins.bottomLeft = bounds.min;
        raycastOrigins.bottomRight = new(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new(bounds.min.x, bounds.max.y);
    }

    void MoveX(ref Vector3 deltaMovement)
    {
        bool movingRight = deltaMovement.x > 0f;
        float rayDistance = Mathf.Abs(deltaMovement.x) + SkinWidth;
        Vector3 rayDirection = movingRight ? Vector3.right : Vector3.left;
        Vector3 initialRayOrigin = movingRight ? raycastOrigins.bottomRight : raycastOrigins.bottomLeft;

        for (int i = 0; i < HorizontalRayCount; i++)
        {
            Vector2 ray = new(initialRayOrigin.x, initialRayOrigin.y + (i * horizontalRayDistance));
            raycastHit = Physics2D.Raycast(ray, rayDirection, rayDistance, platformMask);

            DrawRay(ray, rayDirection, rayDistance);

            if (raycastHit)
            {
                // set deltaMovement upon collision, recalculate rayDistance based on new deltaMovement
                deltaMovement.x = raycastHit.point.x - ray.x;
                rayDistance = Mathf.Abs(deltaMovement.x);

                // remove skin width constant from deltaMovement, update appropriate collision state
                if (movingRight)
                {
                    deltaMovement.x -= SkinWidth;
                    collisionState.right = true;
                }
                else
                {
                    deltaMovement.x += SkinWidth;
                    collisionState.left = true;
                }
            }
        }
    }

    void MoveY(ref Vector3 deltaMovement)
    {
        bool movingUp = deltaMovement.y > 0f;
        float rayDistance = Mathf.Abs(deltaMovement.y) + SkinWidth;
        Vector3 rayDirection = movingUp ? Vector3.up : Vector3.down;
        Vector3 initialRayOrigin = movingUp ? raycastOrigins.topLeft : raycastOrigins.bottomLeft;

        // apply delta x-movement so that raycast is done from the position as if it had moved already
        initialRayOrigin.x += deltaMovement.x;

        for (int i = 0; i < VerticalRayCount; i++)
        {
            Vector2 ray = new(initialRayOrigin.x + (i * verticalRayDistance), initialRayOrigin.y);
            raycastHit = Physics2D.Raycast(ray, rayDirection, rayDistance, platformMask);

            DrawRay(ray, rayDirection, rayDistance);

            if (raycastHit)
            {
                deltaMovement.y = raycastHit.point.y - ray.y;
                rayDistance = Mathf.Abs(deltaMovement.y);

                if (movingUp)
                {
                    deltaMovement.y -= SkinWidth;
                    collisionState.top = true;
                }
                else
                {
                    deltaMovement.y += SkinWidth;
                    collisionState.bottom = true;
                }
            }
        }
    }

    #region DEBUG
    
    void DrawRay(Vector3 origin, Vector3 direction, float distance)
    {
        Debug.DrawRay(origin, distance * direction, Color.magenta);
    }

    #endregion
}