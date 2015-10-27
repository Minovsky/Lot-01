using UnityEngine;
using UnityEngine.Assertions;

using System.Collections;
using System.Collections.Generic;

public class Pedestrian : Moveable
{
    protected static World.WorldCoord leaveLocation = new World.WorldCoord(0, World.WORLD_HEIGHT-1);
    protected bool unhinged = false;

    private static readonly float OFFSCREEN_THRESHOLD = 10f;

    public GameObject trainExitLocation;

    public override void Start()
    {
        base.Start();

        var r = Random.Range(0, World.WORLD_WIDTH/2);
        var startTarget = new World.WorldCoord(r*2, 0);

        TeleportTo(startTarget, new World.WorldCoord(0, 1));
        transform.position = trainExitLocation.transform.position;
    }

    public override void Update()
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

    protected override Vector2 GetWorldLocation(World.WorldCoord c, World.WorldCoord direction)
    {
        return World.Instance.GetCenterGridWorldLocation(c);
    }

    protected override bool CanMoveInto(World.WorldCoord c, World.WorldCoord direction)
    {
        return World.WithinBounds(c) && !World.Instance.IsParkingSpot(c);
    }

    protected override void MoveInto(World.WorldCoord c, World.WorldCoord direction, GameObject go)
    {
        /* Do Nothing */
    }

    protected override void LeaveFrom(World.WorldCoord c, World.WorldCoord direction)
    {
        /* Do Nothing */
    }

    protected override void OnDestinationReached()
    {
        if(worldLocation.y == World.WORLD_HEIGHT-1)
        {
            //Leave towards left
            direction = new World.WorldCoord(-1, 0);
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
            other.gameObject.GetComponent<Car>().WaitForPedestrian();
        }
    }
    protected void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Car" || other.tag == "Player")
        {
            other.gameObject.GetComponent<Car>().StopWaiting();
        }
    }
}
