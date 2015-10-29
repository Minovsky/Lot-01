using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;
using System.Collections.Generic;

public class CarSpawner : MonoBehaviour {
    enum WAYPOINTS_TYPE {
        NORMAL = 0,
        Q4
    }; 
    
    public GameObject standInCarPrefab;

    private enum CAR_STATE {
        WAITING = 0,
        MOVING,
    };
    CAR_STATE carState = CAR_STATE.MOVING;

    private List<Vector2> waypoints = new List<Vector2>();
    private int idxDestWaypoint = 0;
    private float speed = 1;
    private Car npcCar;

    public void Start() {
        waypoints.Add(new Vector2(8, 4));
        waypoints.Add(new Vector2(7, 4));
        waypoints.Add(new Vector2(7, 3));
        waypoints.Add(new Vector2(6, 3));    
        GenerateCar();
    }

    public void Update() {
        MoveCar();
    }

    private void Rotate(Vector2 rotateVec) {
        npcCar.GetComponent<Transform>().rotation = new Quaternion(0, 0, 0, 0);
        Vector2 originalOrientationVec = new Vector2(1, 0);
        //float angle = Vector2.Angle(rotateVec, originalOrientationVec);
        if (rotateVec != Vector2.zero && originalOrientationVec != Vector2.zero) {
        //float angle = Mathf.Acos(Vector2.Dot(rotateVec, originalOrientationVec)/(rotateVec.sqrMagnitude * rotateVec.sqrMagnitude * originalOrientationVec.sqrMagnitude * originalOrientationVec.sqrMagnitude));
            float angle = Mathf.Acos((rotateVec.x * originalOrientationVec.x + rotateVec.y * originalOrientationVec.y) / (rotateVec.magnitude * originalOrientationVec.magnitude));
            angle = angle * Mathf.Rad2Deg;
            Debug.Log(rotateVec + " " + originalOrientationVec + " " + angle);
        npcCar.GetComponent<Transform>().Rotate(0, 0, -angle);
        }   
    }

    private void GenerateCar() {
        npcCar = ((GameObject)Instantiate(standInCarPrefab, Vector2.zero, Quaternion.identity)).GetComponent<StandInCar>();
        npcCar.name = "testCar";
        npcCar.GetComponent<Transform>().position = waypoints[0];
        idxDestWaypoint = 0;
        carState = CAR_STATE.MOVING;
    }

    private void MoveCar() {
        if (carState == CAR_STATE.WAITING) 
        {
            return;
        }
        Vector3 position = npcCar.GetComponent<Transform>().position;
        position = Vector2.MoveTowards(position, waypoints[idxDestWaypoint], speed*Time.deltaTime);
        npcCar.GetComponent<Transform>().position = position;

        if (position == (Vector3) waypoints[idxDestWaypoint]) {
            if (idxDestWaypoint >= waypoints.Count - 1) {
                carState = CAR_STATE.WAITING;
            } else {
                idxDestWaypoint ++;
            }
        } else {

            // Rotate it
            Vector2 shift = new Vector2();
            shift = waypoints[idxDestWaypoint] - (Vector2)npcCar.GetComponent<Transform>().position;
            Rotate(shift);
        }
    }
}
