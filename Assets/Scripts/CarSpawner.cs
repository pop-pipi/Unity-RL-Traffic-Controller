using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    public IntersectionAgent agent;

    public List<CarController> cars = new List<CarController>();
    public GameObject carPrefab;
    public TrafficPath[] paths;
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
        foreach (TrafficPath p in paths) {
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
        TrafficPath path = (TrafficPath) openPaths[spawnIndex];

        // Instantiate car and set parameters
        var car = Instantiate(carPrefab, path.transform.position, Quaternion.identity);
        CarController carController = car.GetComponent<CarController>();
        carController.acceleration = carAcceleration;
        carController.maxSpeed = carMaxSpeed;
        carController.path = path;
        carController.idealSpaceToCarAhead = idealSpaceBetweenCars;
        carController.parent = this;
        cars.Add(carController);


        // TODO: Review ADD TO OBS VALUE
        path.noCars += 1;

        //agent.RequestDecision();
    }

}
