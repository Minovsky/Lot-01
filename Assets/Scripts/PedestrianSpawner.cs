using UnityEngine;
using System.Collections;

public class PedestrianSpawner : MonoBehaviour
{
    public float pedestrianSpawnLength = 20f;
    public float pedestrianSpawnTime = 1f;
    public GameObject pedestrian;

    private Coroutine spawnerRoutine = null;

    // Use this for initialization
    void Start ()
    {
        StartSpawning();
    }

    // Update is called once per frame
    void Update ()
    {
    }

    private IEnumerator SpawnAtInterval(float interval)
    {
        float curLength = pedestrianSpawnLength;
        while((curLength -= interval) > 0)
        {
            yield return new WaitForSeconds(interval);

            Instantiate(pedestrian, Vector3.zero, Quaternion.identity);
        }
    }

    public void StartSpawning()
    {
        StopSpawning();
        spawnerRoutine = StartCoroutine(SpawnAtInterval(pedestrianSpawnTime));
    }

    public void StopSpawning()
    {
        if(spawnerRoutine != null)
        {
            StopCoroutine(spawnerRoutine);
        }
    }
}
