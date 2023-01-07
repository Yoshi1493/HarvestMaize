using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField] Vector2Int mazeDimensions;
    const int minSize = 5;

    [SerializeField] GameObject wallPrefab;
    [SerializeField] Transform wallParent;

    public int[,] mazeData { get; private set; }

    MazeDataGenerator dataGenerator;

    void Awake()
    {
        mazeData = new int[,]
        {
            {1, 1, 1},
            {1, 0, 1},
            {1, 1, 1}
        };

        dataGenerator = new();
    }

    void Start()
    {
        GenerateNewMaze(mazeDimensions.x, mazeDimensions.y);
    }

    public void GenerateNewMaze(int rowCount, int colCount)
    {
        rowCount = Mathf.Max(rowCount, minSize);
        colCount = Mathf.Max(colCount, minSize);

        mazeData = dataGenerator.FromDimensions(rowCount, colCount);

        for (int r = 0; r < rowCount; r++)
        {
            for (int c = 0; c < colCount; c++)
            {
                if (mazeData[r, c] == 1)
                {
                    GameObject newWall = Instantiate(wallPrefab, wallParent);
                    newWall.name = $"Wall {(r, c)}";                            // debug

                    float x = c - ((colCount - 1) / 2f);
                    float y = r - ((rowCount - 1) / 2f);

                    newWall.transform.position = new(x, y);
                }
            }
        }
    }

}

public class MazeDataGenerator
{
    public float placementThreshold;

    public MazeDataGenerator()
    {
        placementThreshold = 0f;
    }

    public int[,] FromDimensions(int rowCount, int colCount)
    {
        int[,] maze = new int[rowCount, colCount];

        int maxR = maze.GetUpperBound(0);
        int maxC = maze.GetUpperBound(1);

        for (int r = 0; r <= maxR; r++)
        {
            for (int c = 0; c <= maxC; c++)
            {
                // ensure all edges and corners and walls
                if (r == 0 || c == 0 || r == maxR || c == maxC)
                {
                    maze[r, c] = 1;
                }
                // randomly generate maze insides
                else if (r % 2 == 0 && c % 2 == 0)
                {
                    if (Random.value > placementThreshold)
                    {
                        maze[r, c] = 1;

                        int a = Random.value > 0.5f ? 0 : (Random.value > 0.5f ? -1 : 1);
                        int b = a != 0 ? 0 : (Random.value > 0.5f ? -1 : 1);

                        maze[r + a, c + b] = 1;
                    }
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

        // remove 1 edge at random to create the exit
        if (Random.value > 0.5f)
        {
            int c = Random.Range(0, colCount);
            maze[0, c] = 0;
        }
        else
        {
            int r = Random.Range(0, rowCount);
            maze[r, 0] = 0;
        }

        return maze;
    }
}