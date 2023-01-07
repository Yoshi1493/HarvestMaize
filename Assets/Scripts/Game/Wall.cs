using System.Collections;
using UnityEngine;

public class Wall : MonoBehaviour
{
    void Awake()
    {
        
    }

    public virtual void Harvest()
    {
        print($"harvesting {name}");
    }
}