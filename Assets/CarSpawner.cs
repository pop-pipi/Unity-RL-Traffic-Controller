using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    public GameObject carPrefab;
    public Path[] paths;
    public float spawnRate; // Seconds between each car spawn
    public float carMaxSpeed;
    public float carAcceleration; // Accelleration rate for accelarating & decellerating
    public float idealSpaceBetweenCars;

    void Start()
    {
        // Spawn cars repeatedly
        InvokeRepeating("SpawnCar", 1, spawnRate);
    }

    private void SpawnCar()
    {
        // Get free paths
        ArrayList openPaths = new ArrayList();
        foreach (Path p in paths) {
            if (!p.IsSpawnOccupied())
            {
                openPaths.Add(p);
            }
        }

        // If no open spawns return
        if(openPaths.Count == 0)
        {
            return;
        }

        // Assign random open spawn
        System.Random rnd = new System.Random();
        int spawnIndex = rnd.Next(openPaths.Count);
        Path path = (Path) openPaths[spawnIndex];

        // Instantiate car and set parameters
        var car = Instantiate(carPrefab, path.transform.position, Quaternion.identity);
        CarController carController = car.GetComponent<CarController>();
        carController.acceleration = carAcceleration;
        carController.maxSpeed = carMaxSpeed;
        carController.path = path;
        carController.idealSpaceToCarAhead = idealSpaceBetweenCars;
    }

}
