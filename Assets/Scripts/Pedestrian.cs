using UnityEngine;
using UnityEngine.Assertions;

using System;
using System.Collections;
using System.Collections.Generic;

public class Pedestrian : Moveable
{
    protected static World.WorldCoord leaveLocation = new World.WorldCoord(0, World.WORLD_HEIGHT);
    protected bool unhinged = false;

    private static readonly float OFFSCREEN_THRESHOLD = 10f;
    private static readonly float WIDTH = .16f;
    private static readonly float HALO_SIZE = 0f;

    public string trainExitTag = "TrainMarker";
    private GameObject trainExitLocation;

    private List<Car> inPath;

    public override void Start()
    {
        base.Start();

        inPath = new List<Car>();

        var r = UnityEngine.Random.Range(0, World.WORLD_WIDTH/2);
        var startTarget = new World.WorldCoord(r*2, -1);

        TeleportTo(startTarget, new World.WorldCoord(0, 1));

        trainExitLocation = GameObject.FindGameObjectWithTag(trainExitTag);
        transform.position = trainExitLocation.transform.position;
    }

    public override void Update()
    {
        List<Car> carsMissing = new List<Car>();

        foreach(var car in inPath)
        {
            if(!CheckInFront(car))
            {
                carsMissing.Add(car);
            }
            else
            {
                car.StopWaiting();
            }
        }
        foreach(var car in carsMissing)
        {
            car.WaitForPedestrian();
            inPath.Remove(car);
        }

        if(inPath.Count == 0)
        {
            if(!unhinged)
            {
                base.Update();
            }
            else
            {
                transform.position = transform.position + (Vector3)(speed*new Vector2(direction.x, direction.y)*Time.deltaTime);
                if(Vector2.Distance(transform.position, World.Instance.GetCenterGridWorldLocation(worldLocation)) >= OFFSCREEN_THRESHOLD)
                {
                    Destroy(this.gameObject);
                }
            }
        }
    }

    protected bool CheckInFront(Car c)
    {
        float forwardDistance = GetComponent<BoxCollider2D>().size.y;
        var carBounds = c.GetComponent<BoxCollider2D>().bounds;
        Rect carBox = new Rect((Vector2)(carBounds.min), c.GetComponent<BoxCollider2D>().bounds.size);
        Rect ourBox = new Rect((Vector2)transform.position + new Vector2(-WIDTH/2 -HALO_SIZE, -HALO_SIZE), new Vector2(WIDTH+2*HALO_SIZE, forwardDistance+2*HALO_SIZE));

        return carBox.Overlaps(ourBox);
    }

    protected override Vector2 GetWorldLocation(World.WorldCoord c, World.WorldCoord direction)
    {
        return World.Instance.GetCenterGridWorldLocation(c);
    }

    protected override bool CanMoveInto(World.WorldCoord c, World.WorldCoord direction)
    {
        return true;
    }

    protected override void MoveInto(World.WorldCoord c, World.WorldCoord direction, GameObject go)
    {
        /* Do Nothing */
    }

    protected override void LeaveFrom(World.WorldCoord c, World.WorldCoord direction)
    {
        /* Do Nothing */
    }

    protected void unhingeSelf()
    {
        //Leave towards left
        direction = new World.WorldCoord(-1, 0);
        GetComponent<BoxCollider2D>().enabled = false;

        foreach(var car in inPath)
        {
            car.StopWaiting();
        }
    }

    protected override void OnDestinationReached()
    {
        if(worldLocation.y == World.WORLD_HEIGHT)
        {
            unhingeSelf();
        }

        if(worldLocation == leaveLocation)
        {
            unhinged = true;
        }
        else
        {
            MoveIfPossible(direction);
        }
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Car" || other.tag == "Player")
        {
            var car = other.gameObject.GetComponent<Car>();
            if(!CheckInFront(car))
            {
                car.WaitForPedestrian();
            }
            else
            {
                inPath.Add(car);
                car.StopWaiting();

            }
        }
    }
    protected void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Car" || other.tag == "Player")
        {
            var car = other.gameObject.GetComponent<Car>();
            car.StopWaiting();
            if(inPath.Contains(car))
                inPath.Remove(car);
        }
    }
}
