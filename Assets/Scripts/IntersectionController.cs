using UnityEngine;

public class IntersectionController : MonoBehaviour
{

    public TrafficConfiguration[] configurations;

    // Start is called before the first frame update
    void Start()
    {
        
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
                        trafficLight.OpenTraffic();
                    }
                    return;
                }
            }
        }
    }
}