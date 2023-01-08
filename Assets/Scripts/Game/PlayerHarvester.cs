using System;
using UnityEngine;
using static MazeGeneratorHelper;

public class PlayerHarvester : MonoBehaviour
{
    PlayerController player;
    MazeGenerator mazeGenerator;

    [SerializeField] IntObject harvestCounter;

    public event Action HarvestAction;
    public event Action ThresholdReachedAction;

    [SerializeField] LayerMask collisionMask;
    RaycastHit2D raycastHit;
    const float rayDistance = 0.35f;

    const int HarvestThreshold = 10;

    void Awake()
    {
        player = GetComponent<PlayerController>();

        mazeGenerator = FindObjectOfType<MazeGenerator>();
        mazeGenerator.GameStartAction += () => enabled = true;

        harvestCounter.value = 0;
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
                print($"hit {raycastHit.transform.name}");
                if (raycastHit.transform.TryGetComponent(out Wall wall))
                {
                    (int row, int col) = WorldSpaceToMazeIndex(mazeGenerator.RowCount, mazeGenerator.ColCount, wall.transform.position);
                    mazeGenerator.MazeData[row, col] = 0;
                    mazeGenerator.WallObjects[row, col] = null;

                    wall.Harvest();
                    OnHarvest();
                }
            }
        }
    }

    void OnHarvest()
    {
        harvestCounter.value++;
        HarvestAction?.Invoke();

        if (harvestCounter.value == HarvestThreshold)
        {
            ThresholdReachedAction?.Invoke();
        }
    }
}