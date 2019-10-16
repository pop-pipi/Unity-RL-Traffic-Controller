using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntersectionController : MonoBehaviour
{

    public float yellowSignalTimer;
    public float timeBetweenConfigs;
    public TrafficConfiguration[] configurations;
    public ArrayList cars = new ArrayList();
    private int currentConfig;

    // ml agents stuff
    // private bool changingConfigurations;
    // public int currentConfig;

    // Start is called before the first frame update
    // Set all traffic light yellow signal lengths
    void Start()
    {
        setCurrentConfig(9);
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
        // check if agent is changing a new traffic config or remain the same config
        if(Input.anyKeyDown){
            Debug.Log("Player pressed: " + Input.inputString);
            if(getCurrentConfig() != Input.inputString){
                for (int i = 0; i < configurations.Length; i++)
                {
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
                        foreach (TrafficLight trafficLight in configurations[i].trafficLights)
                        {
                            trafficLight.Invoke("OpenTraffic", timeBetweenConfigs);
                        }
                        Debug.Log("Waiting cars: " + getQueueLength());
                        

                        // Update our current config
                        setCurrentConfig(i);

                        return;
                    }
                    
                }
            }
        }
    }

    // TODO: ml agents stuff
    // public void SwitchTrafficConfiguration(int index)
    // {
    //     if (!changingConfigurations && currentConfig != index)
    //     {
    //         changingConfigurations = true;
    //         currentConfig = index;

    //         // Close traffic in all lanes
    //         foreach (TrafficConfiguration configuration in configurations)
    //         {
    //             foreach (TrafficLight trafficLight in configuration.trafficLights)
    //             {
    //                 trafficLight.CloseTraffic();
    //             }
    //         }

    //         // Open traffic in lanes selected by User
    //         foreach (TrafficLight trafficLight in configurations[index].trafficLights)
    //         {
    //             trafficLight.Invoke("OpenTraffic", timeBetweenConfigs);
    //             Invoke("FinishedChangingLanes", timeBetweenConfigs);
    //         }
    //         return;
    //     }
    // }

    // private void FinishedChangingLanes()
    // {
    //     changingConfigurations = false;
    // }

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

    public string getCurrentConfig(){
        return currentConfig.ToString();
    }

    public void setCurrentConfig(int change){
        currentConfig = change;
        Debug.Log("changed current config to: " + getCurrentConfig());
    }
}