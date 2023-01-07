using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField] Vector2Int mazeDimensions;
    const int minSize = 3;

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

                    newWall.transform.position = new(c, r);
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
        placementThreshold = 0.1f;
    }

    public int[,] FromDimensions(int rowCount, int colCount)
    {
        int[,] maze = new int[rowCount, colCount];

        int maxR = maze.GetUpperBound(0);
        int maxC = maze.GetUpperBound(1);

        for (int r = 0; r <= maxR; r++)
        {
            for (int c = 0; c < maxC; c++)
            {
                if (r == 0 || c == 0 || r == maxR || c == maxC)
                {
                    maze[r, c] = 1;
                }

                else if (r % 2 == 0 && c % 2 == 0)
                {
                    if (Random.value > placementThreshold)
                    {
                        maze[r, c] = 1;

                        int a = Random.value < 0.5f ? 0 : (Random.value < 0.5f ? -1 : 1);

                        int b = a != 0 ? 0 : (Random.value < 0.5f ? -1 : 1);
                        maze[r + a, c + b] = 1;
                    }
                }
            }
        }

        return maze;
    }
}