using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

public class PlayerCar : Car
{

    private enum MOVE_ACTION {FORWARD=0, RIGHT, LEFT};
    private MOVE_ACTION nextMove;
    private bool resetAction = true;

    // Use this for initialization
    public override void Start ()
    {
        nextMove = MOVE_ACTION.FORWARD;
    }

    public override void Update()
    {
        if(Input.GetAxis("Horizontal") > 0)
        {
            if(resetAction)
            {
                nextMove = MOVE_ACTION.LEFT;
                resetAction = false;
            }
        }
        else if(Input.GetAxis("Horizontal") < 0)
        {
            if(resetAction)
            {
                nextMove = MOVE_ACTION.RIGHT;
                resetAction = false;
            }
        }
        else
        {
            resetAction = true;
        }
        base.Update();
    }

    private void MoveBasedOnInput()
    {
        World.WorldCoord move;
        uint wrapIndex = (uint)Array.IndexOf(World.POSSIBLE_DIRECTIONS, direction);
        switch(nextMove)
        {
            case MOVE_ACTION.LEFT:
                move = World.POSSIBLE_DIRECTIONS[(wrapIndex-1) % World.POSSIBLE_DIRECTIONS.Length];
                break;
            case MOVE_ACTION.RIGHT:
                move = World.POSSIBLE_DIRECTIONS[(wrapIndex+1) % World.POSSIBLE_DIRECTIONS.Length];
                break;
            default:
            case MOVE_ACTION.FORWARD:
                move = direction;
                break;
        }
        if(World.Instance.CanMoveInto(worldLocation+move, move))
        {
            MoveIfPossible(move);
        }
        else
        {
            //Move forward
            MoveIfPossible(direction);
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
            MoveBasedOnInput();
        }
        nextMove = MOVE_ACTION.FORWARD;
    }

    protected override void OnParked()
    {
    }
}
