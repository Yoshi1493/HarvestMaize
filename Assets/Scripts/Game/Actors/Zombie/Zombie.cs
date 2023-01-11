using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CoroutineHelper;

public class Zombie : Actor
{
    public (int row, int col) currentCoordinate;

    IEnumerator moveCoroutine;
    const float MoveSpeed = 2f;

    const int MoveDirectionCount = 4;

    readonly List<Vector2> moveDirections = new(MoveDirectionCount)
    {
        new(0, 1),
        new(1, 0),
        new(0, -1),
        new(-1, 0)
    };

    List<Vector2> possibleMoveDirections = new(MoveDirectionCount);

    protected override void Awake()
    {
        base.Awake();

        FindObjectOfType<PlayerController>().GameOverAction += OnGameOver;
    }

    protected override void Move()
    {
        if (moveDirection == Vector2.zero)
        {
            moveDirection = GetNextMoveDirection();
            LastNonzeroDirection = moveDirection;
            UpdateSprite();
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

    protected override void PlayAudio()
    {
        if (moveDirection != Vector2.zero)
        {
            if (!aux.isPlaying)
            {
                aux.Play();
            }
        }
    }

    Vector2 GetNextMoveDirection()
    {
        possibleMoveDirections.Clear();

        // get possible move directions
        for (int i = 0; i < MoveDirectionCount; i++)
        {
            Vector2 dir = moveDirections[i];
            int nextRow = currentCoordinate.row + (int)dir.y;
            int nextCol = currentCoordinate.col + (int)dir.x;

            // make sure not to move out of maze bounds
            if (nextRow > 0 && nextRow < mazeGenerator.RowCount - 1 && nextCol > 0 && nextCol < mazeGenerator.ColCount - 1)
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
                possibleMoveDirections.Remove(LastNonzeroDirection * -1);

                int rand = Random.Range(0, possibleMoveDirections.Count);
                return possibleMoveDirections[rand];
            }
        }

        return Vector2.zero;
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
        currentCoordinate = (currentCoordinate.row + (int)moveDirection.y, currentCoordinate.col + (int)moveDirection.x);
        moveDirection = Vector2.zero;

        moveCoroutine = null;
    }

    void UpdateSprite()
    {
        if (moveDirection.x == -1)
        {
            spriteRenderer.flipX = true;
        }
        if (moveDirection.x == 1)
        {
            spriteRenderer.flipX = false;
        }
    }

    void OnGameOver(bool _)
    {
        StopAllCoroutines();
        enabled = false;
    }
}