using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPCCar : Car
{

    // Use this for initialization
    public override void Start ()
    {

    }

    public void MoveRandomDirection()
    {
        World.WorldCoord[] possibleDirections =
        {
            direction,
            new World.WorldCoord(direction.y, direction.x),
            new World.WorldCoord(-direction.y, -direction.x),
        };

        List<World.WorldCoord> directions = new List<World.WorldCoord>();
        foreach(var dir in possibleDirections)
        {
            if(World.Instance.CanMoveInto(worldLocation + dir, dir) && !World.Instance.IsParkingSpot(worldLocation+dir))
                directions.Add(dir);
        }

        if(directions.Count > 0)
        {
            World.WorldCoord chosenDirection = directions[Random.Range(0, directions.Count)];

            MoveIfPossible(chosenDirection);
        }
    }

    protected override void OnDestinationReached()
    {
        World.WorldCoord parkingOffset;
        if(World.Instance.NextToOpenParking(worldLocation, direction, out parkingOffset))
        {
            Park(parkingOffset);
        }
        else
        {
            MoveRandomDirection();
        }
    }

    protected override void OnParked()
    {
    }
}
