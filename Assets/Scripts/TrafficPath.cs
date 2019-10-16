using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficPath : MonoBehaviour
{

    private int numcars = 0;
    public TrafficLight trafficLight;
    public Transform[] waypoints;

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

    public int getNumCars(){
        return numcars;
    }

    public void addCar(){
        numcars += 1;
    }

    public void removeCar(){
        numcars -= 1;
    }

}
