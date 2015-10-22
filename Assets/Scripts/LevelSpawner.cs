using UnityEngine;
using System.Collections;

public class LevelSpawner : MonoBehaviour
{
    public GameObject npcCarPrefab;
    public GameObject playerCarPrefab;

    public uint numberOfNPCs = 8;

    // Use this for initialization
    void Start ()
    {
        Car newCar = ((GameObject)Instantiate(npcCarPrefab, Vector2.zero, Quaternion.identity)).GetComponent<Car>();

        World.WorldCoord teleLocation = new World.WorldCoord(0, 0);
        World.WorldCoord teleDir = new World.WorldCoord(1, 0);
        newCar.TeleportTo(teleLocation, teleDir);
    }
}
