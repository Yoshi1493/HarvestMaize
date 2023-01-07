using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField] Vector2Int mazeDimensions;
    const int minSize = 5;

    [SerializeField] GameObject outerWallPrefab;
    [SerializeField] GameObject innerWallPrefab;
    [SerializeField] Transform wallParent;

    [SerializeField] BoxCollider2D goalTrigger;

    int[,] mazeData;

    void Awake()
    {
        mazeData = new int[,]
        {
            {1, 1, 1},
            {1, 0, 1},
            {1, 1, 1}
        };
    }

    void Start()
    {
        GenerateNewMaze(mazeDimensions.x, mazeDimensions.y);
    }

    public void GenerateNewMaze(int rowCount, int colCount)
    {
        rowCount = Mathf.Max(rowCount, minSize);
        colCount = Mathf.Max(colCount, minSize);

        mazeData = FromDimensions(rowCount, colCount);

        for (int r = 0; r < rowCount; r++)
        {
            for (int c = 0; c < colCount; c++)
            {
                if (mazeData[r, c] != 0)
                {
                    GameObject newWall = null;

                    if (mazeData[r, c] == 1)
                    {
                        newWall = Instantiate(innerWallPrefab, wallParent);
                    }
                    if (mazeData[r, c] == 2)
                    {
                        newWall = Instantiate(outerWallPrefab, wallParent);
                    }

                    newWall.name = $"Wall {(r, c)}";                            // debug

                    float x = c - ((colCount - 1) / 2f);
                    float y = r - ((rowCount - 1) / 2f);

                    newWall.transform.position = new(x, y);
                }
            }
        }

        goalTrigger.size = new(colCount + 1, rowCount + 1);
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

        // create exit
        // exit will always be at cardinal direction regardless of inner wall structure
        if (Random.value > 0.5f)
        {
            int r = Random.value > 0.5f ? 0 : maxR;
            int c = maxC / 2;
            maze[r, c] = 0;
        }
        else
        {
            int r = maxR / 2;
            int c = Random.value > 0.5f ? 0 : maxC;
            maze[r, c] = 0;
        }

        return maze;
    }
}