using UnityEngine;

[RequireComponent(typeof(MazeGenerator))]
public class GameController : MonoBehaviour
{
    void Awake()
    {
        Application.targetFrameRate = 60;
    }
}