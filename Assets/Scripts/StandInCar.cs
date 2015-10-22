using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StandInCar : Car
{
    // Use this for initialization
    public override void Start ()
    {

    }

    public override void Update()
    {
        /* DO NOTHING */
    }

    public override void UnPark()
    {
        World.Instance.LeaveFrom(worldLocation, direction);
        Destroy(this.gameObject);
    }
}
