using System.Collections;
using UnityEngine;

public class InnerWall : Wall
{
    public override void Harvest()
    {
        base.Harvest();
        gameObject.SetActive(false);
    }
}