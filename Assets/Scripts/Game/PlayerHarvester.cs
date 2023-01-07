using System;
using UnityEngine;

public class PlayerHarvester : MonoBehaviour
{
    PlayerController player;

    public event Action HarvestAction;

    [SerializeField] LayerMask collisionMask;
    RaycastHit2D raycastHit;
    const float rayDistance = 0.35f;

    void Awake()
    {
        player = GetComponent<PlayerController>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire"))
        {
            // shoot out a ray based on last direction moved
            Vector2 rayOrigin = transform.position;
            Vector2 rayDirection = player.LastNonzeroDirection;

            // confine rayDirection to one cardinal direction (prioritize left/right)
            if (rayDirection.x != 0 && rayDirection.y != 0)
            {
                rayDirection.y = 0;
            }

            Debug.DrawRay(transform.position, rayDistance * rayDirection, Color.cyan);
            raycastHit = Physics2D.Raycast(rayOrigin, rayDirection, rayDistance, collisionMask);

            if (raycastHit)
            {
                if (raycastHit.transform.TryGetComponent(out Wall wall))
                {
                    wall.Harvest();
                    HarvestAction?.Invoke();
                }
            }
        }
    }
}