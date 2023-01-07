using System.Collections;
using UnityEngine;

public class Goal : MonoBehaviour
{
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out PlayerController player))
        {
            player.OnGameOver(true);
        }
    }
}