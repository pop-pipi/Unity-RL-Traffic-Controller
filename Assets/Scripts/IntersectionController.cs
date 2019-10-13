using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntersectionController : MonoBehaviour
{

    public float yellowSignalTimer;
    public float timeBetweenConfigs;
    public TrafficConfiguration[] configurations;
    public ArrayList cars = new ArrayList();

    // Start is called before the first frame update
    // Set all traffic light yellow signal lengths
    void Start()
    {
        for (int i = 0; i < configurations.Length; i++)
        {
            TrafficConfiguration config = configurations[i];
            for (int a = 0; a < config.trafficLights.Length; a++)
            {
                config.trafficLights[a].yellowSignalTimer = yellowSignalTimer;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < configurations.Length; i++)
        {
            if(Input.anyKeyDown) {
                if (Input.inputString == i.ToString())
                {

                    // Close traffic in all lanes
                    foreach (TrafficConfiguration configuration in configurations)
                    {
                        foreach (TrafficLight trafficLight in configuration.trafficLights)
                        {
                            trafficLight.CloseTraffic();
                        }
                        
                    }

                    // Open traffic in lanes selected by User
                    // TODO: Set to open AFTER a specfied time
                    foreach (TrafficLight trafficLight in configurations[i].trafficLights)
                    {
                        trafficLight.Invoke("OpenTraffic", timeBetweenConfigs);
                    }
                    Debug.Log("Waiting cars: " + getQueueLength());
                    return;
                }
            }
        }
    }

    public int getQueueLength(){
        int waitingcars = 0;
        foreach(CarController car in cars){
            if(car.speed == 0.0)
            {
                waitingcars += 1;
            }
        }
        return waitingcars;
    }
}