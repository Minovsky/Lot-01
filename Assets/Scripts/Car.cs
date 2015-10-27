using UnityEngine;
using UnityEngine.Assertions;

using System.Collections;
using System.Collections.Generic;

public class Car : Moveable
{
    public static readonly float PATIENCE = 3; 

    private bool parking;
    private Coroutine curRoutine;

    private IEnumerator Frustation()
    {
        yield return new WaitForSeconds(PATIENCE);

        parking = false;
        OnDestinationReached();
    }

    public virtual void Park(World.WorldCoord dir)
    {
        Assert.IsTrue(World.Instance.ParkingSpotOpen(worldLocation+dir, dir));

        parking = true;
        MoveIfPossible(dir);

        curRoutine = StartCoroutine(Frustation());
    }

    public virtual void UnPark()
    {
    }

    protected sealed override void OnDestinationReached()
    {
        if(!parking)
            OnRoadReached();
        else
            OnParked();
    }

    protected virtual void OnRoadReached()
    {
        MoveIfPossible(direction);
    }

    protected virtual void OnParked()
    {
        if(curRoutine != null)
            StopCoroutine(curRoutine);
    }
}
