using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPCCar : Car
{
    public GameObject standInCarPrefab;
	public Sprite[] carSprites = new Sprite[4];

    private bool dead = false;
	private int randomValue;

    public int color = -1;
    // Use this for initialization
    public override void Start ()
    {
        if (color < 0) {
		randomValue = Random.Range (0, 4);
		GetComponentInChildren<SpriteRenderer> ().sprite = carSprites [randomValue];
        color = randomValue;
        }


    }
	public void ChangeColor(int value)
	{
        color = value;
		GetComponentInChildren<SpriteRenderer> ().sprite = carSprites [value];
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
            //MoveRandomDirection();
        }
    }

    private IEnumerator RandomLeaveLot()
    {
        yield return new WaitForSeconds(2);

        Car car = World.Instance.GetRandomParkedCar();
        if(car != null)
            car.UnPark();
        Destroy(this.gameObject);
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
			car.GetComponentInChildren<StandInCar> ().ChangeColor (randomValue);
            car.TeleportTo(worldLocation, direction);
        }
    }
}
