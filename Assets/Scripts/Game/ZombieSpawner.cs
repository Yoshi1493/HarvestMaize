using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    [SerializeField] GameObject zombiePrefab;

    List<(int row, int col)> openSpaces = new();
    MazeGenerator mazeGenerator;
    const int ZombieCount = 6;

    void Awake()
    {
        mazeGenerator = FindObjectOfType<MazeGenerator>();
        mazeGenerator.GameStartAction += SpawnZombies;
    }

    void SpawnZombies()
    {
        var mazeData = mazeGenerator.MazeData;
        int rowCount = mazeData.GetUpperBound(0);
        int colCount = mazeData.GetUpperBound(1);

        // for all maze indexes that are 1 space inside the outer wall, and not filled by a wall,
        // add to list of open spaces
        for (int r = 1; r < rowCount; r++)
        {
            if (mazeData[r, 1] == 0)
            {                
                openSpaces.Add((r, 1));
            }
            if (mazeData[r, colCount - 1] == 0)
            {
                openSpaces.Add((r, colCount - 1));
            }
        }

        for (int c = 1; c < rowCount; c++)
        {
            if (mazeData[1, c] == 0)
            {
                openSpaces.Add((1, c));
            }
            if (mazeData[rowCount - 1, c] == 0)
            {
                openSpaces.Add((rowCount - 1, c));
            }
        }

        for (int i = 0; i < ZombieCount; i++)
        {
            int rand = Random.Range(0, openSpaces.Count);
            (float x, float y) = mazeGenerator.MazeIndexToWorldSpace(rowCount + 1, colCount + 1, openSpaces[rand].row, openSpaces[rand].col);
            
            GameObject newZombie = Instantiate(zombiePrefab, transform);
            newZombie.transform.position = new(x, y);

            // ensure nothing gets instantiated at the same position more than once
            openSpaces.RemoveAt(rand);
        }
    }
}