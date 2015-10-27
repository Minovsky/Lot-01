using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPCCar : Car
{
    public GameObject standInCarPrefab;

    private bool dead = false;

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

    protected override void OnRoadReached()
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

    private IEnumerator RandomLeaveLot()
    {
        yield return new WaitForSeconds(2);

        Car car = World.Instance.GetRandomParkedCar();
        if(car != null)
            car.UnPark();
    }

    protected override void OnParked()
    {
        if(!dead)
        {
            dead = true;
            StartCoroutine(RandomLeaveLot());
            World.Instance.LeaveFrom(worldLocation, direction);
            GetComponentInChildren<SpriteRenderer>().enabled = false;
            Car car = ((GameObject)Instantiate(standInCarPrefab, Vector2.zero, Quaternion.identity)).GetComponent<Car>();
            car.TeleportTo(worldLocation, direction);
        }
    }
}
