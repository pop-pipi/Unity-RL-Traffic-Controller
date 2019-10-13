using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficPath : MonoBehaviour
{
    public TrafficLight trafficLight;
    public Transform[] waypoints;

    // TODO: Review TEST OBS VALUE
    public float noCars;

    private bool spawnOccupied;

    public bool IsSpawnOccupied()
    {
        return spawnOccupied;
    }

    private void OnTriggerEnter(Collider other)
    {
        spawnOccupied = true;
    }

    private void OnTriggerExit(Collider other)
    {
        spawnOccupied = false;
    }

}
