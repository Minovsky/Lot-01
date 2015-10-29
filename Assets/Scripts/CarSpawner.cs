using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;
using System.Collections.Generic;

public class CarSpawner : MonoBehaviour {
    
    public GameObject standInCarPrefab;
    public GameObject npcCarPrefab;

    private enum CAR_STATE {
        WAITING = 0,
        MOVING,
    };
    CAR_STATE carState = CAR_STATE.WAITING;

    private List<Vector2> waypoints = new List<Vector2>();
    private int idxDestWaypoint = 0;
    private float speed = 1;
    private StandInCar comeInCar;
    private int numCars = 0;

    private static CarSpawner _instance = null;
    public static CarSpawner Instance {
        get {
            if (_instance == null) {
                _instance = FindObjectOfType<CarSpawner>();
            }
            return _instance;
        }
    }

    public void Start() {
//        waypoints.Add(new Vector2(10, 4));
//        waypoints.Add(new Vector2(7, 4));
//        waypoints.Add(new Vector2(7, 3.5f));
//        waypoints.Add(new Vector2(5.47f, 3.5f));    
        waypoints.Add(new Vector2(10, 4));
        waypoints.Add(new Vector2(8, 4));
        waypoints.Add(new Vector2(7, 3.8f));
        waypoints.Add(new Vector2(6, 3.6f));
        waypoints.Add(new Vector2(5.8f, 3.5f));
        waypoints.Add(new Vector2(5.47f, 3.5f));
    }

    public void Update() {
        MoveCar();
    }

    private void Rotate(Vector2 rotateVec) {
        comeInCar.GetComponent<Transform>().rotation = new Quaternion(0, 0, 0, 0);
        Vector2 originalOrientationVec = new Vector2(1, 0);
        //float angle = Vector2.Angle(rotateVec, originalOrientationVec);
        if (rotateVec != Vector2.zero && originalOrientationVec != Vector2.zero) {
        //float angle = Mathf.Acos(Vector2.Dot(rotateVec, originalOrientationVec)/(rotateVec.sqrMagnitude * rotateVec.sqrMagnitude * originalOrientationVec.sqrMagnitude * originalOrientationVec.sqrMagnitude));
            float angle = Mathf.Acos((rotateVec.x * originalOrientationVec.x + rotateVec.y * originalOrientationVec.y) / (rotateVec.magnitude * originalOrientationVec.magnitude));
            angle = angle * Mathf.Rad2Deg;
            //Debug.Log(rotateVec + " " + originalOrientationVec + " " + angle);
        comeInCar.GetComponent<Transform>().Rotate(0, 0, -angle);
        }   
    }

    public void AddCar() {
        //Debug.Log("yes");
        numCars ++;
    }
    private void GenerateCar() {
        comeInCar = ((GameObject)Instantiate(standInCarPrefab, Vector2.zero, Quaternion.identity)).GetComponent<StandInCar>();
        comeInCar.name = "testCar";
        comeInCar.GetComponent<Transform>().position = waypoints[0];
        idxDestWaypoint = 0;
        carState = CAR_STATE.MOVING;
    }

    private void MoveCar() {
        if (carState == CAR_STATE.WAITING) 
        {
            if (numCars > 0) {
                GenerateCar();
                numCars --;
            }
            return;
        }
        Vector3 position = comeInCar.GetComponent<Transform>().position;
        position = Vector2.MoveTowards(position, waypoints[idxDestWaypoint], speed*Time.deltaTime);
        comeInCar.GetComponent<Transform>().position = position;
        // comeInCar.Start();

        if (position == (Vector3) waypoints[idxDestWaypoint]) {
            if (idxDestWaypoint >= waypoints.Count - 1) {
                carState = CAR_STATE.WAITING;
                MoveIntoParkingArea(comeInCar.color);
                Destroy(comeInCar.gameObject);
            } else {
                idxDestWaypoint ++;
            }
        } else {
            // Rotate it
            Vector2 shift = new Vector2();
            shift = waypoints[idxDestWaypoint] - (Vector2)comeInCar.GetComponent<Transform>().position;
            Rotate(shift);
        }
    }

    private void MoveIntoParkingArea(int color) {
        NPCCar newCar = ((GameObject)Instantiate(npcCarPrefab, Vector2.zero, Quaternion.identity)).GetComponent<NPCCar>();  
        newCar.name = "NewCar";
        newCar.ChangeColor(color);
        newCar.TeleportTo(new World.WorldCoord(World.WORLD_WIDTH -1, World.WORLD_HEIGHT-1), new World.WorldCoord(-1, 0));
    }
}
