using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CoroutineHelper;

public class Zombie : MonoBehaviour
{
    new Transform transform;

    MazeGenerator mazeGenerator;
    (int maxRow, int maxCol) mazeBounds;
    public (int row, int col) currentCoordinate;

    IEnumerator moveCoroutine;
    Vector2Int moveDirection;
    Vector2Int lastNonzeroDirection;
    const float MoveSpeed = 2f;

    const int MoveDirectionCount = 4;

    readonly List<Vector2Int> moveDirections = new(MoveDirectionCount)
    {
        new(0, 1),
        new(1, 0),
        new(0, -1),
        new(-1, 0)
    };

    List<Vector2Int> possibleMoveDirections = new(MoveDirectionCount);

    void Awake()
    {
        transform = GetComponent<Transform>();

        FindObjectOfType<PlayerController>().GameOverAction += OnGameOver;
        mazeGenerator = FindObjectOfType<MazeGenerator>();

        mazeBounds = (mazeGenerator.MazeData.GetUpperBound(0), mazeGenerator.MazeData.GetUpperBound(1));
    }

    void Update()
    {
        if (moveDirection == Vector2.zero)
        {
            moveDirection = GetNextMoveDirection();
            lastNonzeroDirection = moveDirection;
        }
        else
        {
            if (moveCoroutine == null)
            {
                Vector2 nextPos = (Vector2)transform.position + moveDirection;

                moveCoroutine = MoveTo(nextPos);
                StartCoroutine(moveCoroutine);
            }
        }
    }

    Vector2Int GetNextMoveDirection()
    {
        possibleMoveDirections.Clear();

        // get possible move directions
        for (int i = 0; i < MoveDirectionCount; i++)
        {
            Vector2Int dir = moveDirections[i];
            (int nextRow, int nextCol) = (currentCoordinate.row + dir.y, currentCoordinate.col + dir.x);

            // make sure not to move out of maze bounds
            if (nextRow > 0 && nextRow < mazeBounds.maxRow && nextCol > 0 && nextCol < mazeBounds.maxCol)
            {
                // add to list of possible move directions if space is empty
                if (mazeGenerator.MazeData[nextRow, nextCol] == 0)
                {
                    possibleMoveDirections.Add(dir);
                }
            }
        }

        // randomly pick possible move direction
        if (possibleMoveDirections.Count > 0)
        {
            if (possibleMoveDirections.Count == 1)
            {
                return possibleMoveDirections[0];
            }
            else
            {
                // don't pick direction that is directly opposite of current moveDirection
                possibleMoveDirections.Remove(lastNonzeroDirection * -1);

                int rand = Random.Range(0, possibleMoveDirections.Count);
                return possibleMoveDirections[rand];
            }
        }

        return Vector2Int.zero;
    }

    // lerp position from current position to <endPos>
    IEnumerator MoveTo(Vector2 endPos)
    {
        float currentLerpTime = 0f;
        float totalLerpTime = 1 / MoveSpeed;
        Vector2 startPos = transform.position;

        while (currentLerpTime < totalLerpTime)
        {
            float lerpProgress = currentLerpTime / totalLerpTime;
            transform.position = Vector2.Lerp(startPos, endPos, lerpProgress);

            yield return EndOfFrame;
            currentLerpTime += Time.deltaTime;
        }

        transform.position = endPos;
        currentCoordinate = (currentCoordinate.row + moveDirection.y, currentCoordinate.col + moveDirection.x);
        moveDirection = Vector2Int.zero;

        moveCoroutine = null;
    }

    void OnGameOver(bool _)
    {
        StopAllCoroutines();
        enabled = false;
    }
}