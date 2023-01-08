using UnityEngine;

public class Goal : MonoBehaviour
{
    BoxCollider2D goalTrigger;
    MazeGenerator mazeGenerator;

    void Awake()
    {
        goalTrigger = GetComponent<BoxCollider2D>();
        mazeGenerator = FindObjectOfType<MazeGenerator>();
        mazeGenerator.GameStartAction += SetColliderSize;
    }

    // adjust goal trigger size based on maze dimensions
    void SetColliderSize()
    {
        goalTrigger.enabled = true;
        int[,] mazeDimensions = mazeGenerator.MazeData;
        goalTrigger.size = new(mazeDimensions.GetLength(1), mazeDimensions.GetLength(0));
    }
}