using System.Collections;
using UnityEngine;
using static CoroutineHelper;
using static MazeGeneratorHelper;

public enum MazeSize
{ 
    Small,
    Medium,
    Large
}

public class MazeGenerator : MonoBehaviour
{
    [SerializeField] IntObject selectDifficulty;
    [SerializeField] Vector2IntObject[] mazeDimensionObjects;

    [SerializeField] Transform wallParent;
    [SerializeField] GameObject outerWallPrefab;
    [SerializeField] GameObject innerWallPrefab;

    public event System.Action GameStartAction;

    public int[,] MazeData { get; private set; }
    public GameObject[,] WallObjects { get; private set; }

    public int RowCount { get => MazeData.GetLength(0); }
    public int ColCount { get => MazeData.GetLength(1); }

    void Awake()
    {
        FindObjectOfType<PlayerHarvester>().ThresholdReachedAction += CreateExit;
    }

    IEnumerator Start()
    {
        Vector2Int selectedMazeDimension = mazeDimensionObjects[selectDifficulty.value].value;
        yield return GenerateNewMaze(selectedMazeDimension.x, selectedMazeDimension.y);

        GameStartAction?.Invoke();
    }

    IEnumerator GenerateNewMaze(int rowCount, int colCount)
    {
        MazeData = FromDimensions(rowCount, colCount);
        WallObjects = new GameObject[rowCount, colCount];

        // construct maze
        for (int r = 0; r < rowCount; r++)
        {
            for (int c = 0; c < colCount; c++)
            {
                GameObject newWall = null;

                Vector3 pos = MazeIndexToWorldSpace(rowCount, colCount, r, c);
                pos.z = (rowCount - r) * -0.1f;

                if (MazeData[r, c] != 0)
                {
                    if (MazeData[r, c] == 1)
                    {
                        newWall = Instantiate(innerWallPrefab, wallParent);
                    }
                    if (MazeData[r, c] == 2)
                    {
                        newWall = Instantiate(outerWallPrefab, wallParent);
                    }

                    newWall.transform.position = pos;
                    WallObjects[r, c] = newWall;

                    yield return EndOfFrame;
                }
            }
        }

        yield return EndOfFrame;
    }

    int[,] FromDimensions(int rowCount, int colCount)
    {
        int[,] maze = new int[rowCount, colCount];

        int maxR = maze.GetUpperBound(0);
        int maxC = maze.GetUpperBound(1);

        for (int r = 0; r <= maxR; r++)
        {
            for (int c = 0; c <= maxC; c++)
            {
                // ensure all edges and corners and outer walls
                if (r == 0 || c == 0 || r == maxR || c == maxC)
                {
                    maze[r, c] = 2;
                }
                // randomly generate inner walls
                else if (r % 2 == 0 && c % 2 == 0)
                {
                    maze[r, c] = 1;

                    int a = Random.value > 0.5f ? 0 : (Random.value > 0.5f ? -1 : 1);
                    int b = a != 0 ? 0 : (Random.value > 0.5f ? -1 : 1);

                    maze[r + a, c + b] = 1;

                }
            }
        }

        // create 3x3 opening for starting point
        for (int r = -1; r <= 1; r++)
        {
            for (int c = -1; c <= 1; c++)
            {
                maze[maxR / 2 + r, maxC / 2 + c] = 0;
            }
        }

        return maze;
    }

    void CreateExit()
    {
        int maxR = RowCount - 1;
        int maxC = ColCount - 1;

        int r, c;

        // exit will always be at cardinal direction regardless of inner wall structure
        if (Random.value > 0.5f)
        {
            r = Random.value > 0.5f ? 0 : maxR;
            c = maxC / 2;            
        }
        else
        {
            r = maxR / 2;
            c = Random.value > 0.5f ? 0 : maxC;
        }

        MazeData[r, c] = 0;
        WallObjects[r, c].SetActive(false);
    }
}

public static class MazeGeneratorHelper
{
    // convert maze index to world space based on row and column count
    // maze index [0, 0] = bottom left
    public static Vector3 MazeIndexToWorldSpace(int rowCount, int colCount, int row, int col)
    {
        float x = col - ((colCount - 1) / 2f);
        float y = row - ((rowCount - 1) / 2f);

        return new(x, y);
    }

    public static (int, int) WorldSpaceToMazeIndex(int rowCount, int colCount, Vector3 v)
    {
        int r = ((rowCount - 1) / 2) + (int)v.y;
        int c = ((colCount - 1) / 2) + (int)v.x;

        return (r, c);
    }
}