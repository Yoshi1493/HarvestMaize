using System.Collections;
using UnityEngine;

public class InnerWall : Wall
{
    void Awake()
    {
        
    }

    public override void Harvest()
    {
        base.Harvest();
        gameObject.SetActive(false);
    }
}