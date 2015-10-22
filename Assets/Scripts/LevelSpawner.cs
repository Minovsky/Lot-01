using UnityEngine;
using System.Collections;

public class LevelSpawner : MonoBehaviour
{
    private static readonly int MAX_GUESSES = 3000;

    public GameObject npcCarPrefab;
    public GameObject playerCarPrefab;

    public uint numberOfNPCs = 8;

    // Use this for initialization
    void Start ()
    {
        for(int i = 0; i < numberOfNPCs; i++)
        {
            SpawnRandomCar();
        }
    }

    private void SpawnRandomCar()
    {
        World.WorldCoord teleLocation = new World.WorldCoord(Random.Range(0, World.WORLD_WIDTH), Random.Range(0, World.WORLD_HEIGHT));
        World.WorldCoord teleDir = World.POSSIBLE_DIRECTIONS[Random.Range(0, World.POSSIBLE_DIRECTIONS.Length)];
        int count = 0;
        while(!World.Instance.CanMoveInto(teleLocation, teleDir)
                || World.Instance.IsParkingSpot(teleLocation)
                || count >= MAX_GUESSES)
        {
            count++;
            teleLocation = new World.WorldCoord(Random.Range(0, World.WORLD_WIDTH), Random.Range(0, World.WORLD_HEIGHT));
            teleDir = World.POSSIBLE_DIRECTIONS[Random.Range(0, World.POSSIBLE_DIRECTIONS.Length)];
        }

        if(count < MAX_GUESSES)
        {
            Car newCar = ((GameObject)Instantiate(npcCarPrefab, Vector2.zero, Quaternion.identity)).GetComponent<Car>();
            newCar.TeleportTo(teleLocation, teleDir);
        }
    }
}
